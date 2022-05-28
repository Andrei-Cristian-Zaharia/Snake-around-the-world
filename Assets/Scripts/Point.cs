using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : Collectable
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager GM = (GameManager)GameObject.FindObjectOfType(typeof(GameManager));
            SnakeController SC = (SnakeController)GameObject.FindObjectOfType(typeof(SnakeController));
            SC.size += 2;
            GameManager.AddScore(SC.size);
            GM.SpawnNewPoint();
            
            hit = true;
            Destroy(this.gameObject);
        }
    }
}
