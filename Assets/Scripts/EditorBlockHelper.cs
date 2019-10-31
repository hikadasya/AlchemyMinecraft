using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditorBlockHelper : MonoBehaviour
{
    [SerializeField] float gridSize = 0.8f;

    // Update is called once per frame
    void Update()
    {
        Vector3 snapPos;

        snapPos.x = Mathf.RoundToInt(transform.position.x / gridSize) * gridSize;
        snapPos.y = Mathf.RoundToInt(transform.position.y / gridSize) * gridSize;
        snapPos.z = Mathf.RoundToInt(transform.position.z / gridSize) * gridSize;

        transform.SetPositionAndRotation(snapPos, transform.rotation);
    }
}
