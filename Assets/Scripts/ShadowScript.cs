using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScript : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float currentSpeed = 3f;
    [SerializeField] private TimeRewind timeRewindScript;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //transform.forward = player.transform.position - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, timeRewindScript.pastPositions[0], currentSpeed * Time.deltaTime);
    }

    private void OnRewindTIme()
    {
        StartCoroutine(MoveTowardsCourutine());
        currentSpeed = 0;
        StopCoroutine(MoveTowardsCourutine());
    }

    private IEnumerator MoveTowardsCourutine()
    {

        while (transform.position == player.transform.position)
        {
            transform.position = transform.position;
            currentSpeed = speed;

            yield return null;
        }
    }
}
