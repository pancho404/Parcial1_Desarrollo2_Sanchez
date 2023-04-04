using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{

    [SerializeField] private int index;
    [SerializeField] private bool working;

    private void OnCollisionEnter(Collision collision)
    {
        if (working)
        {
            SceneManager.LoadScene(index);
        }
    }
}
