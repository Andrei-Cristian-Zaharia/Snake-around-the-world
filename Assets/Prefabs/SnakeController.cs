using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public GameObject Head;

    public int size = 0;

    public int currentSize = 0;

    public float distance = 0.25f;

    public GameObject BodyPrefab;

    private void Update()
    {
        while (currentSize < size)
            StartCoroutine("GenerateTail");
    }

    IEnumerator GenerateTail()
    {
        yield return new WaitForSeconds(1);

        Vector3 spawnPosition = new Vector3(Head.transform.position.x, Head.transform.position.y, Head.transform.position.z - distance);

        GameObject newBody = Instantiate(BodyPrefab, spawnPosition, Quaternion.identity, this.transform) as GameObject;

        currentSize++;

        Destroy(newBody, 1);
    }
}
