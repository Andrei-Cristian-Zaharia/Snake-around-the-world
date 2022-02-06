using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IPointerUpHandler, IDragHandler
{
    public Transform player;

    public RectTransform pad;
    Vector3 moveForward;
    Vector3 moveRotate;
    public float moveSpeed;
    public float rotateSpeed;

    public Transform planet;

    void Start()
    {
        StartCoroutine("PlayerMove");
        moveForward = new Vector3(0, 0, 1).normalized;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        transform.localPosition = Vector2.ClampMagnitude(eventData.position - (Vector2)pad.position, pad.rect.width * 0.5f);

        moveRotate = new Vector3(0, transform.localPosition.x, 0).normalized;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
        moveRotate = Vector3.zero;
    }

    IEnumerator PlayerMove()
    {
        while (true)
        {
            player.Translate(moveForward * moveSpeed * Time.deltaTime);

            if (Mathf.Abs(transform.localPosition.x) > pad.rect.width * 0.3f)
                player.Rotate(moveRotate * rotateSpeed * Time.deltaTime);

            player.GetComponent<Rigidbody>().AddForce((player.position - planet.position).normalized * -9.81f);
            player.rotation = Quaternion.Slerp(player.rotation, Quaternion.FromToRotation(player.up,
                (player.position - planet.position).normalized) * player.rotation, rotateSpeed * Time.deltaTime);

            yield return null;
        }
    }
}
