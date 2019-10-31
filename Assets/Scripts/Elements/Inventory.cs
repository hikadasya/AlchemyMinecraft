using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]List<int> elements = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToInventory(int i)
    {
        elements.Add(i);
    }

    public List<int> GetInventory()
    {
        return elements;
    }
}
