using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] Vector3 blockSize;
    Mesh defaultBlockMesh;

    // Start is called before the first frame update
    void Start()
    {
        defaultBlockMesh = GameObject.FindGameObjectWithTag("DefaultBlock").GetComponentInChildren<MeshFilter>().mesh;
        blockSize = defaultBlockMesh.bounds.size;
    }

    // Update is called once per frame
    void Update()
    {
    }



}
