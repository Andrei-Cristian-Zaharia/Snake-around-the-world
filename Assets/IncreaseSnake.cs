using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseSnake : Collectable
{
    public int amount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SnakeController SC = (SnakeController)GameObject.FindObjectOfType(typeof(SnakeController));
            SC.size += amount;

            Destroy(this.gameObject);
        }
    }
}