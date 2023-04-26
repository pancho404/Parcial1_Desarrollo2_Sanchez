using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeRewind : MonoBehaviour
{
    private Vector3[] pastPositions = new Vector3[5];
    Vector3 destiny = Vector3.zero;

    [Header("Time")]
    [SerializeField] private float timeBetweenCapture = 0.2f;
    [SerializeField] private float timeToClean = 4f;
    [SerializeField] private float rewindDuration = 0.2f;
    private float captureTimer = 0.0f;
    private float cleanTimer = 0.0f;
    private float lerpTimer = 0.0f;
    private float rewindTime = 0.0f;
    private float percentageComplete = 0.0f;
    bool isSet = false;
    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < 5; i++)
        {
            if (pastPositions == null)
            {
                pastPositions.SetValue(Vector3.zero, i);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(pastPositions[0]);
        if (cleanTimer >= timeToClean)
        {
            ClearValues();
            cleanTimer = 0.0f;
        }

        lerpTimer = Time.deltaTime;
        captureTimer += Time.deltaTime;
        cleanTimer += Time.deltaTime;
        if (captureTimer >= timeBetweenCapture * Time.deltaTime)
        {
            for (int i = 0; i < 5; i++)
            {
                if (pastPositions[i] == Vector3.zero)
                {
                    pastPositions[i] = transform.position;
                    isSet = true;
                }
            }
            captureTimer = 0.0f;
        }

        rewindTime += Time.deltaTime;
        percentageComplete = rewindTime / rewindDuration;
    }

    void OnRewindTime(InputValue input)
    {
        bool isCoroutineStarted = false;
        if (input.isPressed)
        {

            for (int i = 0; i < 5; i++)
            {
                if (isValidRewind(i) && !isCoroutineStarted)
                {
                    destiny = pastPositions[i];
                    Debug.Log($"Destiny set to: {destiny}" +
                        $"\nCurrent Position: {transform.position}");
                    break;
                }
            }
            if (destiny == Vector3.zero)
            {
                transform.position = transform.position;
            }

            //  transform.position = destiny;
            StartCoroutine(LerpRewind());
            isCoroutineStarted = true;
        }
        else
        {
            rewindTime = 0.0f;
            StopCoroutine(LerpRewind());
            isCoroutineStarted = false;
        }
      

    }

    private IEnumerator LerpRewind()
    {
        var now = Time.time;
        var start = Time.time;
        //for (var now = Time.time; now - start < rewindDuration; now = Time.time)
        //{
        //    transform.position = Vector3.Lerp(transform.position, destiny, (now - start) / rewindDuration);
        //    yield return null;
        //}
        while (now - start < rewindDuration)
        {
            transform.position = Vector3.Lerp(transform.position, destiny, (now - start) / rewindDuration);
            yield return null;
            now = Time.time;
        }
        transform.position = destiny;

    }

    private void ClearValues()
    {
        for (int i = 0; i < 5; i++)
        {
            pastPositions.SetValue(Vector3.zero, i);
        }
    }

    private bool isValidRewind(int i)
    {
        RaycastHit hit;

        return Physics.Raycast(pastPositions[i], Vector3.down, out hit, 2);
    }
}
