using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodBlockPlacement : MonoBehaviour
{
    [SerializeField] KeyCode keySlot1 = KeyCode.Alpha1;
    [SerializeField] KeyCode keySlot2 = KeyCode.Alpha2;
    [SerializeField] KeyCode keySlot3 = KeyCode.Alpha3;
    [SerializeField] KeyCode keySlot4 = KeyCode.Alpha4;
    [SerializeField] KeyCode keySlot5 = KeyCode.Alpha5;
    [SerializeField] KeyCode keySlot6 = KeyCode.Alpha6;
    [SerializeField] KeyCode keySlot7 = KeyCode.Alpha7;
    [SerializeField] KeyCode keySlot8 = KeyCode.Alpha8;
    [SerializeField] KeyCode keySlot9 = KeyCode.Alpha9;

    int slotSelected = 0;

    [SerializeField] float blockSize = 0.8f;
    Camera cam;

    GameObject gameController;
    Transform hand;
    ElementDatabase db;
    GameObject environment;

    BlockDatabase blockLocations;

    Vector3 dragStartLoc;
    Vector3 dragEndLoc;
    bool draggingBlock = false;

    [SerializeField] LayerMask layer;
    int layerMask;

    void Awake()
    {
        hand = GameObject.FindGameObjectWithTag("GodHand").transform;
        gameController = GameObject.FindGameObjectWithTag("GameController");
        db = gameController.GetComponent<ElementDatabase>();
        cam = GameObject.FindGameObjectWithTag("GodCam").GetComponent<Camera>();
        environment = GameObject.FindGameObjectWithTag("Environment");
        blockLocations = gameController.GetComponent<BlockDatabase>();

        layerMask = layermask_to_layer(layer);
    }

    // Update is called once per frame
    void Update()
    {
        HoldBlock();
    }

    //Actually Place Blocks

    //Holding Block and switching between the different ones in your equip slots
    void HoldBlock()
    {
        //Check which slot the player wants to use
        //Future me: dear god please figure out how to simplify this because it's disgusting.
        if (!draggingBlock)
        {
            if (Input.GetKeyDown(keySlot1))
            {
                slotSelected = 1;
            }
            else if (Input.GetKeyDown(keySlot2))
            {
                slotSelected = 2;
            }
            else if (Input.GetKeyDown(keySlot3))
            {
                slotSelected = 3;
            }
            else if (Input.GetKeyDown(keySlot4))
            {
                slotSelected = 4;
            }
            else if (Input.GetKeyDown(keySlot5))
            {
                slotSelected = 5;
            }
            else if (Input.GetKeyDown(keySlot6))
            {
                slotSelected = 6;
            }
            else if (Input.GetKeyDown(keySlot7))
            {
                slotSelected = 7;
            }
            else if (Input.GetKeyDown(keySlot8))
            {
                slotSelected = 8;
            }
            else if (Input.GetKeyDown(keySlot9))
            {
                slotSelected = 9;
            }
            else
            {
                //Scroll Up and Down to change slot
                if (Input.mouseScrollDelta.y > 0)
                {
                    slotSelected--;
                    if (slotSelected < 1)
                    {
                        slotSelected = 9;
                    }
                }
                else if (Input.mouseScrollDelta.y < 0)
                {
                    slotSelected++;
                    if (slotSelected > 9)
                    {
                        slotSelected = 1;
                    }
                }
            }
        }

        //If they selected a valid slot
        if (slotSelected > 0)
        {
            GameObject slot = GameObject.Find("Slot " + slotSelected);
            int id = slot.GetComponent<ElementID>().elementID;

            //If they selected a slot WITH an element in it
            if (id > 0)
            {
                //If they are already holding a block
                if (hand.childCount > 0)
                {
                    //If the block is different from the one you're holding
                    if (hand.GetComponentInChildren<ElementID>().elementID != id)
                    {
                        Destroy(hand.GetComponentInChildren<ElementID>().gameObject); //Destroy the old one

                        //Make the new one
                        GameObject newObject;
                        newObject = (GameObject)Instantiate(db.GetElementByID(id).GetWorldPrefab(), hand);
                    }
                }
                else //If they're not holding a block
                {
                    //Just make the block
                    GameObject newObject;
                    newObject = (GameObject)Instantiate(db.GetElementByID(id).GetWorldPrefab(), hand);
                }
            }
            else if (hand.childCount > 0)
            {
                Destroy(hand.GetComponentInChildren<ElementID>().gameObject); //Destroy the old one
            }

        }
    }



    //Taken from https://answers.unity.com/questions/1288179/layer-layermask-which-is-set-in-inspector.html
    public static int layermask_to_layer(LayerMask l)
    {
        int layerNumber = 0;
        int layer = l.value;
        while (layer > 0)
        {
            layer = layer >> 1;
            layerNumber++;
        }
        return layerNumber - 1;
    }
}
