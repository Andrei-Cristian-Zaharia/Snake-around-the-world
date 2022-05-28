using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakeController : MonoBehaviour
{
    public bool move;

    public GameObject Head;
    public Transform spawnPoint;
    public Transform planet;

    [Space(20f)]
    public bool invulnerability;
    [Range(1f, 10f)]
    public float invulnerabilityTime = 2f;

    [Space(10f)]
    public bool slow;
    [Range(1f, 10f)]
    public float slowTime = 4f;
    [Range(1f, 10)]
    public float slowPower = 2;

    [Space(20f)]
    public int size = 0;
    public float rotateSpeed;
    public float distance = 0.25f;

    [SerializeField] public float generateSpeed = 0.2f;
    [SerializeField] public float moveSpeed;
    private int currentSize = 0;

    [Space(20f)]
    public GameObject BodyPrefab;

    public List<GameObject> parts = new List<GameObject>();

    private Vector3 moveRotate;
    private GameObject lastBody;

    Vector2 myCentre = new Vector2(Screen.width / 2, Screen.height / 2);
    private float touchPos = 0;

    // change this to affect how quickly the number moves toward its destination
    public float lerpSpeed = 0.1f;

    // set this as big or small as you want. I'm using a factor of the screen's size
    float deadZone = 0.1f * Mathf.Min(Screen.width, Screen.height);

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
        if (move)
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
    }

    public void SpawnSnake()
    {
        StartCoroutine("GenerateBody");
        Body.endGame = false;
        move = true;
    }

    public void EnableInvulnerability()
    {
        invulnerability = true;
        GameManager.timerTime = invulnerabilityTime;
        GameManager.currentTimerTime = invulnerabilityTime;
        StartCoroutine("DisableInvulnerability");
    }

    public void EnableSlow()
    {
        slow = true;
        GameManager.timerTime = slowTime;
        GameManager.currentTimerTime = slowTime;
        StartCoroutine("DisableSlow");
    }

    IEnumerator GenerateBody()
    {
        while (true)
        {
            yield return new WaitForSeconds(generateSpeed);

            if (Body.endGame)
            {
                if (currentSize == 0)
                    break;

                StartCoroutine("DestroyTail");
                continue;
            }

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

    IEnumerator DestroyTail()
    {
        yield return new WaitForSeconds(generateSpeed);

        currentSize--;
        lastBody = parts[0];
        parts.Remove(parts[0]);
        Destroy(lastBody);
    }

    IEnumerator DisableInvulnerability()
    {
        yield return new WaitForSeconds(invulnerabilityTime);

        invulnerability = false;
    }

    IEnumerator DisableSlow()
    {
        yield return new WaitForSeconds(slowTime);

        slow = false;
    }
}
