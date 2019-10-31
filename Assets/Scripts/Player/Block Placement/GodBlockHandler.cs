using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GodBlockHandler : MonoBehaviour
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

    [SerializeField] float blockSize = 0.8f;
    [SerializeField] float groundSize = 0.1f;
    [SerializeField] int maxHeight = 4;

    [SerializeField] LayerMask layer;
    int layerMask;

    int slotSelected = 0;

    Camera cam;

    GameObject gameController;
    Transform hand;
    ElementDatabase db;
    GameObject environment;

    Vector3 mousePos;

    BlockDatabase blockLocations;
    
    Vector3 dragStartLoc;
    Vector3 dragEndLoc;
    bool draggingBlock = false;

    private void Awake()
    {
        hand = GameObject.FindGameObjectWithTag("GodHand").transform;
        gameController = GameObject.FindGameObjectWithTag("GameController");
        db = gameController.GetComponent<ElementDatabase>();
        cam = GameObject.FindGameObjectWithTag("GodCam").GetComponent<Camera>();
        environment = GameObject.FindGameObjectWithTag("Environment");
        blockLocations = gameController.GetComponent<BlockDatabase>();

        layerMask = layermask_to_layer(layer);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SnapToGrid(hand.transform.position);
        SnapToBlockSide();
        HoldBlock();
        PrepareBlockPlacement();
    }

    void SnapToBlockSide()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            mousePos = hit.point;

            Vector3 newPos = mousePos;

            //The following if/else statements are taken and modified from code posted by "Ultimationz" on https://forum.unity.com/threads/detect-the-side-of-physics-raycast-hit.121697/
            //Originally posted May 26, 2015. Code used here on October 13, 2019.
            Vector3 incomingVec = hit.normal - Vector3.up;

            if (incomingVec == new Vector3(0, -1, -1))
            {
                Debug.Log("South of: " + hit.transform.gameObject.name);
                newPos.z -= blockSize;
            }

            else if (incomingVec == new Vector3(0, -1, 1))
            {
                Debug.Log("North of: " + hit.transform.gameObject.name);
                newPos.z += blockSize;
            }

            else if (incomingVec == new Vector3(0, 0, 0))
            {
                Debug.Log("Up of: " + hit.transform.gameObject.name);
                if (hit.transform.gameObject.name.Equals("Ground"))
                {
                    newPos.y = groundSize;
                }
                else
                {
                    newPos.y += blockSize;
                }
            }

            else if (incomingVec == new Vector3(1, 1, 1))
            {
                Debug.Log("Down of: " + hit.transform.gameObject.name);
                if (hit.transform.gameObject.name.Equals("Ground"))
                {
                    newPos.y = -groundSize;
                }
                else
                {
                    newPos.y -= blockSize;
                }
            }                

            else if (incomingVec == new Vector3(-1, -1, 0))
            {
                Debug.Log("West of: " + hit.transform.gameObject.name);
                newPos.x -= blockSize;
            }

            else if (incomingVec == new Vector3(1, -1, 0))
            {
                Debug.Log("East of: " + hit.transform.gameObject.name);
                newPos.x += blockSize;
            }
            
            hand.transform.position = newPos;
            hand.transform.rotation = new Quaternion();

            //Debug.Log(hit.transform.gameObject.name);
        }

    }
    //Makes it more minecrafty. Cant place blocks inside of eachother essentially
    Vector3 SnapToGrid(Vector3 originalPos)
    {
        Vector3 snapPos;

        snapPos.x = Mathf.RoundToInt(originalPos.x / blockSize) * blockSize;
        snapPos.y = originalPos.y;
        snapPos.z = Mathf.RoundToInt(originalPos.z / blockSize) * blockSize;

        //Debug.Log(originalPos);

         Vector2 loc = new Vector2(snapPos.x, snapPos.z);
        
        //If there is/are already blocks in this x,y location, we add it to the top of the stack
        //if (blockLocations.ContainsKey(loc))
        //{
            //basically if there are already 2 blocks in an x,y location, you'll be adding a 3rd block to the top
            //so it'll set y to (3 * size of block) so that it goes to the top
            //This is not a good way to do it but we'll see if i change it

            //snapPos.y = blockSize * blockLocations.Get(loc);
        //}

        return snapPos;
    }

    void PrepareBlockPlacement()
    {
        if (Input.GetMouseButton(0) && Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!draggingBlock)
            {
                Debug.Log("Dragging!");
                draggingBlock = true;
                dragStartLoc = hand.transform.position;
            }
            else
            {
                float distance = Mathf.Abs(Vector3.Distance(dragStartLoc, hand.transform.position)); //Distance between the starting drag location and the current hand location
                
                if (distance >= blockSize)
                {

                }
            }
        }
        else if (Input.GetMouseButtonDown(0) && !draggingBlock && slotSelected > 0 && hand.childCount > 0)
        {
            PlaceBlock(hand.transform.position);
        }
        else if (Input.GetMouseButtonUp(0) && draggingBlock)
        {
            draggingBlock = false;
            dragEndLoc = hand.transform.position;
        }
    }

    void PlaceBlock(Vector3 loc)
    {
        int id = hand.GetComponentInChildren<ElementID>().elementID;

        if (id>0)
        {
            GameObject block = hand.GetComponentInChildren<ElementID>().gameObject; //The actual block
            block.transform.SetParent(environment.transform); //When we place a block, we change the parent from our hand to the environment
            block.GetComponentInChildren<BoxCollider>().gameObject.layer = layerMask; //Set the layer to whatever is listed in the editor. For raycasting purposes.

            //Actually create the object and put it in the world
            GameObject forHand;
            forHand = (GameObject)Instantiate(db.GetElementByID(id).GetWorldPrefab(), hand);
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

    /*
    void PlaceBlock2(int id, Vector2 loc)
    { 
        GameObject block = hand.GetComponentInChildren<ElementID>().gameObject; //The actual block
        block.transform.position = SnapToGrid(new Vector3(loc.x, 0.0f, loc.y));
        block.transform.SetParent(environment.transform); //When we place a block, we change the parent from our hand to the environment

        //Vector2 loc = new Vector2(block.transform.position.x, block.transform.position.z); //Change it to a Vector2 because our map only checks x & y coords

        //If there is already a block in this location
        if (blockLocations.ContainsKey(loc))
        {
            blockLocations.Increment(loc); //Increment the number of blocks at this location
        }
        else //Otherwise, just throw it into the map
        {
            blockLocations.Add(loc, 1);
        }

        //Actually create the object and put it in the world
        GameObject forHand;
        forHand = (GameObject)Instantiate(db.GetElementByID(id).GetWorldPrefab(), hand);
    }

    void PlaceBlock()
    {
        //If the player left clicks
        if (Input.GetMouseButtonDown(0))
        {
            //And they're holding a block
            if (hand.childCount > 0)
            {
                int id = hand.GetComponentInChildren<ElementID>().elementID; //The id of the element we're holding

                //PlaceBlock2(id);
            }
        }
        else if (Input.GetMouseButton(0)) //If theyre dragging the mouse
        {
            //And they're holding a block
            if (hand.childCount > 0 && !draggingBlock)
            {
                draggingBlock = true;
                dragStartLoc = new Vector2(hand.transform.position.x, hand.transform.position.z);
                //Debug.Log("Dragging started at: " + dragStartLoc);
            }
        }
        else if (draggingBlock && Input.GetMouseButtonUp(0))
        {
            draggingBlock = false;
            dragEndLoc = new Vector2(hand.transform.position.x, hand.transform.position.z);
            //Debug.Log("Dragging ended at: " + dragEndLoc);
            List<Vector2> dragBlocksTempLocs = GetBlocksWithinDragRange();
            PlaceDraggedBlocks(dragBlocksTempLocs);
        }
    }

    List<Vector2> GetBlocksWithinDragRange()
    {
        //Do some triangle math. I did this on a whiteboard so you know it's legit
        int a = Mathf.CeilToInt(dragEndLoc.x - dragStartLoc.x);
        int b = Mathf.CeilToInt(dragEndLoc.y - dragStartLoc.y); //The y's here are actually the z location so keep that in mind

        int numBlocksX = Mathf.CeilToInt(blockSize * a);
        int numBlocksY = Mathf.CeilToInt(blockSize * b);

        Debug.Log("Width: " + a + " Height: " + b);
        Debug.Log("BlocksX: " + numBlocksX + " BlocksY: " + numBlocksY);

        List<Vector2> blocks = new List<Vector2>();

        for (int i = 0; i <= numBlocksX; i++)
        {
            for (int j = 0; j <= numBlocksY; j++)
            {
                //Debug.Log("Before: " + new Vector2(dragStartLoc.x + (i * blockSize), dragStartLoc.y + (j * blockSize)));
                blocks.Add(new Vector2(dragStartLoc.x + (i * blockSize), dragStartLoc.y + (j * blockSize)));
                //Debug.Log("After: " + new Vector2(dragStartLoc.x + (i * blockSize), dragStartLoc.y + (j * blockSize)));
            }
        }

        return blocks;
    }

    void PlaceDraggedBlocks(List<Vector2> blocks)
    {
        int id = hand.GetComponentInChildren<ElementID>().elementID;

        foreach (Vector2 block in blocks)
        {
            //Debug.Log("After: " + block);
            PlaceBlock2(id, block);
        }
    }
    */
}
