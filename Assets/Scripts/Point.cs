using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : Collectable
{
    public int amount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SnakeController SC = (SnakeController)GameObject.FindObjectOfType(typeof(SnakeController));

            GameManager GM = (GameManager)GameObject.FindObjectOfType(typeof(GameManager));
            SC.size += amount;
            GameManager.AddScore(amount);
            GM.SpawnNewPoint();

            hit = true;
            Destroy(this.gameObject);
        }
    }
}
