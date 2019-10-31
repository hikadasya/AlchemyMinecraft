using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonPlayerController : MonoBehaviour
{
    [SerializeField] float playerMoveSpeed = 0.5f;
    [SerializeField] float playerRotationSpeed = -0.5f;
    [SerializeField] float maxRotation;
    [SerializeField] float minRotation;

    Camera cam;

    GameObject gameController;
    ElementDatabase db;
    GameObject environment;

    [SerializeField] public Dictionary<Vector2, int> blockLocations = new Dictionary<Vector2, int>(); //Don't need a dictionary but it's faster accessing than lists.

    Vector3 mousePos;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        db = gameController.GetComponent<ElementDatabase>();
        cam = GameObject.FindGameObjectWithTag("FPSCam").GetComponent<Camera>();
        environment = GameObject.FindGameObjectWithTag("Environment");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        PlayerRotate();
    }

    void PlayerMove()
    {
        float hor = Input.GetAxis("Horizontal"); //left & right
        float ver = Input.GetAxis("Vertical"); //forward and back
        Vector3 camMovement = new Vector3(hor, 0.0f, ver) * playerMoveSpeed * Time.deltaTime; //Accurately move based on whatever is considered forward/right atm
        transform.Translate(camMovement, Space.Self);//translate from world(?) space to local space
    }

    void PlayerRotate()
    {
        //-----------------Figure out clamping because it gets wonky if you look up too much--------------------//

        float mouseX = -Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float rotationX = mouseY * playerRotationSpeed;
        float rotationY = mouseX * playerRotationSpeed;
                
        Vector3 ahhh = new Vector3(rotationX, rotationY, 0.0f);
        transform.Rotate(ahhh);

        float x = transform.rotation.eulerAngles.x;
        float y = transform.rotation.eulerAngles.y;

        transform.rotation = Quaternion.Euler(x, y, 0.0f);        
        
    }
}
