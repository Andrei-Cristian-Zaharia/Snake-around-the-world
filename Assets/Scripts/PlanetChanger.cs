using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetChanger : MonoBehaviour
{
    private GameManager GM;

    public GameObject[] planets;
    public GameObject currentPlanet;
    public int currentIndex = 0;

    public Button playButton;
    
    public Button nextButton;
    public Button previousButton;

    public GameObject lockedPanel;
    public TextMeshProUGUI lockedText;

    private void Start()
    {
        GM = this.GetComponent<GameManager>();
        
        LoadData();
        AssignNewPlanet();
        EnableControls();

        currentPlanet.SetActive(true);
    }

    public void NextPlanet()
    {
        if (currentIndex < planets.Length - 1)
        {
            DisableControls();
            StartCoroutine(ChangePlanetsAnimNext());
        }
    }

    public void PreviousPlanet()
    {
        if (currentIndex > 0)
        {
            DisableControls();
            StartCoroutine(ChangePlanetsAnimBack());
        }
    }

    IEnumerator ChangePlanetsAnimNext()
    {
        lockedPanel.SetActive(false);
        Animation animOut = planets[currentIndex].GetComponent<Animation>();
        animOut.Play("PlanetOut");

        planets[currentIndex + 1].transform.position = new Vector3(20, 0, 0);

        planets[currentIndex + 1].SetActive(true);
        Animation animIn = planets[currentIndex + 1].GetComponent<Animation>();
        animIn.Play("PlanetIn");

        currentIndex++;

        AssignNewPlanet();

        //Wait until Animation is done Playing
        while (animOut.IsPlaying("PlanetOut"))
        {
            yield return null;
        }

        planets[currentIndex - 1].SetActive(false);

        EnableControls();

        if (planets[currentIndex].GetComponent<Planet>().locked)
        { 
            playButton.interactable = false; 
            lockedPanel.SetActive(true);
            
            if (currentIndex != 0)
                lockedText.text = "You need " + planets[currentIndex].GetComponent<Planet>().reqScore + " points on "
                    + planets[currentIndex - 1].GetComponent<Planet>().name + " to unlock this planet.";
        }
    }

    IEnumerator ChangePlanetsAnimBack()
    {
        lockedPanel.SetActive(false);
        Animation animOut = planets[currentIndex].GetComponent<Animation>();
        animOut.Play("PlanetOut_Point");

        planets[currentIndex - 1].transform.position = new Vector3(-20, 0, 0);

        planets[currentIndex - 1].SetActive(true);
        Animation animIn = planets[currentIndex - 1].GetComponent<Animation>();
        animIn.Play("PlanetIn_Point");

        currentIndex--;

        AssignNewPlanet();

        //Wait until Animation is done Playing
        while (animOut.IsPlaying("PlanetOut_Point"))
        {
            yield return null;
        }

        planets[currentIndex + 1].SetActive(false);

        EnableControls();

        if (planets[currentIndex].GetComponent<Planet>().locked)
        { 
            playButton.interactable = false; 
            lockedPanel.SetActive(true); 

            if (currentIndex != 0)
            lockedText.text = "You need " + planets[currentIndex].GetComponent<Planet>().reqScore + " points on " 
                + planets[currentIndex - 1].GetComponent<Planet>().name + " to unlock this planet.";
        }
    }

    void AssignNewPlanet()
    {
        if (!planets[currentIndex].GetComponent<Planet>().locked)
        {
            GM.planet = planets[currentIndex];
            currentPlanet = planets[currentIndex];
            SaveData();
        }
    }

    void DisableControls()
    {
        playButton.interactable = false;
        nextButton.interactable = false;
        previousButton.interactable = false;
    }

    void EnableControls()
    {
        playButton.interactable = true;
        nextButton.interactable = true;
        previousButton.interactable = true;

        if (currentIndex == 0)
            previousButton.interactable = false;
        else
            previousButton.interactable = true;

        if (currentIndex == planets.Length - 1)
            nextButton.interactable = false;
        else
            nextButton.interactable = true;
    }

    void SaveData()
    {
        PlayerPrefs.SetInt("CurrentPlanet", currentIndex);
    }

    void LoadData()
    {
        currentIndex = PlayerPrefs.GetInt("CurrentPlanet");

        for (int i = 0; i < planets.Length; i++)
        {

            Planet planet = planets[i].GetComponent<Planet>();
            planet.LoadData();
            
            if (i == 0) continue;
            
            Planet previousPlanet = planets[i - 1].GetComponent<Planet>();

            if (previousPlanet.planetScore >= planet.reqScore) 
                planet.locked = false;
            else 
                planet.locked = true;
        }
    }
}