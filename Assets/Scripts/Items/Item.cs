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

    public override bool Equals(object other)
    {
        return base.Equals(other as Item);
    }

    public bool Equals(Item other)
    {
        // Make sure it is not null.
        bool notNull = other != null;

        // Compare the basic info.
        bool sameType = other.type == type;
        bool sameID = other.ID == ID;
        bool sameDescription = other.description == description;
        bool sameIcon = other.icon == icon;
        bool sameInfo = sameType && sameID && sameDescription && sameIcon;

        // Compare the stat changes.
        bool samedHP = other.dHP == dHP;
        bool samedSpeed = other.dSpeed == dSpeed;
        bool samedDamage = other.dDamage == dDamage;
        bool sameStats = samedHP && samedSpeed && samedDamage;

        // Comapre the misc. info for epic and special items.
        bool sameBullet = other.bulletName == bulletName;
        bool sameShip = other.ship == ship;
        bool sameMisc = sameBullet && sameShip;

        // Determine whether they are equal.
        return notNull && sameInfo && sameStats && sameMisc;
    }
}
