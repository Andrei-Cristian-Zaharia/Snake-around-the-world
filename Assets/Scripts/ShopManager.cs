using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public int gold = 0;

    public int currentTheme = 0;
    public int currentSkin = 0;
    public List<ShopItem> themeItems = new List<ShopItem>();
    public List<SkinItem> skinItems = new List<SkinItem>();

    private void Start()
    {
        LoadData();
        ChangeSkin(currentSkin);
    }

    public void ChangeSkin(int ID)
    {
        skinItems.Find(x => x.ID == ID).Equip();

        SaveData();
    }

    void SaveData()
    {
        PlayerPrefs.SetInt("currentSkin", currentSkin);
        PlayerPrefs.SetInt("currentTheme", currentTheme);
    }

    void LoadData()
    {
        currentTheme = PlayerPrefs.GetInt("currentTheme");
        currentSkin = PlayerPrefs.GetInt("currentSkin");
    }
}
