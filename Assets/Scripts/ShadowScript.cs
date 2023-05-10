using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScript : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float currentSpeed = 3f;
    [SerializeField] private TimeRewind timeRewindScript;
    [SerializeField] private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, currentSpeed*Time.deltaTime);        
        transform.forward = player.transform.position - transform.position;
    }

    private void OnRewindTIme()
    {
        StartCoroutine(MoveTowardsCourutine());
        currentSpeed = 0;
        StopCoroutine(MoveTowardsCourutine());
        currentSpeed = speed;
    }

    private IEnumerator MoveTowardsCourutine()
    {

        while (transform.position == player.transform.forward)
        {
            currentSpeed = 0;
            
            yield return null;
        }
    }
}
