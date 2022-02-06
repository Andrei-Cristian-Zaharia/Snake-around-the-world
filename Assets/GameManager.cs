using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject PointPrefab;


    void Start()
    {
        StartCoroutine(SpawnPoint());
    }

    public void SpawnNewPoint()
    {
        StartCoroutine(SpawnPoint());
    }

    IEnumerator SpawnPoint()
    {
        Vector3 postion = Random.onUnitSphere * 13f;

        Instantiate(PointPrefab, postion, Quaternion.identity);

        yield return null;
    }
}
