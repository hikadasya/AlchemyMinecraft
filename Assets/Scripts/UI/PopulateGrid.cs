using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopulateGrid : MonoBehaviour
{
    GameObject gameController;
    Inventory inventory;
    ElementDatabase db;

    [SerializeField]string currentGridType = "All";
    [SerializeField] string sorting = "Discovery";

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        inventory = gameController.GetComponent<Inventory>();
        db = gameController.GetComponent<ElementDatabase>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Populate();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.C)) //For Debugging purposes
        //{
        //    DestroyGrid();
        //}
    }

    //Destroys and repopulates the grid based on the grid type
    void Populate()
    {
        DestroyGrid();

        List<string> elements = new List<string>();

        foreach (int i in inventory.GetInventory())
        {
            switch(sorting)
            {
                case "Discovery": AddToGrid(i); //If it's sorted by the order you discovered them, just throw them on the grid by their place in the inventory 
                    break;
                default: elements.Add(db.GetElementByID(i).GetName()); //Otherwise, add the element names to a list so we can sort and add them later
                    break;
            }
        }

        //If you're not sorting by the order you discovered
        if (!sorting.Equals("Discovery"))
        {
            elements.Sort(); //Sort them alphabetically

            if(sorting.Equals("Z-A")) //If you sorted them descending, reverse the sort order
            {
                elements.Reverse();
            }

            //Now that they're sorted, just add all the elements to the grid
            foreach(string e in elements)
            {
                AddToGrid(db.GetElementByName(e).GetID());
            }
        }
    }

    //Adds a single element to the grid
    private void AddToGrid(int id)
    {
        GameObject elementPicture; //The image that will be shown on the grid
        TextMeshProUGUI elementName; //The text that will be shown on the grid

        switch (currentGridType)
        {
            case "All": //If you filtered by all elements, just throw it in there
                elementPicture = (GameObject)Instantiate(db.GetElementByID(id).GetUIPrefab(), transform); //Get the UI prefab and instantiate it with the scrollview content as its parent
                elementPicture.GetComponent<ElementID>().elementID = id; //Set the id to the element's id so that it works accurately when combining and placing it into an equip slot

                elementName = (TextMeshProUGUI)Instantiate(db.GetElementByID(id).GetTMP(), elementPicture.transform); //Get the element name prefab and instantiate it with the picture as its parent
                break;
            case "Combinable": 
                if (db.GetElementByID(id).IsCombinable()) //If you filtered by combinable elements, only throw the element in there if it is a combinable element
                {
                    elementPicture = (GameObject)Instantiate(db.GetElementByID(id).GetUIPrefab(), transform);
                    elementPicture.GetComponent<ElementID>().elementID = id;

                    elementName = (TextMeshProUGUI)Instantiate(db.GetElementByID(id).GetTMP(), elementPicture.transform);
                }
                break;
            case "Creatable": 
                if (db.GetElementByID(id).IsCreatable()) //If you filtered by creatable elements, only throw the element in there if it is a creatable element
                {
                    elementPicture = (GameObject)Instantiate(db.GetElementByID(id).GetUIPrefab(), transform); 
                    elementPicture.GetComponent<ElementID>().elementID = id;

                    elementName = (TextMeshProUGUI)Instantiate(db.GetElementByID(id).GetTMP(), elementPicture.transform);
                }
                break;
        }
    }

    //Finds the last element added to the inventory and adds it to the grid
    public void AddLastToGrid()
    {
        int lastIndex = inventory.GetInventory().Count -1; //Get the last index in the inventory
        int id = inventory.GetInventory()[lastIndex]; //Get its ID

        AddToGrid(id); //Add it to the grid
    }

    //Removes all elements from the grid
    //Usually only used when sorting the grid
    public void DestroyGrid()
    {
        if (transform.childCount > 0) //Dont waste your time if there arent any elements in the grid
        {
            foreach (RectTransform t in GetComponentInChildren<RectTransform>()) //Was having weird issues when using just the transform. For some reason, the RectTransform works better for UI objects I guess?
            {
                Destroy(t.gameObject);//boom
            }
        }
    }

    //Updates the type of grid you selected (All, Combinable, Creatable, etc) and repopulates the grid
    public void SetGridType(string type)
    {
        currentGridType = type;
        Populate(); 
    }

    //Updates the grid base on the sorting you selected (Discovery, A-Z, Z-A) and repopulates the grid
    public void SetSortType(string type)
    {
        sorting = type;
        Populate();
    }
}
