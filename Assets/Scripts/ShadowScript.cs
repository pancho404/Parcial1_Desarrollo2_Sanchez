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
    }

    // Update is called once per frame
    void Update()
    {
        //transform.forward = player.transform.position - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, timeRewindScript.pastPositions[0], speed * Time.deltaTime);
    }

    private void OnRewindTIme()
    {
        transform.position = transform.position;
        StartCoroutine(MoveTowardsCourutine());
    }

    private IEnumerator MoveTowardsCourutine()
    {

        while (transform.position == player.transform.position)
        {
            transform.position = transform.position;
            if (transform.position != player.transform.position)
            {
                StopCoroutine(MoveTowardsCourutine());
                break;
            }
            yield return null;
        }
    }
}
