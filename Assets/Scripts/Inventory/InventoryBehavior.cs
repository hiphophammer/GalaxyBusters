using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBehavior : MonoBehaviour
{
    // Constants.
    private const int INVENTORY_SIZE = 5;
    private const int RESERVED_SLOTS = 3;       // 1 for the special item and the common
                                                // and rare items.
    private const int MAX_EPIC_ITEMS = INVENTORY_SIZE - RESERVED_SLOTS;

    // Private member variables.
    private PlayerBehavior player;
    private string ship;

    List<Item>[] items;
    private bool specialItemAdded;
    private int epicItemCount;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize our list & other members.
        items = new List<Item>[5];
        specialItemAdded = false;
        epicItemCount = 0;        
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: remove this later!
        player = Camera.main.GetComponent<GameManager>().GetPlayer1();
    }

    public void SetPlayer(PlayerBehavior player)
    {
        this.player = player;
        ship = player.name;
    }

    public bool AddItem(Item item, bool isPowerUp)
    {
        bool success = false;

        // First check if the item can be added.
        bool validItem = IsValidItem(item);

        if (validItem)
        {
            // Add the item to inventory.
            AddToInventory(item);

            // Update the player.
            UpdatePlayer(item, isPowerUp);

            // Updated the UI.
            UpdateUI();

            success = true;
        }

        return success;
    }

    private bool IsCommonItem(Item item)
    {
        if (item.type == Item.ItemType.common)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsRareItem(Item item)
    {
        if (item.type == Item.ItemType.rare)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsEpicItem(Item item)
    {
        if (item.type == Item.ItemType.epic)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsSpecialItem(Item item)
    {
        if (item.type == Item.ItemType.special)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsValidItem(Item item)
    {
        bool valid = false;

        if (!IsCommonItem(item) && !IsRareItem(item))
        {
            // Check if we have a valid epic item.
            if (IsEpicItem(item) && (epicItemCount < MAX_EPIC_ITEMS))
            {
                valid = true;
            }
            else if (!specialItemAdded || item.ship == ship)
            {
                valid = true;
            }
        }
        else
        {
            valid = true;
        }

        return valid;
    }

    private void AddToInventory(Item item)
    {
        bool success = false;

        for (int i = 0; i < INVENTORY_SIZE; i++)
        {
            List<Item> list = items[i];
            if ((list == null) || (list[0].type == item.type))
            {
                // The list at this location is empty, OR the type of the items in this
                // list matches that of the item we need to add (this only happens with
                // common, rare, and epic items), so we add it here.
                if (list == null)
                {
                    items[i] = new List<Item>();
                    list = items[i];
                }

                list.Add(item);
                success = true;
                break;
            }
        }

        Debug.Assert(success == true);  // This MUST always turn out true - otherwise,
                                        // give an error.

        // Update our counters/flags accordingly.
        if (IsEpicItem(item))
        {
            epicItemCount++;
        }
        else if (IsSpecialItem(item))
        {
            Debug.Assert(specialItemAdded == false);    // We must never add a special
                                                        // item twice.
            specialItemAdded = true;
        }
    }

    private void UpdatePlayer(Item item, bool isPowerUp)
    {
        if (IsCommonItem(item) || IsRareItem(item))
        {
            // Update speed and weapon damage.
            player.SetSpeed(player.GetSpeed() + item.dSpeed);
            player.SetWeaponDamage(player.GetWeaponDamage() + item.dDamage);

            // Update the health/hitpoints accordingly.
            if (isPowerUp)
            {
                player.GetHealthBar().AddHealth(item.dHP);
            }
            else
            {
                player.GetHealthBar().IncreaseHitPoints(item.dHP);
            }
        }
        else if (IsEpicItem(item))
        {
            if (item.ID == 1)
            {
                // Bullet upgrade.
                player.GetWeapon().SetBullet(item.bulletName);
            }
            else if (item.ID == 2)
            {
                // Shoot dual streams of bullets.
                player.GetWeapon().FireDualStream(true);
            }
            else if (item.ID == 3)
            {
                // Shield.
                // TODO: implement this!
            }
        }
        else
        {
            // It's a special item.
            player.EnableSpecialItem();
        }
    }

    private void UpdateUI()
    {
        // Nothing here yet!
    }
}
