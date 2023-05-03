using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScript : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private TimeRewind timeRewindScript;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveTowardsCourutine());
    }

    // Update is called once per frame
    void Update()
    {
        //for (int i = 0; i < timeRewindScript.pastPositions.Length; i++)
        //{
        //    transform.position = timeRewindScript.pastPositions[i];
        //}
        Debug.Log(timeRewindScript.pastPositions.Length);
        transform.forward = player.transform.position - transform.position;
    }
    
    private void OnTimeRewind()
    {
        StopCoroutine(MoveTowardsCourutine());
        while (player.transform.position != transform.position)
        {
            transform.position = transform.position;
        }
        StartCoroutine(MoveTowardsCourutine());
    }

    private IEnumerator MoveTowardsCourutine()
    {

        transform.position = Vector3.MoveTowards(transform.position, timeRewindScript.pastPositions[0], speed * Time.deltaTime);

        yield return null;
        StartCoroutine(MoveTowardsCourutine());
    }
}
