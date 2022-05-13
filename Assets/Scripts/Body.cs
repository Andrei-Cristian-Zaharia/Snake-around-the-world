using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            SnakeController SC = (SnakeController)GameObject.FindObjectOfType(typeof(SnakeController));

            if (SC.invulnerability == true)
                return;

            GameManager GM = (GameManager)GameObject.FindObjectOfType(typeof(GameManager));
            GM.EndGame();
        }
    }

    IEnumerator EnableThis()
    {
        yield return new WaitForSeconds(.5f);

        canInteract = true;
    }
}
