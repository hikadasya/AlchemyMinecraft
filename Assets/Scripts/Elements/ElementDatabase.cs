using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ElementDatabase : MonoBehaviour
{
    [SerializeField] List<Element> elements = new List<Element>();

    public Element GetElementByID(int ID)
    {
        foreach(Element e in elements)
        {
            if (e.GetID() == ID)
            {
                return e;
            }
        }

        return null;
    }

    public Element GetElementByName(string name)
    {
        foreach (Element e in elements)
        {
            if (e.GetName() == name)
            {
                return e;
            }
        }

        return null;
    }

}
