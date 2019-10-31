using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    GameObject elementBeingDragged;
    GameObject gameController;

    ElementID element;
    Transform parent;
    Vector3 startPosition;
    ElementDatabase eleDB;
    ActiveElements activeElements;
    CombinationDatabase comboDB;
    Inventory inventory;
    PopulateGrid grid;
    
    bool draggingFromInventory = false;

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        eleDB = gameController.GetComponent<ElementDatabase>();
        parent = GameObject.FindGameObjectWithTag("CombinationView").transform;
        activeElements = gameController.GetComponent<ActiveElements>();
        comboDB = gameController.GetComponent<CombinationDatabase>();
        inventory = gameController.GetComponent<Inventory>();
        grid = GameObject.FindGameObjectWithTag("InventoryView").GetComponent<PopulateGrid>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForDeletion();
    }

    #region IBeginDragHandler implementation
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (parent != transform.parent)
        {
            if (parent.GetComponentInParent<Canvas>().enabled)
            {
                element = gameObject.GetComponent<ElementID>();
                elementBeingDragged = (GameObject)Instantiate(eleDB.GetElementByID(element.elementID).GetUIPrefab(), parent);
                elementBeingDragged.transform.SetAsLastSibling();
                draggingFromInventory = true;
            }
        }
        else
        {
            elementBeingDragged = gameObject;
            elementBeingDragged.transform.SetAsLastSibling();
            draggingFromInventory = false;
        }

        startPosition = transform.position;
    }
    #endregion

    #region IDragHandler implementation
    public void OnDrag(PointerEventData eventData)
    {
        if (parent.GetComponentInParent<Canvas>().enabled)
        {
            elementBeingDragged.transform.position = Input.mousePosition;
        }
    }
    #endregion

    #region IEndDragHandler implementation
    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggingFromInventory)
        {
            activeElements.AddElement(elementBeingDragged.GetComponent<ElementID>());
        }

        if (elementBeingDragged.transform.localPosition.x < -220.0f && elementBeingDragged.transform.localPosition.y < 575.0f)
        {
            RemoveElement(elementBeingDragged.GetComponent<ElementID>());
        }
        else if (elementBeingDragged.transform.localPosition.y >= 575.0f)
        {
            RemoveElement(elementBeingDragged.GetComponent<ElementID>());
        }
        else
        {
            CheckCombinationOverlap();
        }
        elementBeingDragged = null;
    }
    #endregion

    void CheckCombinationOverlap()
    {
        Rect rect1 = new Rect(elementBeingDragged.GetComponent<RectTransform>().localPosition.x, elementBeingDragged.GetComponent<RectTransform>().localPosition.y, elementBeingDragged.GetComponent<RectTransform>().rect.width, elementBeingDragged.GetComponent<RectTransform>().rect.height);
        
        if (activeElements.GetSize() > 1)
        {
            foreach (ElementID e in activeElements.GetElements().ToArray())
            {
                Rect rect2 = new Rect(e.GetComponent<RectTransform>().localPosition.x, e.GetComponent<RectTransform>().localPosition.y, e.GetComponent<RectTransform>().rect.width, e.GetComponent<RectTransform>().rect.height);
                
                //Make sure it's not overlapping itself
                if (!rect1.Equals(rect2) && rect1.Overlaps(rect2))
                {
                    CheckForCombination(e);
                }
            }
        }

    }
    
    private void CheckForDeletion()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GraphicRaycaster raycaster = transform.parent.GetComponentInParent<GraphicRaycaster>();
            PointerEventData eventData = new PointerEventData(null);
            eventData.position = Input.mousePosition;

            List<RaycastResult> objectsHit = new List<RaycastResult>();

            raycaster.Raycast(eventData, objectsHit);

            if (objectsHit.Count > 0)
            {
                if (objectsHit[0].gameObject.tag.Equals("Element") && objectsHit[0].gameObject.transform.parent.tag.Equals("CombinationView"))
                {
                    RemoveElement(objectsHit[0].gameObject.GetComponent<ElementID>());
                }
            }
        }
    }

    private void RemoveElement(ElementID e)
    {
        activeElements.RemoveElement(e);
        Destroy(e.gameObject);
    }

    private void AddElement(ElementID e)
    {
        activeElements.AddElement(e);
        if (!inventory.GetInventory().Contains(e.elementID))
        {
            inventory.AddToInventory(e.elementID);
            grid.AddLastToGrid();
        }
    }

    private void CheckForCombination(ElementID element2)
    {
        int id1 = elementBeingDragged.GetComponent<ElementID>().elementID;
        int id2 = element2.elementID;
        int[] comboID = comboDB.GetCombinations(id1, id2);
        
        if (comboID.Length > 0)
        {
            foreach (int i in comboID)
            {
                GameObject newObj = Instantiate(eleDB.GetElementByID(i).GetUIPrefab(), parent);
                newObj.transform.position = elementBeingDragged.transform.position;

                ElementID newElement = newObj.GetComponent<ElementID>();

                AddElement(newElement);
            }

            RemoveElement(elementBeingDragged.GetComponent<ElementID>());
            RemoveElement(element2);
        }
    }

}
