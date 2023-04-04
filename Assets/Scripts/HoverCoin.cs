using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverCoin : MonoBehaviour
{

    [SerializeField] private Vector3 originPos = new Vector3(0, 1, 0);
    [SerializeField] private Vector3 tempPos = new Vector3(0, 1, 0);
    [SerializeField] private float amplitude = 0.2f;
    [SerializeField] private float frequency = 1f;


    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        tempPos = originPos;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        transform.position = tempPos;

    }
}
