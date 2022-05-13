using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public int ID;
    public int cost;
    public bool unlocked;

    public virtual void Equip() { }

    public virtual void Unlock() { }
}
