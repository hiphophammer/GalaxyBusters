using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemCatalog : MonoBehaviour
{
    // Public member variables.
    public GameManager gameManager;

    // Private member variables.
    List<Item> commonItems;
    List<Item> rareItems;
    List<Item> epicItems;
    Item specialItem;

    bool loadedSpecialItem;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(gameManager != null);

        // Load all of our items.
        commonItems = Resources.LoadAll<Item>("Items/Common Items").ToList();
        rareItems = Resources.LoadAll<Item>("Items/Rare Items").ToList();
        epicItems = Resources.LoadAll<Item>("Items/Epic Items").ToList();

        loadedSpecialItem = false;
    }

    // Update is called once per frame
    void Update()
    {
        LoadSpecialItem();

        if (loadedSpecialItem)
        {
            Debug.Log("Retrieved special item!");
        }
    }

    // Public methods - these are by using by the item selection screen.
    public Item[] GetCommonItem(int amount)
    {
        Item[] items = new Item[amount];

        // If we have <= the specified amount.
        if (commonItems.Count <= amount)
        {
            // Just copy them all over.
            for (int i = 0; i < commonItems.Count; i++)
            {
                items[i] = commonItems[i];
            }
        }
        else
        {
            // Otherwise, pick the items at random.
            for (int i = 0; i < amount; i++)
            {
                Item item = commonItems[Random.Range(0, commonItems.Count)];

                while (items.Contains<Item>(item))
                {
                    item = commonItems[Random.Range(0, commonItems.Count)];
                }

                items[i] = item;
            }
        }

        return items;
    }

    public Item[] GetRareItem(int amount)
    {
        Item[] items = new Item[amount];

        // If we have <= the specified amount.
        if (rareItems.Count <= amount)
        {
            // Just copy them all over.
            for (int i = 0; i < rareItems.Count; i++)
            {
                items[i] = rareItems[i];
            }
        }
        else
        {
            // Otherwise, pick the items at random.
            for (int i = 0; i < amount; i++)
            {
                Item item = rareItems[Random.Range(0, rareItems.Count)];

                while (items.Contains<Item>(item))
                {
                    item = rareItems[Random.Range(0, rareItems.Count)];
                }

                items[i] = item;
            }
        }

        return items;
    }

    public Item[] GetEpicItem(int amount)
    {
        Item[] items = new Item[amount];

        // If we have <= the specified amount.
        if (epicItems.Count <= amount)
        {
            // Just copy them all over.
            for (int i = 0; i < epicItems.Count; i++)
            {
                items[i] = epicItems[i];
            }
        }
        else
        {
            // Otherwise, pick the items at random.
            for (int i = 0; i < amount; i++)
            {
                Item item = epicItems[Random.Range(0, epicItems.Count)];

                while (items.Contains<Item>(item))
                {
                    item = epicItems[Random.Range(0, epicItems.Count)];
                }

                items[i] = item;
            }
        }

        return items;
    }

    public Item GetSpecialItem()
    {
        Item retVal = specialItem;
        specialItem = null;
        return retVal;
    }

    // Public methods - these are for use by the inventory.
    public void RemoveItem(Item target)
    {
        if (target.type == Item.ItemType.common)
        {
            foreach (Item item in commonItems)
            {
                if (item == target)
                {
                    commonItems.Remove(item);
                    break;
                }
            }
        }
        else if (target.type == Item.ItemType.rare)
        {
            foreach (Item item in rareItems)
            {
                if (item == target)
                {
                    rareItems.Remove(item);
                    break;
                }
            }
        }
        else if (target.type == Item.ItemType.epic)
        {
            foreach (Item item in epicItems)
            {
                if (item == target)
                {
                    epicItems.Remove(item);
                    break;
                }
            }
        }
        else
        {
            // It's a special item - but be sure.
            if (specialItem == target)
            {
                specialItem = null;
            }
        }
    }

    public void AddItem(Item item)
    {
        throw new System.NotImplementedException();
    }

    // Private helper methods.
    private void LoadSpecialItem()
    {
        if (!loadedSpecialItem && gameManager.Ready())
        {
            PlayerBehavior player = gameManager.GetPlayer1();
            string shipName = player.GetShipName();
            specialItem = Resources.Load<Item>("Items/Special Items/" + shipName + " Special");
            loadedSpecialItem = true;
        }
    }
}
