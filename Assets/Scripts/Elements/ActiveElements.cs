using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveElements : MonoBehaviour
{
    [SerializeField]public List<ElementID> elements = new List<ElementID>();

    public void AddElement(ElementID e)
    {
        elements.Add(e);
    }

    public void RemoveElement(ElementID e)
    {
        elements.Remove(e);
    }

    public List<ElementID> GetElements() { return elements; }

    public int GetSize() { return elements.Count; }

}
