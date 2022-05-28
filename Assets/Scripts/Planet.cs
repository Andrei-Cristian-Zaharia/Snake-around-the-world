using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public new string name;
    
    public Transform playerSpawnLocation;
    public GameObject snakeReplica;

    public int planetScore = 0;
    public int reqScore = 100;

    public bool locked = false;

    public void PrepareForGame()
    {
        Destroy(snakeReplica);
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("PlanetScore" + "_" + name, planetScore);
    }

    public void LoadData()
    {
        planetScore = PlayerPrefs.GetInt("PlanetScore" + "_" + name);
    }
}
