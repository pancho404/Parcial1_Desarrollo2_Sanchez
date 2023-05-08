using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class CameraControls : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float speed = 0.2f;
    [SerializeField] private AnimationCurve rotationCurve;

    private Vector3[] cameraPositions = new Vector3[4];
    private Vector3 nextPosition;
    private Vector3 previousPosition;
    private float rotationValue;
    private int counter = 1;
    float degrees = 90;

    // Start is called before the first frame update
    void Start()
    {
        cameraPositions[0] = new Vector3(0, 2, -10);
        cameraPositions[1] =new Vector3(10, 2, 0);
        cameraPositions[2] =new Vector3(0, 2, 10);
        cameraPositions[3] = new Vector3(-10, 2, 0);
        Debug.Log(cameraPositions[0]);
        Debug.Log(cameraPositions[1]);
        Debug.Log(cameraPositions[2]);
        Debug.Log(cameraPositions[3]);
        nextPosition = cameraPositions[1];
        previousPosition = cameraPositions[3];
    }

    // Update is called once per frame
    void Update()
    {
        //cameraPositions[0] = player.transform.forward - new Vector3(0, 2, 10);
        //cameraPositions[1] = player.transform.right + new Vector3(10, 2, 0);
        //cameraPositions[2] = player.transform.forward + new Vector3(0, 2, 10);
        //cameraPositions[3] = player.transform.right - new Vector3(10, 2, 0);
        rotationValue = rotationCurve.Evaluate(speed * Time.deltaTime);
        transform.LookAt(player.transform.position);
        player.transform.LookAt(transform);
    }

    //void OnRotateCameraLeft(InputValue input)
    //{
    //    var now = Time.time;
    //    var start = Time.time;
    //    if (counter <= 0)
    //    {
    //        counter = 3;
    //    }
    //    //while (transform.position != previousPosition)
    //    //{
    //    //    Debug.Log("Rotating");
    //    //    transform.position = Vector3.Lerp(transform.position, previousPosition, speed * Time.deltaTime);
    //    //    previousPosition = cameraPositions[counter];
    //    //}
    //    // transform.position = cameraPositions[counter];

    //    transform.RotateAround(player.transform.position, player.transform.up, 90.0f);
    //    player.transform.Rotate(new Vector3(0, 90.0f, 0));
    //    player.transform.rotation = new Quaternion(0,-degrees,0,1);
    //    previousPosition = cameraPositions[counter];
    //    counter--;
    //    degrees += 90;
    //}
    //void OnRotateCameraRight(InputValue input)
    //{
    //    var degrees = 90;
    //    //nextPosition = cameraPositions[counter];
    //    //counter++;
    //    //if (counter >= 3)
    //    //{
    //    //    counter = 0;
    //    //}
    //    //StartCoroutine(SlerpCamera(nextPosition));
    //    //if (!input.isPressed)
    //    //{
    //    //    //StopAllCoroutines();
    //    //}
    //    ////transform.Rotate(player.transform.up, 90.0f);
    //    player.transform.rotation.SetFromToRotation(transform.position, new Vector3(0,degrees,0));
    //    degrees += 90;
    //}

    private IEnumerator SlerpCamera(Vector3 position)
    {
        var now = Time.time;
        var start = Time.time;
        while (now - start < speed)
        {
            Debug.Log("Rotating");
            // transform.position = Vector3.Lerp(transform.position, nextPosition, ((now - start) / speed) * Time.deltaTime);
            transform.position = Vector3.Slerp(transform.position, position, ((now - start) / speed) * Time.deltaTime);
            now = Time.time;
            yield return null;
        }
        //transform.position = nextPosition;

    }
}