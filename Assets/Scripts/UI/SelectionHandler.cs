using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionHandler : MonoBehaviour
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

    [SerializeField]ElementID elementSelected;
    ElementDatabase db;
    GameObject gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        db = gameController.GetComponent<ElementDatabase>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForClick();
        CheckForAssignment();
    }

    private void CheckForClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GraphicRaycaster raycaster = transform.GetComponent<GraphicRaycaster>();
            PointerEventData eventData = new PointerEventData(null);
            eventData.position = Input.mousePosition;
            
            List<RaycastResult> objectsHit = new List<RaycastResult>();
            
            raycaster.Raycast(eventData, objectsHit);
            
            if (objectsHit.Count > 0)
            {
                if (objectsHit[0].gameObject.tag.Equals("Element") && objectsHit[0].gameObject.transform.parent.tag.Equals("InventoryView"))
                {
                    elementSelected = objectsHit[0].gameObject.GetComponent<ElementID>();
                }
            }
        }
    }

    private void CheckForAssignment()
    {
        //Check which slot the user wants to add to
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

        //If they've selected an element and a slot
        if (elementSelected && slotSelected > 0)
        {
            bool creatable = db.GetElementByID(elementSelected.elementID).IsCreatable(); //Checks to see if the element is creatable in the world

            if (creatable)
            {
                string slotName = "Slot " + slotSelected;

                //Set the UI Image
                GameObject slot = GameObject.Find(slotName);
                Image slotImage = slot.GetComponent<Image>();
                slotImage.sprite = db.GetElementByID(elementSelected.elementID).GetUIPrefab().GetComponent<Image>().sprite;

                //Set the element id
                slot.GetComponent<ElementID>().elementID = elementSelected.elementID;

                slotName = "UISlot " + slotSelected;

                //Set the UI Image
                slot = GameObject.Find(slotName);
                slotImage = slot.GetComponent<Image>();
                slotImage.sprite = db.GetElementByID(elementSelected.elementID).GetUIPrefab().GetComponent<Image>().sprite;            }
            else
            {
                Debug.Log("Cannot create this element"); //Notify the user that this element is not creatable in the world
            }
            elementSelected = null;//reset the selected item
        }

        slotSelected = 0;
    }

}
