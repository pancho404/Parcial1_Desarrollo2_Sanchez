using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class CameraControls : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float speed = 0.2f;
    private Vector3[] cameraPositions = new Vector3[4];
    private Vector3 nextPosition;
    private Vector3 previousPosition;
    private Vector3 cameraRotationDir;
    private int counter = 1;

    // Start is called before the first frame update
    void Start()
    {
        cameraPositions[0] = player.transform.forward - new Vector3(0, 0, 10);
        cameraPositions[1] = player.transform.right + new Vector3(10, 0, 0);
        cameraPositions[2] = player.transform.forward + new Vector3(0, 0, 10);
        cameraPositions[3] = player.transform.right - new Vector3(10, 0, 0);
        nextPosition = cameraPositions[1];
        previousPosition = cameraPositions[3];
        cameraRotationDir=(player.transform.position-transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        cameraPositions[0] = player.transform.forward - new Vector3(0, 3, 10);
        cameraPositions[1] = player.transform.right + new Vector3(10, 3, 0);
        cameraPositions[2] = player.transform.forward + new Vector3(0, 3, 10);
        cameraPositions[3] = player.transform.right - new Vector3(10, 3, 0);
        transform.LookAt(player.transform.position);
    }

    void OnRotateCameraLeft(InputValue input)
    {
        var now = Time.time;
        var start = Time.time;

        counter--;
        if (counter <= 0)
        {
            counter = 3;
        }
        //while (now - start < speed)
        //{
        //    Debug.Log("Rotating");
        //    transform.position = Vector3.Lerp(transform.position, previousPosition, ((now - start) / speed) * Time.deltaTime);
        //    previousPosition = cameraPositions[counter];
        //}
       // transform.position = cameraPositions[counter];
        //transform.RotateAround(player.transform.position,transform.up, 90.0f);
        //transform.RotateAround(player.transform.position,transform.right, 17.0f);
        transform.Rotate(transform.up, 90.0f);
    }
    void OnRotateCameraRight(InputValue input)
    {
        var now = Time.time;
        var start = Time.time;
        counter++;
        if (counter >= 3)
        {
            counter = 0;
        }
        while (now - start < speed)
        {
            Debug.Log("Rotating");
            transform.position = Vector3.Lerp(transform.position, nextPosition, ((now - start) / speed) * Time.deltaTime);
            nextPosition = cameraPositions[counter];
        }
    }
}