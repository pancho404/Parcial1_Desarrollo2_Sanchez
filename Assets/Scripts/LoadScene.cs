using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{

    [SerializeField] private int index;

    private void OnCollisionEnter(Collision collision)
    {
        SceneManager.LoadScene(index);
    }
}
