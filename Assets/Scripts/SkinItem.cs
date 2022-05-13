using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinItem : ShopItem
{
    public Color skinColor;

    private void Awake()
    {
        this.GetComponent<Image>().color = skinColor;
    }

    public override void Equip()
    {
        Material bodyMaterial = Resources.Load<Material>("Materials/SnakeBody");
        bodyMaterial.color = skinColor;
    }

    public override void Unlock()
    {
        base.Unlock();
    }
}
