using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    private void OnDestroy()
    {
        SnakeController SC = (SnakeController)FindObjectOfType(typeof(SnakeController));

        SC.currentSize--;
    }
}
