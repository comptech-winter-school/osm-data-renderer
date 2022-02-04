using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using BuildingClass;

public class CameraMovement : MonoBehaviour
{

    public float cameraSpeed = 0.25f * 1.4f;
    private float cameraSensitivity = 4.0f;
    private float xInput;
    private float zInput;
    private bool isSpacePressed = false;
    private bool isShiftPressed = false;
    Vector2 rotation = new Vector2();

    // Start is called before the first frame update
    void Start()
    {
        Request req = new Request(new Point(transform.position.x, transform.position.z), 200);
        req.encode();

        Point[] pnts = new Point[4];
        pnts[0] = new Point(40.0f, 50.0f);
        pnts[1] = new Point(40.0f, 55.0f);
        pnts[2] = new Point(45.0f, 55.0f);
        pnts[3] = new Point(45.0f, 50.0f);

        Response resp;
    }

    // Update is called once per frame
    void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");
        rotation.y += Input.GetAxis("Mouse X");
        rotation.x -= Input.GetAxis("Mouse Y");
        transform.eulerAngles = rotation * cameraSensitivity;

        if (Input.GetKeyDown("space"))
            isSpacePressed = true;
        if (Input.GetKeyUp("space"))
            isSpacePressed = false;
        if (Input.GetKeyDown(KeyCode.LeftShift))
            isShiftPressed = true;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            isShiftPressed = false;
    }

    private void FixedUpdate()
    {
        transform.Translate(new Vector3(0.0f, 0.0f, zInput * cameraSpeed));
        transform.Translate(new Vector3(xInput * cameraSpeed, 0.0f, 0.0f));
        if (isSpacePressed)
            transform.Translate(new Vector3(0.0f, cameraSpeed, 0.0f));
        if (isShiftPressed)
            transform.Translate(new Vector3(0.0f, -cameraSpeed, 0.0f));
    }
}
