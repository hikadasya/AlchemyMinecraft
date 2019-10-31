using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonBlockHandler : MonoBehaviour
{
    //--Keycodes for element equips
    [SerializeField] KeyCode keySlot1 = KeyCode.Alpha1;
    [SerializeField] KeyCode keySlot2 = KeyCode.Alpha2;
    [SerializeField] KeyCode keySlot3 = KeyCode.Alpha3;
    [SerializeField] KeyCode keySlot4 = KeyCode.Alpha4;
    [SerializeField] KeyCode keySlot5 = KeyCode.Alpha5;
    [SerializeField] KeyCode keySlot6 = KeyCode.Alpha6;
    [SerializeField] KeyCode keySlot7 = KeyCode.Alpha7;
    [SerializeField] KeyCode keySlot8 = KeyCode.Alpha8;
    [SerializeField] KeyCode keySlot9 = KeyCode.Alpha9;

    //--For separating the blocks on the grid
    [SerializeField] float gridSize = 0.8f;
    [SerializeField] int maxHeight = 4;

    //--The player's camera
    Camera cam;

    //--Some extra things we need
    GameObject gameController; //Holds all that good good stuff thats not attached to actual things. Like databases and stuff like that
    Transform hand; //Where the blocks are held (the parent object)
    ElementDatabase db; //Holds all the element data
    GameObject environment; //Where the blocks get added
    BlockDatabase blockLocations; //The locations of all the blocks that have been placed

    int slotSelected = 0; //The slot the player has selected

    Vector3 mousePos; //Where the mouse is

    // Start is called before the first frame update
    void Start()
    {
        hand = GameObject.FindGameObjectWithTag("Hand").transform;
        gameController = GameObject.FindGameObjectWithTag("GameController");
        db = gameController.GetComponent<ElementDatabase>();
        cam = GameObject.FindGameObjectWithTag("FPSCam").GetComponent<Camera>();
        environment = GameObject.FindGameObjectWithTag("Environment");
        blockLocations = gameController.GetComponent<BlockDatabase>();
    }

    // Update is called once per frame
    void Update()
    {
        //RaycastHit hit;
        //Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        //
        //if (Physics.Raycast(ray, out hit))
        //{
        //    mousePos = hit.point;
        //}

        //HoldBlock(); //Put it in your hand
        //PlaceBlock(); //Put it somewhere in the world
    }       
}
