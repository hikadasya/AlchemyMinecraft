using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Element
{
    [SerializeField] GameObject uiPrefab;
    [SerializeField] GameObject worldPrefab;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] int id;
    [SerializeField] string name;
    [SerializeField] bool combinable;
    [SerializeField] bool creatable;

    public int GetID() { return id; }
    public string GetName() { return name; }
    public GameObject GetUIPrefab() { return uiPrefab;}
    public GameObject GetWorldPrefab() { return worldPrefab; }
    public TextMeshProUGUI GetTMP() { text.text = name;  return text; }
    public bool IsCreatable() { return creatable; }
    public bool IsCombinable() { return combinable; }
}
