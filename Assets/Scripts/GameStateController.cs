using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    enum GAME_STATE
    {
        NONE = -1,
        INVENTORY = 0,
        GOD = 1,
        FPS = 2
    }

    //---Game States
    [SerializeField] GAME_STATE gameState = GAME_STATE.NONE;
    [SerializeField] GAME_STATE previousGameState = GAME_STATE.GOD;

    //---Key bindings
    [SerializeField] KeyCode toggleInventory = KeyCode.E;
    [SerializeField] KeyCode toggleGodMode = KeyCode.T;

    //---Game Objects
    //UI
    GameObject uiCanvasWrapper; //The inventory
    //Cameras
    Camera godCamera; //Camera for Godmode
    Camera playerCamera; //Camera for first person mode
    //Extras
    GameObject godHand; //where the blocks are held in god mode
    GameObject playerHand; //where the blocks are held in first person mode
    FirstPersonPlayerController playerController; //player movement & rotation
    Canvas reticalCanvas;
    //GodCameraController godController; //We don't need the GodCameracontroller object atm because that is all handled in the actual camera itself, eg "godCamera", not on the gameobject it's attached to.

    private void Awake()
    {
        //Init canvas things
        uiCanvasWrapper = GameObject.FindGameObjectWithTag("ElementUI");
        reticalCanvas = GameObject.FindGameObjectWithTag("ReticalCanvas").GetComponent<Canvas>();

        //Init camera things
        godCamera = GameObject.FindGameObjectWithTag("GodCam").GetComponent<Camera>();
        playerCamera = GameObject.FindGameObjectWithTag("FPSCam").GetComponent<Camera>();

        //Init extra gameObjects and things
        godHand = GameObject.FindGameObjectWithTag("GodHand");
        playerHand = GameObject.FindGameObjectWithTag("Hand");
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonPlayerController>();
    }

    private void Start()
    {
        UpdateState(GAME_STATE.INVENTORY);
    }

    private void Update()
    {
        if(Input.GetKeyDown(toggleInventory)) //If someone toggles the inventory
        {
            if (gameState == GAME_STATE.INVENTORY) //If they're already in the inventory, return them to whatever state they were before
            {
                //Debug.Log("Toggle Inven");
                UpdateState(previousGameState);
            }
            else //Otherwise, open the inventory
            {
                UpdateState(GAME_STATE.INVENTORY);
            }
        }
        else if (Input.GetKeyDown(toggleGodMode)) //If someone toggles Godmode
        {
            if (gameState != GAME_STATE.INVENTORY) //If theyre not in the inventory atm
            {
                if (gameState == GAME_STATE.GOD) //If theyre already in Godmode, switch them to first person mode
                {
                    UpdateState(GAME_STATE.FPS);
                }
                else //If theyre in first person mode, switch them to Godmode 
                {
                    UpdateState(GAME_STATE.GOD);
                }
            }
        }
    }

    //Updates the state of the game (Using inventory, interacting with the world in god mode, interacting with the world in first person, etc)
    private void UpdateState(GAME_STATE state)
    {
        //Update the variables
        previousGameState = gameState;
        gameState = state;

        //Call the appropriate functions to initialize the new game state
        switch (gameState)
        {
            case GAME_STATE.INVENTORY: InitInventoryState();
                break;
            case GAME_STATE.GOD: InitGodState();
                break;
            case GAME_STATE.FPS: InitFPSState();
                break;
        }
    }

    //Initialize the Inventory state
    private void InitInventoryState()
    {
        //Disabling Things        
        GodCamActive(false);    //We don't need any cameras
        PlayerCamActive(false); //We don't need any cameras
        PlayerControllerActive(false); //Deactivate the player controller
        GodHandActive(false); //Deactivate the God hand
        PlayerHandActive(false); //Deactivate the player hand
        ReticalActive(false); //Deactivate the retical

        //Enabling things
        InventoryActive(true); //Show the inventory

        //Cursor things
        Cursor.visible = true; //We want to see the cursor
        Cursor.lockState = CursorLockMode.None; //Unity is weird and it doesn't consistently switch from "Locked" to "Confined" & vice versa, but setting it to "None" first clears it out and prevents issues
        Cursor.lockState = CursorLockMode.Confined; //We want it confined to the game screen
    }

    //Initialize the God Mode state
    private void InitGodState()
    {
        //Disabling things
        InventoryActive(false); //Get rid of that inventory
        PlayerCamActive(false); //Get rid of that player camera
        PlayerControllerActive(false); //Deactivate the player controller        
        PlayerHandActive(false); //Deactivate the player hand
        ReticalActive(false); //Deactivate the retical

        //Enabling things
        GodCamActive(true);  //Activate the god cam
        GodHandActive(true); //Activate the god hand for placing blocks

        //Cursor things
        //Cursor.visible = false; //We don't want to see the cursor
        Cursor.lockState = CursorLockMode.None; //Unity is weird and it doesn't consistently switch from "Locked" to "Confined" & vice versa, but setting it to "None" first clears it out and prevents issues
        Cursor.lockState = CursorLockMode.Confined; //Confine it to the game screen, but don't lock it to the center because that messes with placing blocks
    }

    //Initialize the First Person state
    private void InitFPSState()
    {
        //Disabling things
        InventoryActive(false); //Get rid of that inventory
        GodCamActive(false); //Get rid of that God camera
        GodHandActive(false); //Deactivate the God hand

        //Enabling things
        PlayerCamActive(true); //Activate the player cam
        PlayerHandActive(true); //Activate the player hand for placing blocks
        PlayerControllerActive(true); //Activate the player controller

        //Cursor things
        ReticalActive(true); //Activate the retical
        Cursor.visible = false; //We don't want to see the cursor
        Cursor.lockState = CursorLockMode.None; //Unity is weird and it doesn't consistently switch from "Locked" to "Confined" & vice versa, but setting it to "None" first clears it out and prevents issues
        Cursor.lockState = CursorLockMode.Locked; //This is first person mode, so we want to lock the cursor to the center of the screen
    }

    //--------Activate or deactive certain gameObjects/UIs/cameras------------//
    private void InventoryActive(bool val)
    {
        uiCanvasWrapper.SetActive(val);
    }
    private void GodCamActive(bool val)
    {
        godCamera.gameObject.SetActive(val);
    }
    private void GodHandActive(bool val)
    {
        godHand.SetActive(val);
    }
    private void PlayerCamActive(bool val)
    {
        playerCamera.gameObject.SetActive(val);
    }
    private void PlayerControllerActive(bool val)
    {
        playerController.enabled = val;
    }
    private void PlayerHandActive(bool val)
    {
        playerHand.SetActive(val);
    }
    private void ReticalActive(bool val)
    {
        reticalCanvas.enabled = val;
    }

}
