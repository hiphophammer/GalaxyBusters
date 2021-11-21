using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item: ScriptableObject
{
    public enum ItemType
    {
        common,
        rare,
        epic,
        special
    };

    public bool isPowerUp;

    // These are used for displaying the item.
    public int ID;
    public ItemType type;
    public string description;
    public Sprite icon;

    // Fields for stat changes.
    public float dHP;
    public float dSpeed;
    public float dDamage;

    // Fields for epic items.
    public string bulletName;

    // Fields for special items.
    public string ship;
}
