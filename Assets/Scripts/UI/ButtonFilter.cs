using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonFilter : MonoBehaviour
{
    Button btn;
    PopulateGrid grid;

    void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClickTask);

        grid = GameObject.FindGameObjectWithTag("InventoryView").GetComponent<PopulateGrid>();
    }

    private void Start()
    {
        if (btn.GetComponentInChildren<TextMeshProUGUI>().text.Equals("All"))
        {
            btn.Select();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnClickTask()
    {
        grid.SetGridType(btn.GetComponentInChildren<TextMeshProUGUI>().text);
    }
}
