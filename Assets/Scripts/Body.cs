using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Body : MonoBehaviour
{
    private bool canInteract;

    private void Start()
    {
        StartCoroutine("EnableThis");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canInteract)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    IEnumerator EnableThis()
    {
        yield return new WaitForSeconds(.5f);

        canInteract = true;
    }
}
