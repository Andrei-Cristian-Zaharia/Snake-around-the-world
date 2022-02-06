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

    public float moveSpeed;
    public float rotateSpeed;
    Vector3 moveRotate;

    public Transform planet;

    private void Start()
    {
        StartCoroutine("GenerateBody");
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
            moveRotate = new Vector3(0, -1, 0).normalized;

        if (Input.GetKey(KeyCode.D))
            moveRotate = new Vector3(0, 1, 0).normalized;

        foreach (Touch touch in Input.touches)
        {
            if (touch.position.x < Screen.width / 2)
            {
                moveRotate = new Vector3(0, -1, 0).normalized;
            }
            else if (touch.position.x > Screen.width / 2)
            {
                moveRotate = new Vector3(0, 1, 0).normalized;
            }
        }

        MoveTowards();
    }

    void MoveTowards()
    {
        Vector3 moveForward = new Vector3(0, 0, 1).normalized;
        Head.transform.Translate(moveForward * moveSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            Head.transform.Rotate(moveRotate * rotateSpeed * (Time.deltaTime * 2));

        if (Input.touchCount > 0)
            if (Input.GetTouch(0).phase == TouchPhase.Began)
                Head.transform.Rotate(moveRotate * rotateSpeed * (Time.deltaTime * 2));

        Head.transform.GetComponent<Rigidbody>().AddForce((Head.transform.position - planet.position).normalized * -9.81f);
        Head.transform.rotation = Quaternion.Slerp(Head.transform.rotation, Quaternion.FromToRotation(Head.transform.up,
            (Head.transform.position - planet.position).normalized) * Head.transform.rotation, rotateSpeed * Time.deltaTime);
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
