using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed;
    public GameObject currentTile;
    public float directionX = 1.0f;
    public float directionY = 1.0f;
    public bool enableRotate;
    public int OriginalPhase;

    private Transform current_t;
    private GameObject nextTile;
    private Transform next_t;
    private Tile tile;
    private Tile tile_nextTile;
    private bool move;
    private Vector3 distance;
    private float prevAngleF;           // flashlight
    private float prevAngleM;           // player
    private int prevPhase;              // four directions, o - top right, 1 - bottom right, 2 - top left , 3 - bottom left

    public PlayerMovement my_movement;
    public GameObject my_flashight;         // so we can switch between different flashlights
    private Inventory my_inventory;
    private Animator my_anim;
    private pauseMenu game_menu;

	void Start ()
    {
        game_menu = GameObject.FindGameObjectWithTag("Menu Canvas").GetComponent<pauseMenu>();

        my_anim = GetComponent<Animator>();
        current_t = currentTile.GetComponent<Transform>();
        my_inventory = GetComponent<Inventory>();
        tile = currentTile.GetComponent<Tile>();

        nextTile = null;
        move = false;
        enableRotate = false;

        prevAngleF = my_flashight.GetComponent<Flashlight>().angle;
        prevPhase = OriginalPhase;
        SetDirection(OriginalPhase);

        //tile.DebugGetAllTile();
    }
	
	void Update ()
    {

        //only move when play select a valid tile
        if (move && PlayerTurn.playerTurn)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, distance, step);
            //update flashlight here so it will only call the function when player moves
            my_flashight.GetComponent<Flashlight>().FlashlightFollowPlayer();

            if (Vector3.Distance(transform.position,distance) < 0.001f)
			{
                PickUpItem();
                CheckEndGame();
				move = false;
                my_anim.SetBool("Move", false);
                PlayerTurn.SetPlayerTurn();
                prevAngleF = my_flashight.GetComponent<Flashlight>().angle;
                prevPhase = my_movement.GetPhase();
            }
        }

        //place and pick up flashlight
        if (Input.GetMouseButtonDown(1) && my_flashight)
        {
            if (enableRotate)
            {
                //place
                if (!my_flashight.GetComponent<Flashlight>().IsPlaced())
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (my_flashight.GetComponent<Flashlight>().GetPointedTile())
                        {
                            if (tile.GetAdjacentTileT(i) == my_flashight.GetComponent<Flashlight>().GetPointedTile())
                            {
                                my_anim.SetBool("PickUp", true);
                                tile.PlaceFlashlight(my_flashight);
                                my_flashight.GetComponent<Flashlight>().Place(my_movement.GetPhase());
                                enableRotate = false;
                                prevAngleM = my_movement.angle;
                                prevAngleF = my_flashight.GetComponent<Flashlight>().angle;
                                prevPhase = my_movement.GetPhase();
                                SoundManager.instance.PlaySFX(5);
                            }
                        }
                    }
                }
            }
            //pick up
            else if(my_flashight.GetComponent<Flashlight>().IsPlaced())
            {
                //check if player is on the tile with the flashlight
                Debug.Log(tile.flashlightPlaced);
                if (tile.flashlightPlaced)
                {
                    my_anim.SetBool("PickUp", false);
                    my_flashight = tile.PickUpFlashlight();
                    my_flashight.GetComponent<Flashlight>().PickUp();
                    my_flashight.GetComponent<Flashlight>().angle = prevAngleM;
                    SoundManager.instance.PlaySFX(4);
                }
            }
            else
            {
                enableRotate = true;
            }
            
        }

        if (Input.GetKey(KeyCode.R))
        {
            PlayerTurn.Restart();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            LevelComplete();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!enableRotate)
            {
                my_movement.SelectThisTile();
            }
            else
            {
                enableRotate = false;
                SetDirection(prevPhase);
                my_flashight.GetComponent<Flashlight>().SetAngle(prevAngleF);
            }
        }

        // turn on
        if(Input.GetAxis("Mouse ScrollWheel") > 0.0f)
        {
            if (!my_flashight.GetComponent<Flashlight>().IsOn())
            {
                SoundManager.instance.PlaySFX(7);
            }
            my_flashight.GetComponent<Flashlight>().TurnOn();
        }
        // turn off
        else if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
        {
            if (my_flashight.GetComponent<Flashlight>().IsOn())
            {
                SoundManager.instance.PlaySFX(6);
            }
            my_flashight.GetComponent<Flashlight>().TurnOff();
        }

    }

    public void SelectTile(GameObject selectedTile)
    {
        bool found = false;
        if (!move)
        {
            for (int i = 0; i < 4; i++)
            {
                found = false;
                if (tile.GetAdjacentTile(i) == selectedTile)
                {
                    //assign nextTile and get Tile component
                    nextTile = selectedTile;
                    tile_nextTile = selectedTile.GetComponent<Tile>();
                    found = true;

                    //adjacent tile and is empty
                    if (tile_nextTile.IsEmpty())
                    {
                        found = true;
                        //enable move
                        move = true;
                        my_anim.SetBool("Move", true);

                        prevAngleM = my_movement.angle;

                        tile.playerOn = false;
                        tile_nextTile.playerOn = true;

                        //get Transform
                        next_t = nextTile.GetComponent<Transform>();
                        CalculateDis();
                        SelectDirections();
                        NextTurn();

                        SoundManager.instance.PlaySFX(0);
                    }
                }
            }
            //if move is not enable, tell the player that they can't move to the tile
            if (!move)
            {
                if (found)
                {
                    if (!tile_nextTile.IsEmpty())
                    {
                        Debug.Log("Tile is not empty");
                    }
                }
                else
                {
                    Debug.Log("Not adjacent tile");
                }

            }
        }
        
    }

    void CalculateDis()
    {
        //calculate which way to move to
        Vector3 tmp = next_t.position - current_t.position;
        distance = transform.position + tmp;
        directionX = tmp.x;
        directionY = tmp.y;
    }

    void NextTurn()
    {
        //change currentTile to nextTile
        //change nextTile to null
        currentTile = nextTile;
        current_t = nextTile.GetComponent<Transform>();
        tile = nextTile.GetComponent<Tile>();

        nextTile = null;
        next_t = null;
        tile_nextTile = null;
    }

    void PickUpItem()
    {
        if (tile.item)
        {
            my_inventory.TakeItem(tile.item);
            tile.GetItem();
        }
    }

    void CheckEndGame()
    {
        if ((currentTile == my_inventory.endTile) && my_inventory.allItem && !my_flashight.GetComponent<Flashlight>().IsPlaced())
        {
            LevelComplete();
        }
    }

    void SelectDirections()
    {
        if (directionX > 0 && directionY > 0)
        {
            my_anim.SetInteger("Phase", 0);
        }
        else if (directionX > 0 && directionY < 0)
        {
            my_anim.SetInteger("Phase", 1);
        }
        else if (directionX < 0 && directionY > 0)
        {
            my_anim.SetInteger("Phase", 2);
        }
        else if (directionX < 0 && directionY < 0)
        {
            my_anim.SetInteger("Phase", 3);
        }
    }

    public void SetDirection(int directionIndex)
    {
        my_anim.SetInteger("Phase", directionIndex);
    }

    public GameObject GetPlayerCurrentTile()
    {
        return currentTile;
    }

    public bool IsMove()
    {
        return move;
    }

    void LevelComplete()
    {
        Debug.Log("Level complete");
        PlayerTurn.GameOver = true;
        stageSelectController.stageClear[SceneManager.GetActiveScene().buildIndex - 1] = 1; // set next level to unlocked
        game_menu.Victory();
    }
}
