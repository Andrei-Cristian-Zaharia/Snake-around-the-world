using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public GameObject Head;
    public Transform spawnPoint;
    public GameObject lastBody;

    public float generateSpeed = 0.2f;

    public int size = 0;

    public int currentSize = 0;

    public float distance = 0.25f;

    public GameObject BodyPrefab;

    public List<GameObject> parts = new List<GameObject>();

    private void Start()
    {
        StartCoroutine("GenerateBody");
    }

    IEnumerator GenerateBody()
    {
        while (true)
        {
            yield return new WaitForSeconds(generateSpeed);

            currentSize++; 
            StartCoroutine("GenerateTail");

            if (currentSize == size)
            {
                currentSize--;
                lastBody = parts[0];
                parts.Remove(parts[0]);
                Destroy(lastBody);
            }
        }
    }

    IEnumerator GenerateTail()
    {
        yield return new WaitForSeconds(generateSpeed);

        GameObject newBody = Instantiate(BodyPrefab, spawnPoint.transform.position, Quaternion.identity, this.transform) as GameObject;
        parts.Add(newBody);
    }
}
