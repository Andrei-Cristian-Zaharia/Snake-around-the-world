using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;

    public void Update()
    {
        if (!GameManager.isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.A))
                previousLevel();
            
            if (Input.GetKeyDown(KeyCode.D))
                nextLevel();
        }
    }

    public void nextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex != SceneManager.sceneCount)
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void previousLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
