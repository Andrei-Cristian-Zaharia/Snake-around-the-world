using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : Collectable
{
    public float amount;
    public float duration;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager GM = (GameManager)GameObject.FindObjectOfType(typeof(GameManager));
            SnakeController SC = (SnakeController)GameObject.FindObjectOfType(typeof(SnakeController));
            SC.EnableChangeSpeedForSeconds(-amount, duration);
            GM.SpawnNewPowerUp();

            hit = true;
            Destroy(this.gameObject);
        }
    }
}
