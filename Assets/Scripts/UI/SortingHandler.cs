using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SortingHandler : MonoBehaviour
{
    string optionSelected = "Discovery";
    TMP_Dropdown dropdown;
    PopulateGrid grid;

    void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        grid = GameObject.FindGameObjectWithTag("InventoryView").GetComponent<PopulateGrid>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSort();
    }

    public void ChangeSort()
    {
        string newOption = dropdown.options[dropdown.value].text;

        if(!newOption.Equals(optionSelected))
        {
            optionSelected = newOption;
            grid.SetSortType(optionSelected);
        }

    }
}
