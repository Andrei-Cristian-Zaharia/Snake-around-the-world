using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggerSpeed : Collectable
{
    public float amount;
    public float duration;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SnakeController SC = (SnakeController)GameObject.FindObjectOfType(typeof(SnakeController));
            SC.EnableChangeSpeedForSeconds(amount, duration);
  
            Destroy(this.gameObject);
        }
    }
}
