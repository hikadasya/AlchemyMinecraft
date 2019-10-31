using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Block : MonoBehaviour
{
    Vector3 position;
    [SerializeField]ElementID elementId;

    private void Awake()
    {
        elementId = GetComponent<ElementID>();
    }
}
