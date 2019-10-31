using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodCameraController : MonoBehaviour
{
    //Camera controls
    [Header("Camera Controls")]
    [SerializeField] float camMoveSpeed = 0.2f;
    [SerializeField] float camZoomSpeed = 1.0f;
    [SerializeField] float camMaxZoom = 50.0f;
    [SerializeField] float camMinZoom = 90.0f;
    [SerializeField] float camRotationSpeed = -0.5f;

    Camera cam;
    Transform camTrans;
    GameObject elementUI;


    [SerializeField] bool godHandActive = true;

    GameObject godHandCam; //Location of where the camera should go in god mode
    Transform godHandCamTransform;
    Vector3 godHandRotationValues;

    // Start is called before the first frame update
    void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("GodCam").GetComponent<Camera>();
        camTrans = cam.transform;
        godHandCam = GameObject.Find("God Cam");

        //Setup the transform for the godHand camera
        godHandCamTransform = godHandCam.transform; //initial setup

        elementUI = GameObject.FindGameObjectWithTag("ElementUI");
    }

    // Update is called once per frame
    void Update()
    {
        ControlCamera();
    }

    void ControlCamera()
    {
        CameraMove();
        CameraRotate();
    }

    void CameraMove()
    {
        float hor = Input.GetAxis("Horizontal"); //left & right
        float ver = Input.GetAxis("Vertical"); //forward and back
        Vector3 camMovement = new Vector3(hor, 0.0f, ver) * camMoveSpeed * Time.deltaTime; //Accurately move based on whatever is considered forward/right atm
        transform.parent.transform.Translate(camMovement, Space.Self);//translate from world(?) space to local space
    }

    //---------Probs change this from FOV to some other shit. Not currently done the best way, but the easiest way-----------------//
    void CameraZoom()
    {
        //Scroll Up and Down
        if (Input.mouseScrollDelta.y > 0)
        {
            cam.fieldOfView -= camZoomSpeed;
            if (cam.fieldOfView < camMaxZoom)
            {
                cam.fieldOfView = camMaxZoom;
            }
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            cam.fieldOfView += camZoomSpeed;
            if (cam.fieldOfView > camMinZoom)
            {
                cam.fieldOfView = camMinZoom;
            }
        }
    }

    void CameraRotate()
    {
        if (Input.GetMouseButton(1))
        {
            //Get rotation based off of the mouse axes. Multiplying by the cam rotation speed applies the turn based off how much you move the mouse around
            transform.parent.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * camRotationSpeed, -Input.GetAxis("Mouse X") * camRotationSpeed, 0.0f));

            //Do that quaternion shit dog
            float x = transform.parent.transform.rotation.eulerAngles.x;
            float y = transform.parent.transform.rotation.eulerAngles.y;

            //Put it all together
            transform.parent.transform.rotation = Quaternion.Euler(x, y, 0);
        }
    }
}
