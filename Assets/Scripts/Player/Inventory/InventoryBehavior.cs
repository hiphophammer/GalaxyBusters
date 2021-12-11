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

    // Public member variables.
    public ItemCatalog itemCatalog;
    public Slot[] slots;

    // Private member variables.
    private PlayerBehavior player;
    private string ship;

    private Item[] items;
    private bool specialItemAdded;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(itemCatalog != null);

        // Initialize our list & other members.
        items = new Item[5];
        specialItemAdded = false;

        foreach (Slot slot in slots)
        {
            Debug.Assert(slot != null);
        }
    }

    public ItemCatalog GetItemCatalog()
    {
        return itemCatalog;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetPlayer(PlayerBehavior player)
    {
        this.player = player;
        ship = player.GetShipName();
        Debug.Log("ShipName: " + ship);
    }

    public bool AddItem(Item item)
    {
        bool success = false;

        // First check if the item can be added.
        bool validItem = IsValidItem(item);

        if (validItem)
        {
            // Add the item to inventory.
            AddToInventory(item);
            itemCatalog.RemoveItem(item);

            // Update the player.
            UpdatePlayer(item);

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

        if (!IsCommonItem(item) && !IsRareItem(item) && !IsEpicItem(item))
        {
            // Check if we have a valid epic item.
            if (IsSpecialItem(item) && !specialItemAdded && item.ship == ship)
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
            if (items[i] == null)
            {
                items[i] = item;
                success = true;
                break;
            }
        }

        Debug.Assert(success == true);  // This MUST always turn out true - otherwise,
                                        // give an error.

        // Update our counters/flags accordingly.
        if (IsSpecialItem(item))
        {
            Debug.Assert(specialItemAdded == false);    // We must never add a special
                                                        // item twice.
            specialItemAdded = true;
        }
    }

    private void UpdatePlayer(Item item)
    {
        if (IsCommonItem(item) || IsRareItem(item))
        {
            // Update speed and weapon damage.
            player.SetSpeed(player.GetSpeed() + item.dSpeed);
            player.SetWeaponDamage(player.GetWeaponDamage() + item.dDamage);

            // Update the health/hitpoints accordingly.
            player.GetHealthBar().IncreaseHitPoints(item.dHP);
            
        }
        else if (IsEpicItem(item))
        {
            if (item.ID == 1)
            {
                // Bullet upgrade.
                player.GetWeapon().SetPenetrate(true);
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
            else if (item.ID == 4)
            {
                // Vampire Bullets: Health gained on hit
                player.GetWeapon().SetVampire(true);
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
        for (int i = 0; i < INVENTORY_SIZE; i++)
        {
            if (items[i] != null)
            {
                slots[i].SetIcon(items[i].icon, 0);
            }
        }
    }
}
