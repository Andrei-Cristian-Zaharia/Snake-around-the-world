using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertControls : Collectable
{
    public float duration;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SnakeController SC = (SnakeController)GameObject.FindObjectOfType(typeof(SnakeController));
            SC.EnableInvert(duration);

            Destroy(this.gameObject);
        }
    }
}