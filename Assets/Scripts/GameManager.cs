using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Well, only the script that handles the game. =)))))
/// I hope I won't forget what the fuck I did here. If I do that ... well, it was nice to work at it.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region public Fields

    public TextMeshProUGUI planetScore; // text that displays the score for each planet
    public TextMeshProUGUI goldText; // text that displays the gold => this should have been only in shop manager or game menu, move this ;) thx
    public int gold;

    public GameObject player; // current player
    public GameObject planet; // current planet

    [Range(8f, 100f)]
    public float powerUpsTimer; // time that it takes to spawn a new power up
    [Range(8f, 100f)]
    public float debuffTimer; // time that it takes to spawn a new debuff

    public GameObject displayTimerPrefab; // the timer that will be spawned for an ability / debuff
    public GameObject displayTimerParent; // the parent that holds the timers for each active ability / debuff

    public GameObject PointPrefab; // point prefab as it's name says =)))))
    public GameObject[] powerUpsPrefabs; // prefabs for power ups
    public GameObject[] debuffsPrefabs; // prefabs for debuffs

    public GameObject playButton; // move this to game menu ;) thx

    public Text scoreText;
    public static int score = -2;

    public GameObject EndGamePanel;

    #endregion

    #region private Fields

    private Planet planetScript;
    private GameObject currentPlayer;

    #endregion

    #region Methods
    
    void Start()
    {
        Application.targetFrameRate = 60; // set the target frame rate to 60
        
        Time.timeScale = 1;
        LoadData();
    }

    public void Play()
    {
        // spawn snake object at the planet set location for the player
        GameObject snake = Instantiate(player, planetScript.playerSpawnLocation.position, Quaternion.identity);
        currentPlayer = snake;
        planet.GetComponent<Planet>().PrepareForGame(); // Destroy replica for now


        SnakeController SC = snake.GetComponent<SnakeController>();
        SC.planet = planet.transform;

        SC.currentMoveSpeed = SC.moveSpeed;
        Camera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camera.transform.parent = SC.Head.transform;
        
        SC.SpawnSnake();
        
        playButton.SetActive(false);
        goldText.gameObject.SetActive(false);
        planetScore.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        AddScore(SC.size);

        SpawnNewPoint();
        SpawnNewPowerUp();
        SpawnNewDebuff();
    }

    public void RestartGame()
    {
        StaticManager.isRestart = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void AddScore(int currentAmount)
    {
        score += currentAmount;
        Text scoreText = GameObject.Find("Score").GetComponent<Text>();
        scoreText.text = "Score: " + score;
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
        float selectedTime = Random.Range(15f, powerUpsTimer) - 10f;
        
        StartCoroutine(SpawnPowerUp(selectedTime));
    }

    IEnumerator SpawnPowerUp(float time)
    {
        yield return new WaitForSeconds(time + 5f);

        Vector3 postion = getSpawnPosition();

        GameObject selectedPowerUp = powerUpsPrefabs[Random.Range(0, powerUpsPrefabs.Length)];

        Instantiate(selectedPowerUp, postion, Quaternion.identity);
    }

    public void SpawnNewDebuff()
    {
        float selectedTime = Random.Range(15f, debuffTimer) - 10f;

        StartCoroutine(SpawnDebuff(selectedTime));
    }

    IEnumerator SpawnDebuff(float time)
    {
        yield return new WaitForSeconds(time + 8f);

        Vector3 postion = getSpawnPosition();

        GameObject selectedDebuff = debuffsPrefabs[Random.Range(0, debuffsPrefabs.Length)];

        GameObject obj = Instantiate(selectedDebuff, postion, Quaternion.identity);

        Destroy(obj, 5f);
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

        planetScore.text = "Score on " + planetScript.name + ": \n" + planetScript.planetScore;
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("Gold", gold);

        PlayerPrefs.Save();
    }

    void LoadData()
    {
        gold = PlayerPrefs.GetInt("Gold");
        ChangeGold();
    }

    public void EndGame()
    {
        StopAllCoroutines();

        gold += score;
        
        SaveData();
        
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

    public void CreateDisplayTimer(float time) 
    {
        GameObject timer = Instantiate(displayTimerPrefab, displayTimerParent.transform);
        DisplayTimer DT = timer.GetComponent<DisplayTimer>();
        timer.transform.parent = displayTimerParent.transform;
        
        DT.Init(time);
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

    #endregion
}
