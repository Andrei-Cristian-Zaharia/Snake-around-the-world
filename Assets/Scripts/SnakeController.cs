using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public bool move;
    public bool invertControls;
    
    public GameObject Head;
    public Transform spawnPoint;
    public Transform planet;

    [Space(20f)]
    public int size = 0;
    private int currentSize = 0;
    public float rotateSpeed;

    [Range(0f, 20f)]
    public float moveSpeed;
    
    [HideInInspector]
    public float currentMoveSpeed;
    private float lerpMoveSpeed = 0.8f;
    
    [Space(20f)]
    public GameObject BodyPrefab;
    public ParticleSystem bodyParticle;

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

        if (currentMoveSpeed > moveSpeed + 0.1f || currentMoveSpeed < moveSpeed - 0.1f)
        {
            currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, moveSpeed, Time.deltaTime * lerpMoveSpeed);
        }

        // if the mouse is down / a touch is active...
        if (Input.GetMouseButton(0) == true)
        {
            if (!invertControls)
            {
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
                if (delta.x > deadZone)
                {
                    touchPos = Mathf.Lerp(touchPos, -1f, lerpSpeed);
                    moveRotate = new Vector3(0, touchPos, 0);
                }
                else if (delta.x < -deadZone)
                {
                    touchPos = Mathf.Lerp(touchPos, 1f, lerpSpeed);
                    moveRotate = new Vector3(0, touchPos, 0);
                }
                else
                {
                    touchPos = 0;
                    moveRotate = new Vector3(0, touchPos, 0).normalized;
                }
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
            Head.transform.Translate(moveForward * currentMoveSpeed * Time.deltaTime);

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
        StaticManager.endGame = false;
        move = true;
    }

    public void EnableInvulnerability(float duration)
    {
        StaticManager.invulnerability = true;

        GameManager GM = (GameManager)GameObject.FindObjectOfType(typeof(GameManager));
        GM.CreateDisplayTimer(duration);
        StartCoroutine(DisableInvulnerability(duration));
    }

    public void EnableChangeSpeedForSeconds(float amount, float duration)
    {
        moveSpeed += amount;

        // GameManager.timerTime = amount;
        // GameManager.currentTimerTime = amount;
        GameManager GM = (GameManager)GameObject.FindObjectOfType(typeof(GameManager));
        GM.CreateDisplayTimer(duration);
        StartCoroutine(DisableChangeSpeedForSeconds(duration, amount));
    }

    public void EnableInvert(float duration)
    {
        invertControls = true;

        GameManager GM = (GameManager)GameObject.FindObjectOfType(typeof(GameManager));
        GM.CreateDisplayTimer(duration);
        StartCoroutine(DisableInvert(duration));
    }

    IEnumerator GenerateBody()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / (3 * currentMoveSpeed));

            if (StaticManager.endGame)
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
        yield return new WaitForSeconds(1 / (3 * currentMoveSpeed));

        GameObject newBody = Instantiate(BodyPrefab, spawnPoint.transform.position, Quaternion.identity, this.transform) as GameObject;
        parts.Add(newBody);
    }

    IEnumerator DestroyTail()
    {
        yield return new WaitForSeconds(0.06f);

        currentSize--;
        lastBody = parts[0];

        ParticleSystem particle = Instantiate(bodyParticle, lastBody.transform.position, Quaternion.identity);
        Destroy(particle.gameObject, 0.2f);

        parts.Remove(parts[0]);
        Destroy(lastBody);
    }

    IEnumerator DisableInvulnerability(float duration)
    {
        yield return new WaitForSeconds(duration);

        StaticManager.invulnerability = false;
    }

    IEnumerator DisableChangeSpeedForSeconds(float duration, float amount)
    {
        yield return new WaitForSeconds(duration);

        moveSpeed -= amount;
    }

    IEnumerator DisableInvert(float duration)
    {
        yield return new WaitForSeconds(duration);

        invertControls = false;
    }
}
