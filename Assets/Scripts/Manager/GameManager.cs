using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Constants.
    private const float LEVEL_INFO_FLASH_TIME = 3.0f;

    // Public member variables.
    // Stuff for the players.
    public Sprite player1LancerSprite;
    public Sprite player1VanguardSprite;
    public Sprite player1TrailblazerSprite;

    public Sprite player2LancerSprite;
    public Sprite player2VanguardSprite;
    public Sprite player2TrailblazerSprite;

    public GameObject player1LancerUlt;
    public GameObject player1VanguardUlt;
    public GameObject player1TrailblazerUlt;
    public GameObject player2LancerUlt;
    public GameObject player2VanguardUlt;
    public GameObject player2TrailblazerUlt;

    public InventoryBehavior player1Inventory;
    public InventoryBehavior player2Inventory;

    // Stuff for levels.
    public ItemSelection itemSelection;
    public GameObject levelAttach;
    public EnemySpawnControl spawner;

    public TMPro.TextMeshProUGUI levelNum;
    public TMPro.TextMeshProUGUI levelName;

    public static bool winLoss;

    // Private member variables.
    private PlayerBehavior player1;
    private PlayerBehavior player2;

    private Vector3 player1StartPos = new Vector3(-1.5f, -2.5f, 0.0f);
    private Vector3 player2StartPos = new Vector3(1.5f, -2.5f, 0.0f);

    private bool ready;
    private bool singlePlayer;

    private float levelTime;

    // Start is called before the first frame update
    void Start()
    {
        // Perform some checks.
        Debug.Log("GameManager: Waking up!");
        Debug.Assert(itemSelection != null);

        // Make sure we have valid references to our inventories.
        Debug.Assert(player1Inventory != null);
        Debug.Assert(player2Inventory != null);

        // Build the players.
        BuildPlayers(MainMenu.player1Ship, MainMenu.player2Ship);

        // Set up the inventories.
        player1Inventory.SetPlayer(player1);
        if (player2 != null)
        {
            player2Inventory.SetPlayer(player2);
        }

        // This takes care of the levels - the actual gameplay.
        spawner = levelAttach.GetComponent<EnemySpawnControl>();
        StartCoroutine(StartGame());

        // This tells the ScoreManager we've determined whether this is single player.
        ready = true;

        winLoss = false;
    }

    // Update is called once per frame
    void Update()
    {
        DetectCondition();

        // Pseudo-invulnerability key
        if (Input.GetKeyDown("x"))
        {
            GameObject p = GameObject.FindWithTag("Player");
            p.transform.position = new Vector3(0.0f, -999.0f, 0.0f);
        }
        if (Input.GetKeyUp("x"))
        {
            GameObject p = GameObject.FindWithTag("Player");
            p.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }

    // Public methods.
    public bool Ready()
    {
        return ready;
    }

    public bool SinglePlayer()
    {
        return singlePlayer;
    }

    public PlayerBehavior GetPlayer1()
    {
        return player1;
    }

    public PlayerBehavior GetPlayer2()
    {
        return player2;
    }

    // Private helper methods.
    private void BuildPlayers(string ship1, string ship2)
    {
        Quaternion rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        if ((ship1 != null && ship1 != "None") && (ship2 == null || ship2 == "None"))
        {
            Debug.Log("GameManager: Singleplayer detected");
            player1StartPos = new Vector3(0.0f, -2.5f, 0.0f);
            player2Inventory.Hide();
            singlePlayer = true;
        }

        for (int i = 0; i < 2; i++)
        {
            bool playerOne = i == 0;
            string ship = playerOne ? ship1 : ship2;

            if (ship != null && ship != "None")
            {
                // Instantiate the game object.
                Vector3 startPos = playerOne ? player1StartPos : player2StartPos;
                GameObject player = Instantiate(Resources.Load("Prefabs/Player") as GameObject,
                                                    startPos,
                                                    rotation);
                player.tag = "Player";

                // Set up the behavior component.
                PlayerBehavior playerBehavior = player.GetComponent<PlayerBehavior>();
                playerBehavior.playerOne = playerOne;
                playerBehavior.SetShipName(ship);
                playerBehavior.SetInventory(playerOne ? player1Inventory : player2Inventory);

                if (playerOne)
                {
                    player1 = playerBehavior;
                }
                else
                {
                    player2 = playerBehavior;
                }

                // Set the sprite and add appropriate components.
                SpriteRenderer renderer = player.GetComponent<SpriteRenderer>();
                if (ship == "Lancer")
                {
                    renderer.sprite = playerOne ? player1LancerSprite : player2LancerSprite;

                    // Set stats.
                    playerBehavior.GetHealthBar().SetHitPoints(150.0f);
                    playerBehavior.SetWeaponDamage(25.0f);
                    playerBehavior.SetSpeed(7.0f);

                    // Add appropriate components.
                    player.AddComponent<BaseMovement>();
                    player.GetComponent<BaseMovement>().SetParent(playerBehavior);

                    player.AddComponent<BaseCollider>();
                    player.GetComponent<BaseCollider>().SetParent(playerBehavior);

                    player.AddComponent<BaseWeapon>();
                    player.GetComponent<BaseWeapon>().SetParent(playerBehavior);

                    player.AddComponent<LancerBasicAbility>();
                    player.GetComponent<LancerBasicAbility>().SetParent(playerBehavior);

                    player.AddComponent<LancerUltimateAbility>();
                    player.GetComponent<LancerUltimateAbility>().SetParent(playerBehavior);
                    SetupUltBars(playerOne, ship);
                }
                else if (ship == "Vanguard")
                {
                    renderer.sprite = playerOne ? player1VanguardSprite : player2VanguardSprite;

                    playerBehavior.GetHealthBar().SetHitPoints(200.0f);
                    playerBehavior.SetWeaponDamage(20.0f);
                    playerBehavior.SetSpeed(5.0f);

                    player.AddComponent<VanguardMovement>();
                    player.GetComponent<VanguardMovement>().SetParent(playerBehavior);

                    player.AddComponent<VanguardCollider>();
                    player.GetComponent<VanguardCollider>().SetParent(playerBehavior);

                    player.AddComponent<BaseWeapon>();
                    player.GetComponent<BaseWeapon>().SetParent(playerBehavior);
                    SetupUltBars(playerOne, ship);
                }
                else if (ship == "Trailblazer")
                {
                    // Otherwise, given the string is not null, it must be the trailblazer.
                    renderer.sprite = playerOne ? player1TrailblazerSprite : player2TrailblazerSprite;

                    playerBehavior.GetHealthBar().SetHitPoints(100.0f);
                    playerBehavior.SetWeaponDamage(20.0f);
                    playerBehavior.SetSpeed(10.0f);

                    // Add appropriate components
                    player.AddComponent<TrailblazerMovement>();
                    player.GetComponent<TrailblazerMovement>().SetParent(playerBehavior);

                    player.AddComponent<TrailblazerCollider>();
                    player.GetComponent<TrailblazerCollider>().SetParent(playerBehavior);

                    player.AddComponent<BaseWeapon>();
                    player.GetComponent<BaseWeapon>().SetParent(playerBehavior);

                    player.AddComponent<TrailblazerUltimateAbility>();
                    player.GetComponent<TrailblazerUltimateAbility>().SetParent(playerBehavior);
                    SetupUltBars(playerOne, ship);
                }
            }
        }
    }

    private void SetupUltBars(bool forPlayerOne, string shipName)
    {
        if(forPlayerOne)
        {
            if(shipName == "Lancer")
            {
                player1LancerUlt.SetActive(true);
                player1VanguardUlt.SetActive(false);
                player1TrailblazerUlt.SetActive(false);
            }
            else if(shipName == "Vanguard")
            {
                player1LancerUlt.SetActive(false);
                player1VanguardUlt.SetActive(true);
                player1TrailblazerUlt.SetActive(false);
            }
            else
            {
                player1LancerUlt.SetActive(false);
                player1VanguardUlt.SetActive(false);
                player1TrailblazerUlt.SetActive(true);
            }
        }
        else
        {
            if (shipName == "Lancer")
            {
                player2LancerUlt.SetActive(true);
                player2VanguardUlt.SetActive(false);
                player2TrailblazerUlt.SetActive(false);
            }
            else if (shipName == "Vanguard")
            {
                player2LancerUlt.SetActive(false);
                player2VanguardUlt.SetActive(true);
                player2TrailblazerUlt.SetActive(false);
            }
            else
            {
                player2LancerUlt.SetActive(false);
                player2VanguardUlt.SetActive(false);
                player2TrailblazerUlt.SetActive(true);
            }
        }
    }

    // Methods for gameplay/level management.
    private IEnumerator StartGame()
    {
        // Level 1 - Set number and name.
        SetLevelNumAndName(1, "A Walk in the Park (Except the Park is an Endless Void)");
        yield return new WaitForSeconds(LEVEL_INFO_FLASH_TIME);
        HideLevelNumAndName();

        // Level 1.
        levelAttach.AddComponent<LevelOne>();
        levelAttach.GetComponent<LevelOne>().SetSpawner(spawner);
        levelTime = levelAttach.GetComponent<LevelOne>().GetLevelTime();
        yield return new WaitForSeconds(levelTime);

        itemSelection.PresentItems(1);
        ClearEnemies();
        yield return new WaitUntil(() => itemSelection.DonePresenting());

        // Level 2 - Set number and name.
        SetLevelNumAndName(2, "Slightly More Enemies (This is Actually What we had Written Down)");
        yield return new WaitForSeconds(LEVEL_INFO_FLASH_TIME);
        HideLevelNumAndName();

        // Level 2.
        levelAttach.AddComponent<LevelTwo>();
        levelAttach.GetComponent<LevelTwo>().SetSpawner(spawner);
        levelTime = levelAttach.GetComponent<LevelTwo>().GetLevelTime();
        yield return new WaitForSeconds(levelTime);

        itemSelection.PresentItems(2);
        ClearEnemies();
        yield return new WaitUntil(() => itemSelection.DonePresenting());

        // Level 3 - Set number and name.
        SetLevelNumAndName(3, "Revenge of the Ship (Wait, what?)");
        yield return new WaitForSeconds(LEVEL_INFO_FLASH_TIME);
        HideLevelNumAndName();

        // Level 3.
        levelAttach.AddComponent<LevelThree>();
        levelAttach.GetComponent<LevelThree>().SetSpawner(spawner);
        levelTime = levelAttach.GetComponent<LevelThree>().GetLevelTime();
        yield return new WaitForSeconds(levelTime);

        itemSelection.PresentItems(3);
        ClearEnemies();
        yield return new WaitUntil(() => itemSelection.DonePresenting());

        // Level 4 - Set number and name.
        SetLevelNumAndName(4, "Stupid Level Name (We're out of ideas)");
        yield return new WaitForSeconds(LEVEL_INFO_FLASH_TIME);
        HideLevelNumAndName();

        // Level 4.
        levelAttach.AddComponent<LevelFour>();
        levelAttach.GetComponent<LevelFour>().SetSpawner(spawner);
        levelTime = levelAttach.GetComponent<LevelFour>().GetLevelTime();
        yield return new WaitForSeconds(levelTime);

        itemSelection.PresentItems(4);
        ClearEnemies();
        yield return new WaitUntil(() => itemSelection.DonePresenting());

        // TODO: Level 5 - Set number and name.
        /*SetLevelNumAndName(5, "Galaxy Buster");
        yield return new WaitForSeconds(LEVEL_INFO_FLASH_TIME);
        HideLevelNumAndName();*/

        // TODO: Level 5 (Boss fight).

        // Victory!
        if (player1.IsAlive() || player2.IsAlive())
        {
            winLoss = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void SetLevelNumAndName(int num, string name)
    {
        levelNum.text = "Level " + num.ToString();
        levelNum.enabled = true;
        levelName.text = name;
        levelName.enabled = true;
    }

    private void HideLevelNumAndName()
    {
        levelNum.enabled = false;
        levelName.enabled = false;
    }

    private void DetectCondition()
    {
        if (!player1.IsAlive())
        {
            Debug.Log("PLAYER DIED");
            winLoss = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    // Meant to be called at the end of every level.
    // Destroys all enemies and enemy bullets currently onscreen.
    private void ClearEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] enemyBullets = GameObject.FindGameObjectsWithTag("EnemyProjectile");
        foreach(GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        foreach(GameObject bullet in enemyBullets)
        {
            Destroy(bullet);
        }
    }
}