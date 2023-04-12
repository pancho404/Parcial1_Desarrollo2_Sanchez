using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TimeRewind : MonoBehaviour
{

    private Vector3[] pastPositions = new Vector3[5];
    [SerializeField] private float timeBetweenCapture = 0.2f;
    [SerializeField] private float timeToClean = 4f;
    private float captureTimer = 0.0f;
    private float timer = 0.0f;
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
        Debug.Log(pastPositions[0]);
        if (timer >= timeToClean)
        {
            ClearValues();
            timer = 0.0f;
        }

        captureTimer += Time.deltaTime;
        timer += Time.deltaTime;
        if (captureTimer >= timeBetweenCapture)
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
    }

    void OnRewindTime()
    {

        Vector3 destiny = Vector3.zero;
        for (int i = 0; i < 5; i++)
        {
            if (isValidRewind(i))
            {
                destiny = pastPositions[i];
                break;
            }
        }
        if (destiny==Vector3.zero)
        {
            transform.position = transform.position;
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
