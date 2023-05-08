using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class CameraControls : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float speed = 0.2f;
    [SerializeField] private AnimationCurve rotationCurve;
    [SerializeField] private Rigidbody rigidbody;

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
        cameraPositions[1] = new Vector3(10, 2, -0.5f);
        cameraPositions[2] = new Vector3(0, 2, 10);
        cameraPositions[3] = new Vector3(-10, 2, 0);

        nextPosition = cameraPositions[1];
        previousPosition = cameraPositions[3];
    }

    // Update is called once per frame
    void Update()
    {
        cameraPositions[0] = player.transform.forward - new Vector3(0, 2, 10);
        cameraPositions[1] = player.transform.right + new Vector3(10, 2, 0);
        cameraPositions[2] = player.transform.forward + new Vector3(0, 2, 10);
        cameraPositions[3] = player.transform.right - new Vector3(10, 2, 0);
        //rotationValue = rotationCurve.Evaluate(speed * Time.deltaTime);
        transform.LookAt(player.transform.position);
    }

    void OnRotateCameraLeft(InputValue input)
    {
        nextPosition = cameraPositions[counter];
        previousPosition = cameraPositions[counter];
        counter++;
        if (counter >= 3)
        {
            counter = 0;
        }
        StartCoroutine(SlerpCamera(previousPosition));
        if (!input.isPressed)
        {
            //StopAllCoroutines();
        }

        // transform.RotateAround(player.transform.position, player.transform.up, 90.0f);

        counter--;
        rigidbody.MoveRotation(Quaternion.Euler(0, -90, 0));
    }
    void OnRotateCameraRight(InputValue input)
    {
        nextPosition = cameraPositions[counter];
        Debug.Log(nextPosition);
        counter++;
        if (counter >= 3)
        {
            counter = 0;
        }
        StartCoroutine(SlerpCamera(nextPosition));
        StopCoroutine(SlerpCamera(nextPosition));        
    }

    private IEnumerator SlerpCamera(Vector3 position)
    {
        var now = Time.time;
        var start = Time.time;
        while (transform.position != position)
        {
            transform.position = Vector3.Slerp(transform.position, position, ((now - start) / speed) * Time.deltaTime);
            now = Time.time;
            yield return null;
        }
        transform.position = position;

    }

   
     
    
}