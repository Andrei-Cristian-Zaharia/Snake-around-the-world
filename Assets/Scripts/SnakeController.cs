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
    public Vector3 moveRotate;

    public Transform planet;

    Vector2 myCentre = new Vector2(Screen.width / 2, Screen.height / 2);
    public float touchPos = 0;

    // change this to affect how quickly the number moves toward its destination
    public float lerpSpeed = 0.1f;

    // set this as big or small as you want. I'm using a factor of the screen's size
    float deadZone = 0.1f * Mathf.Min(Screen.width, Screen.height);

    private void Start()
    {
        StartCoroutine("GenerateBody");
    }

    private void FixedUpdate()
    {
        Vector2 delta = (Vector2)Input.mousePosition - myCentre;

        // if the mouse is down / a touch is active...
        if (Input.GetMouseButton(0) == true)
        {
            // for the X axis...
            if (delta.x > deadZone)
            {
                // if we're to the right of centre and out of the deadzone, move toward 1
                touchPos = Mathf.Lerp(touchPos, 1f, lerpSpeed);
                moveRotate = new Vector3(0, touchPos, 0);
            }
            else if (delta.x < -deadZone)
            {
                // if we're to the left of centre and out of the deadzone, move toward -1
                touchPos = Mathf.Lerp(touchPos, -1f, lerpSpeed);
                moveRotate = new Vector3(0, touchPos, 0);
            }
            else
            {
                // otherwise, we're in the deadzone, move toward 0
                touchPos = 0;
                moveRotate = new Vector3(0, touchPos, 0).normalized;
            }
        }
        else
        {
            touchPos = 0;
            moveRotate = new Vector3(0, touchPos, 0).normalized;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            moveRotate = new Vector3(0, Input.GetAxis("Horizontal"), 0).normalized;

        MoveTowards();
    }

    void MoveTowards()
    {
        Vector3 moveForward = new Vector3(0, 0, 1).normalized;
        Head.transform.Translate(moveForward * moveSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            Head.transform.Rotate(moveRotate * rotateSpeed * (Time.deltaTime * 2));

        if (Input.GetMouseButton(0) == true)
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
