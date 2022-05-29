using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject PointPrefab;

    public Text highScoreText;
    public int highScore;
    
    public TextMeshProUGUI goldText;
    public int gold;

    public GameObject player;
    private GameObject currentPlayer;
    public GameObject planet;

    [Range(16f, 100f)]
    public float powerUpsTimer;
    public float selectedTime;
    public static float timerTime;
    public static float currentTimerTime;

    public float baseMoveSpeed;
    public float baseGenerateSpeed;

    public Image displayTimer;
    public GameObject powerUpTimer;

    public GameObject[] powerUpsPrefabs;

    public GameObject playButton;

    public Text scoreText;
    public static int score;

    public GameObject EndGamePanel;

    public Planet planetScript;

    void Start()
    {
        Application.targetFrameRate = 60; // set the target frame rate to 60
        
        Time.timeScale = 1;
        LoadData();
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
        // spawn snake object at the planet set location for the player
        GameObject snake = Instantiate(player, planetScript.playerSpawnLocation.position, Quaternion.identity);
        currentPlayer = snake;
        planet.GetComponent<Planet>().PrepareForGame(); // Destroy replica for now
        
        SnakeController SC = snake.GetComponent<SnakeController>();
        SC.planet = planet.transform;
        
        Camera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camera.transform.parent = SC.Head.transform;
        
        SC.SpawnSnake();
        
        playButton.SetActive(false);
        goldText.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        AddScore(4);

        StartCoroutine("SpawnPoint");
        StartCoroutine("SpawnPowerUp");
    }

    public void RestartGame()
    {
        StaticManager.isRestart = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void AddScore(int currentAmount)
    {
        Text scoreText = GameObject.Find("Score").GetComponent<Text>();
        scoreText.text = "Score: " + (currentAmount - 2);
        score = currentAmount - 2;
    }

    public void ChangeGold(int currentAmount)
    {
        goldText.text = "Gold: " + currentAmount;
        gold += currentAmount;
    }

    public void ChangeGold()
    {
        goldText.text = "Gold: " + gold;
    }

    public void SpawnNewPoint()
    {
        StartCoroutine("SpawnPoint");
    }

    IEnumerator SpawnPoint()
    {
        Vector3 origin = planet.gameObject.transform.position;
        
        Vector3 postion = getSpawnPosition();

        GameObject newPowerUp = Instantiate(PointPrefab, postion, Quaternion.identity) as GameObject;
        newPowerUp.transform.LookAt(origin);
        newPowerUp.transform.rotation = newPowerUp.transform.rotation * Quaternion.Euler(-90, 0, 0);
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

        Vector3 postion = getSpawnPosition();

        GameObject selectedPowerUp = powerUpsPrefabs[Random.Range(0, powerUpsPrefabs.Length)];

        Instantiate(selectedPowerUp, postion, Quaternion.identity);
    }

    Vector3 getSpawnPosition()
    {
        Vector3 postion = Random.onUnitSphere * planetScript.planetRadius;

        while (Physics.OverlapSphere(postion, 0.3f).Length > 0)
        {
            postion = Random.onUnitSphere * planetScript.planetRadius;
        }

        return postion;
    }

    public void changePlanet(GameObject newPlanet)
    {
        planet = newPlanet;
        planetScript = planet.GetComponent<Planet>();
    }

    public void SaveData()
    {
        if (score > highScore)
            PlayerPrefs.SetInt("Highscore", score);

        PlayerPrefs.SetInt("Gold", gold);

        PlayerPrefs.Save();
    }

    void LoadData()
    {
        highScore = PlayerPrefs.GetInt("Highscore");
        highScoreText.text = "Highscore: " + highScore;

        gold = PlayerPrefs.GetInt("Gold");
        ChangeGold();
    }

    public void EndGame()
    {
        StopAllCoroutines();

        gold += score;
        
        SaveData();
        // Time.timeScale = 0;
        SnakeController SC = currentPlayer.GetComponent<SnakeController>();
        SC.move = false;

        if (planet.GetComponent<Planet>().planetScore < score)
        { planet.GetComponent<Planet>().planetScore = score; planet.GetComponent<Planet>().SaveData(); }

        StaticManager.endGame = true;

        EndGamePanel.SetActive(true);
        
        TextMeshProUGUI endGameText = GameObject.Find("EndGameText").GetComponent<TextMeshProUGUI>();

        endGameText.text = StaticManager.causeOfDeath;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    void OnApplicationPause(bool pauseStatus)
    {
        // Debug.Log(pauseStatus);
    }
}
