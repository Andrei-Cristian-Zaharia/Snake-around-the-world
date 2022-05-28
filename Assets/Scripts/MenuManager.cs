using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject shopPanel;

    public GameObject skinPanel;
    public GameObject themesPanel;

    public TextMeshProUGUI skinText;

    public void Back()
    {
        shopPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        mainMenuPanel.SetActive(false);

        skinPanel.SetActive(true);
        themesPanel.SetActive(false);
    }

    public void OpenSkin()
    {
        skinPanel.SetActive(true);
        themesPanel.SetActive(false);
    }

    public void OpenThemes()
    {
        skinPanel.SetActive(false);
        themesPanel.SetActive(true);
    }
}
