using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class CameraControls : MonoBehaviour
{
    [SerializeField] private GameObject feet;
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
        cameraPositions[0] = feet.transform.localPosition;
        cameraPositions[1] = cameraPositions[0] + new Vector3(15, 0, 0);
        cameraPositions[2] = cameraPositions[1] + new Vector3(-15, 0, 15);
        cameraPositions[3] = cameraPositions[2] + new Vector3(15, 0, -15);
        Debug.Log(feet.transform.localPosition);
        Debug.Log(transform.localPosition);
        Debug.Log(cameraPositions[0]);
        nextPosition = cameraPositions[1];
        previousPosition = cameraPositions[3];
    }

    // Update is called once per frame
    void Update()
    {
        //rotationValue = rotationCurve.Evaluate(speed * Time.deltaTime);
        transform.LookAt(feet.transform.position);
    }

    void OnRotateCameraLeft(InputValue input)
    {
        previousPosition = cameraPositions[counter];
        counter--;
        if (counter == 3)
        {
            counter = 0;
        }
        StartCoroutine(SlerpCamera(previousPosition));
        StopCoroutine(SlerpCamera(previousPosition));


        // transform.RotateAround(player.transform.position, player.transform.up, 90.0f);

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
        Vector3 distance = position - transform.position;
        float duration = distance.magnitude / speed;
        while (now - start < duration)
        {
            now = Time.time;
            transform.position = Vector3.Slerp(transform.position, position, (now - start) / duration);
            yield return null;
        }
        transform.position = position;

    }




}