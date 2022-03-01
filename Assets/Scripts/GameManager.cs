using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject PointPrefab;
    public int highScore;
    public GameObject snake;

    [Range(16f, 100f)]
    public float powerUpsTimer;
    public float selectedTime;
    public static float timerTime;
    public static float currentTimerTime;

    public Image displayTimer;
    public GameObject powerUpTimer;

    public GameObject[] powerUpsPrefabs;

    public GameObject playButton;

    private static int score;

    void Start()
    {
        LoadData();
        snake = GameObject.FindGameObjectWithTag("Snake");
    }

    private void Update()
    {
        if (currentTimerTime > 0)
        {
            powerUpTimer.SetActive(true);
            currentTimerTime -= Time.deltaTime;
            displayTimer.fillAmount -= 1.0f / timerTime * Time.deltaTime;
        }
        else { powerUpTimer.SetActive(false); displayTimer.fillAmount = 1; }
    }

    public void Play()
    {
        SnakeController SC = snake.GetComponent<SnakeController>();
        SC.SpawnSnake();
        playButton.SetActive(false);

        StartCoroutine("SpawnPoint");
        StartCoroutine("SpawnPowerUp");
    }

    public static void AddScore(int currentAmount)
    {
        Text scoreText = GameObject.Find("Score").GetComponent<Text>();
        scoreText.text = "Score: " + (currentAmount - 2);
        score = currentAmount - 2;
    }

    public void SpawnNewPoint()
    {
        StartCoroutine("SpawnPoint");
    }

    IEnumerator SpawnPoint()
    {
        Vector3 postion = Random.onUnitSphere * 5.4f;

        Instantiate(PointPrefab, postion, Quaternion.identity);

        yield return null;
    }

    public void SpawnNewPowerUp()
    {
        selectedTime = Random.Range(15f, powerUpsTimer) - 10f;

        StartCoroutine("SpawnPowerUp");
    }

    IEnumerator SpawnPowerUp()
    {
        yield return new WaitForSeconds(selectedTime + 5f);

        Vector3 postion = Random.onUnitSphere * 5.4f;

        GameObject selectedPowerUp = powerUpsPrefabs[Random.Range(0, powerUpsPrefabs.Length)];

        Instantiate(selectedPowerUp, postion, Quaternion.identity);
    }

    public void SaveData()
    {
        if (score > highScore)
            PlayerPrefs.SetInt("Highscore", score);

        PlayerPrefs.Save();
    }

    void LoadData()
    {
        highScore = PlayerPrefs.GetInt("Highscore");
        Text highScoreText = GameObject.Find("Highscore").GetComponent<Text>();
        highScoreText.text = "Highscore: " + highScore;
    }
}
