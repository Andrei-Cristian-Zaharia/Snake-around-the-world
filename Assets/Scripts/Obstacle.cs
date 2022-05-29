using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager GM = (GameManager)GameObject.FindObjectOfType(typeof(GameManager));
            StaticManager.causeOfDeath = "You stuck your head in a fucking cube";
            GM.EndGame();
        }
    }
}