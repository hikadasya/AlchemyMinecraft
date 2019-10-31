using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HideCombinationCanvas : MonoBehaviour
{
    Canvas combinationCanvas;
    Button button;

    // Start is called before the first frame update
    void Awake()
    {
        combinationCanvas = GameObject.FindGameObjectWithTag("CombinationCanvas").GetComponent<Canvas>();
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
    }
    
    void TaskOnClick()
    {
        ToggleCanvas();
    }

    void ToggleCanvas()
    {
        if (combinationCanvas.enabled)
        {
            combinationCanvas.enabled = false;
            button.GetComponentInChildren<TextMeshProUGUI>().text = "<<";
        }
        else
        {
            combinationCanvas.enabled = true;
            button.GetComponentInChildren<TextMeshProUGUI>().text = ">>";
        }
    }
}
