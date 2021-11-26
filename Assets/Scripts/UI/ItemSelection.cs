using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelection : MonoBehaviour
{
    // Constants.
    private const float WAITING_TIME = 10.0f;

    // Public member variables.
    public ItemSelectionButton button1;
    public ItemSelectionButton button2;
    public ItemSelectionButton button3;

    public TMPro.TextMeshProUGUI header;
    public TMPro.TextMeshProUGUI timerText;

    // Private member variables.
    private InventoryBehavior player1Inventory;
    private InventoryBehavior player2Inventory;

    private ItemCatalog player1ItemCatalog;

    private PlayerBehavior player1;
    private PlayerBehavior player2;

    private bool singlePlayer;

    private bool donePresenting;
    private int itemChosen;
    private int levelNum;

    private Item item1;
    private Item item2;
    private Item item3;

    private bool retrievedReferences;

    // FSM stuff.
    private enum ItemSelectionState
    {
        setPlayer1Items,
        waitOnPlayer1,
        setPlayer2Items,
        waitOnPlayer2,
        hide
    };

    private ItemSelectionState state;
    private float stateEntryTime;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(button1 != null);
        Debug.Assert(button2 != null);
        Debug.Assert(button3 != null);

        Debug.Assert(header != null);
        Debug.Assert(timerText != null);

        donePresenting = false;
        state = ItemSelectionState.hide;
    }

    // Update is called once per frame
    void Update()
    {
        RetrieveReferences();

        if (retrievedReferences && !donePresenting)
        {
            UpdateFSM();
        }
    }

    // Public methods.
    public void PresentItems(int levelNum)
    {
        this.levelNum = levelNum;
        donePresenting = false;
        state = ItemSelectionState.setPlayer1Items;
        gameObject.SetActive(true);
    }

    public bool DonePresenting()
    {
        return donePresenting;
    }

    // Public event handler methods.
    public void Item1Chosen()
    {
        itemChosen = 1;
    }

    public void Item2Chosen()
    {
        itemChosen = 2;
    }

    public void Item3Chosen()
    {
        itemChosen = 3;
    }

    // Private helper methods.
    private void GenerateItems()
    {
        if (levelNum == 1)
        {
            // End of level 1.
            Item[] commonItems = player1ItemCatalog.GetCommonItem(3);
            item1 = commonItems[0];
            item2 = commonItems[1];
            item3 = commonItems[2];
        }
        if (levelNum == 2)
        {
            // End of level 2.
            Item[] commonItems = player1ItemCatalog.GetCommonItem(2);
            item1 = commonItems[0];
            item2 = commonItems[1];

            Item[] rareItem = player1ItemCatalog.GetRareItem(1);
            item3 = rareItem[0];
        }
        if (levelNum == 3)
        {
            // End of level 3.
            Item[] rareItems = player1ItemCatalog.GetRareItem(3);
            item1 = rareItems[0];
            item2 = rareItems[1];
            item3 = rareItems[2];
        }
        if (levelNum == 4)
        {
            // End of level 4.
            item1 = player1ItemCatalog.GetRareItem(1)[0];
            item2 = player1ItemCatalog.GetEpicItem(1)[0];
            item3 = player1ItemCatalog.GetSpecialItem();
        }
    }

    private void SetItems()
    {
        itemChosen = 0;
        button1.SetItem(item1);
        button2.SetItem(item2);
        button3.SetItem(item3);
    }

    private void RetrieveReferences()
    {
        if (!retrievedReferences)
        {
            GameManager gameManager = Camera.main.GetComponent<GameManager>();

            if (gameManager.Ready())
            {
                singlePlayer = gameManager.SinglePlayer();
                player1 = gameManager.GetPlayer1();
                player2 = gameManager.GetPlayer2();

                player1Inventory = gameManager.player1Inventory;
                player2Inventory = gameManager.player2Inventory;

                player1ItemCatalog = player1Inventory.GetItemCatalog();

                retrievedReferences = true;
            }
        }
    }

    // FSM definition.
    private void UpdateFSM()
    {
        switch (state)
        {
            case ItemSelectionState.setPlayer1Items:
                ServiceSetPlayer1ItemsState();
                break;
            case ItemSelectionState.waitOnPlayer1:
                ServiceWaitOnPlayer1State();
                break;
            case ItemSelectionState.setPlayer2Items:
                ServiceSetPlayer2ItemsState();
                break;
            case ItemSelectionState.waitOnPlayer2:
                ServiceWaitOnPlayer2State();
                break;
            case ItemSelectionState.hide:
                ServiceHideState();
                break;
        }
    }

    // State service method definitions.
    private void ServiceSetPlayer1ItemsState()
    {
        // Set the header.
        string headerText = "SELECT A NEW ITEM";
        if (!singlePlayer)
        {
            headerText += " - PLAYER 1";
        }
        header.text = headerText;

        // Generate and set the items.
        GenerateItems();
        SetItems();

        // Transition to the next state.
        state = ItemSelectionState.waitOnPlayer1;
        stateEntryTime = Time.time;
    }

    private void ServiceWaitOnPlayer1State()
    {
        float dTime = Time.time - stateEntryTime;
        if (dTime <= WAITING_TIME && itemChosen == 0)
        {
            // Update our timer.
            int diff = (int) Mathf.Floor(dTime);

            timerText.text = (WAITING_TIME - diff).ToString();
        }
        else
        {
            // Retrieve the chosen item.
            if (itemChosen != 0)
            {
                Item chosenItem;
                if (itemChosen == 1)
                {
                    chosenItem = item1;
                }
                else if (itemChosen == 2)
                {
                    chosenItem = item2;
                }
                else
                {
                    chosenItem = item3;
                }

                // Add the item.
                if (chosenItem != null)
                {
                    player1Inventory.AddItem(chosenItem);
                }
            }

            // Transition to the next state.
            if (!singlePlayer)
            {
                state = ItemSelectionState.setPlayer2Items;
            }
            else
            {
                state = ItemSelectionState.hide;
            }
        }
    }

    private void ServiceSetPlayer2ItemsState()
    {
        // Set the header.
        string headerText = "SELECT A NEW ITEM - PLAYER 2";
        header.text = headerText;

        // Generate and set the items.
        GenerateItems();
        SetItems();

        // Transition to the next state.
        state = ItemSelectionState.waitOnPlayer2;
        stateEntryTime = Time.time;
    }

    private void ServiceWaitOnPlayer2State()
    {
        float dTime = Time.time - stateEntryTime;
        if (dTime <= WAITING_TIME && itemChosen == 0)
        {
            // Update our timer.
            int diff = (int)Mathf.Floor(dTime);

            timerText.text = (WAITING_TIME - diff).ToString();
        }
        else
        {
            // Retrieve the chosen item.
            if (itemChosen != 0)
            {
                Item chosenItem;
                if (itemChosen == 1)
                {
                    chosenItem = item1;
                }
                else if (itemChosen == 2)
                {
                    chosenItem = item2;
                }
                else
                {
                    chosenItem = item3;
                }

                // Add the item.
                if (chosenItem != null)
                {
                    player2Inventory.AddItem(chosenItem);
                }
            }

            // Transition to the next state.
            state = ItemSelectionState.hide;
        }
    }

    private void ServiceHideState()
    {
        if (!donePresenting)
        {
            gameObject.SetActive(false);
            donePresenting = true;
        }
    }
}
