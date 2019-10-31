using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialChunk : MonoBehaviour
{
    [SerializeField] Vector3 initalStartPosition;
    [SerializeField] float blockSize;
    [SerializeField] int numRows;
    [SerializeField] int numColumns;
    [SerializeField] string blockName;

    GameObject gameController;
    ElementDatabase db;

    GameObject baseGround;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        db = gameController.GetComponent<ElementDatabase>();

        baseGround = GameObject.FindGameObjectWithTag("Environment");
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int r = 0; r < numRows; r++)
        {
            for(int c = 0; c < numColumns; c++)
            {
                GameObject lol;
                lol = (GameObject)Instantiate(db.GetElementByName(blockName).GetWorldPrefab());
                lol.transform.position = new Vector3(initalStartPosition.x + (c*blockSize), 0.1f, initalStartPosition.z + (r * blockSize));
                lol.transform.rotation = new Quaternion();
                lol.transform.parent = baseGround.transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
