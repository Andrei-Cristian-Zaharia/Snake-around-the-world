using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager GM = (GameManager)GameObject.FindObjectOfType(typeof(GameManager));
            SnakeController SC = (SnakeController)GameObject.FindObjectOfType(typeof(SnakeController));
            SC.size += 10;
            GameManager.AddScore(SC.size);

            GM.SpawnNewPowerUp();
            Destroy(this.gameObject);
        }
    }
}