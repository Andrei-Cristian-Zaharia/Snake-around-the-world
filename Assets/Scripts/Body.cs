using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    private bool canInteract;
    public ParticleSystem bodyParticle;
    public static bool endGame = false;

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
            GameManager.causeOfDeath = "You died trying to eat yourself :(";
            GM.EndGame();
        }
    }

    IEnumerator EnableThis()
    {
        yield return new WaitForSeconds(.5f);

        canInteract = true;
    }

    private void OnDestroy()
    {
        if (endGame)
        {
            ParticleSystem particle = Instantiate(bodyParticle, transform.position, Quaternion.identity);
            Destroy(particle.gameObject, 0.2f);
        }
    }
}
