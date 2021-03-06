using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPoint : Collectable
{
    public int amount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager GM = (GameManager)GameObject.FindObjectOfType(typeof(GameManager));
            SnakeController SC = (SnakeController)GameObject.FindObjectOfType(typeof(SnakeController));
            SC.size += amount;
            GameManager.AddScore(amount);

            GM.SpawnNewPowerUp();
            
            hit = true;
            Destroy(this.gameObject);
        }
    }
}
