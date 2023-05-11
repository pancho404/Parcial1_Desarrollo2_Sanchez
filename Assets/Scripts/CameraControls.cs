using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class CameraControls : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject feet;
    [SerializeField] private float speed = 2f;
    [SerializeField] private AnimationCurve rotationCurve;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private AnimationCurve animation;


    private Vector3[] cameraPositions = new Vector3[4];
    private Vector3 firstPosition = new Vector3(15, 0, 0);
    private Vector3 nextPosition;
    private Vector3 previousPosition;
    private int counter = 1;
    private float percentageComplete;

    // Start is called before the first frame update
    void Start()
    {
        cameraPositions[0] = transform.localPosition;
        //cameraPositions[1] = cameraPositions[0] + firstPosition;
        //cameraPositions[2] = cameraPositions[1] + new Vector3(-firstPosition.x, firstPosition.y, firstPosition.x);
        //cameraPositions[3] = cameraPositions[2] + new Vector3(firstPosition.x, firstPosition.y, -firstPosition.x);
        Debug.Log(feet.transform.localPosition);
        Debug.Log(transform.localPosition);
        Debug.Log(cameraPositions[0]);
        nextPosition = cameraPositions[1];
        previousPosition = cameraPositions[3];
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(feet.transform.position);
    }

    void OnRotateCameraLeft(InputValue input)
    {
        percentageComplete = animation.Evaluate(Time.time);
        //previousPosition = cameraPositions[counter];
        previousPosition = transform.localPosition - firstPosition;
        if (counter > counter - 1)
        {
            counter--;
        }
        if (counter == 3)
        {
            counter = 0;
        }
        StartCoroutine(SlerpCamera(previousPosition));
        StopCoroutine(SlerpCamera(previousPosition));

    }
    void OnRotateCameraRight(InputValue input)
    {
        percentageComplete = animation.Evaluate(Time.time);

        nextPosition = transform.localPosition + firstPosition;
        //nextPosition = cameraPositions[counter];
        Debug.Log(nextPosition);
        if (counter < counter + 1)
        {
            counter++;
        }
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
        while (now - start <= duration)
        {
            now = Time.time;
            transform.position = Vector3.Slerp(transform.position, position, percentageComplete*Time.deltaTime);
            yield return null;
        }
        transform.position = position;
        now = 0.0f;
    }




}

