using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private SaveManager saveManager;
    public ChestManager chestManager;
    public AudioManager audioManager;
    public EnemyManager enemyManager;
    public StoryProgression storyProgression;

    [SerializeField]
    private AudioClip mainMenuBGM;
    [SerializeField]
    private AudioClip battleBGM;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        UIManager.instance = uiManager;
        chestManager = GetComponent<ChestManager>();
        ChestManager.instance = chestManager;
        enemyManager = GetComponent<EnemyManager>();
        EnemyManager.instance = enemyManager;
        audioManager = GetComponent<AudioManager>();
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //Player Info Region
    public UnitInfo playerInfo;

    public Inventory inventory;

    public UnitInfo encounteredEnemy;

    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private UnitInfo playerInfoPrefab;

    //Scene Change Region
    [SerializeField]
    private SceneInfo overworldSceneInfo;

    public Vector2 savedPlayerPosition;
    private string lastLoadedScene;

    private MapRegion currentMapRegion;
    private MapRegion previousMapRegion;

    public SceneInfo currentScene;

    [SerializeField]
    private Animator sceneTransitionAnimator;

    public List<UnitInfo> valleySpawns = new List<UnitInfo>();

    public bool hasReceivedInput = false;

    public enum GameState { MAINMENU, NORMAL, PAUSE, MENU, DIALOG, LOADING, BATTLE, CUTSCENE }
    public GameState gameState;
    private GameState previousState;
    private void ChangeState(GameState newState)
    {
        if (gameState != newState)
        {
            Debug.Log("Changing to " + newState);
            previousState = gameState;
            gameState = newState;

        }
    }

    private Transform player;
    public Transform Player
    {
        get
        {
            if (player == null)
            {
                if (GameObject.FindGameObjectWithTag("Player") == null)
                {
                    GameObject newPlayer = Instantiate(playerPrefab);
                    player = newPlayer.transform;
                }
                else
                {
                    player = GameObject.FindGameObjectWithTag("Player").transform;
                }

            }
            return player;
        }
        set
        {
            player = value;
        }
    }

    private void Start()
    {
        if(gameState == GameState.MAINMENU)
        {
            uiManager.ShowMainMenu();
            audioManager.SetBGM(mainMenuBGM);
        }
        else
        {

        }
        playerInfo = Instantiate(playerInfoPrefab);
        playerInfo.Setup();
        playerInfo.ResetStatsForLevel();
    }

    private void Update()
    {
        hasReceivedInput = false;
        ManageState();
    }

    private void ManageState()
    {
        switch(gameState)
        {
            case GameState.MAINMENU:

                break;
            case GameState.NORMAL:
                //Start Button
                if (Input.GetKeyDown("joystick button 7") || Input.GetKeyDown(KeyCode.Z))
                {
                    ShowPauseMenu();
                }
                else if (Input.GetKey("joystick button 7"))
                {

                }
                else if (Input.GetKeyUp("joystick button 7"))
                {

                }
                break;
            case GameState.DIALOG:

                break;
            case GameState.MENU:
                if(Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Escape))
                {
                    ChangeState(uiManager.CloseCurrentMenu());
                    hasReceivedInput = true;
                }
                break;
            case GameState.PAUSE:
                if (Input.GetKeyDown("joystick button 7") || Input.GetKeyDown(KeyCode.Z))
                {
                    hasReceivedInput = true;
                    HidePauseMenu();
                }
                break;
        }
    }

    public void ShowPauseMenu()
    {
        ChangeState(GameState.PAUSE);
        uiManager.ShowPauseMenu();
    }

    private void HidePauseMenu()
    {
        ChangeState(GameState.NORMAL);
        uiManager.HidePauseMenu();
    }

    public void ShowInGameMenu()
    {
        ChangeState(GameState.MENU);
        uiManager.ShowInGameMenu();
    }

    private void HideInGameMenu()
    {
        ChangeState(GameState.NORMAL);
        uiManager.HideInGameMenu();
    }

    public void ShowInventory()
    {
        uiManager.ShowInventory();
        //HideInGameMenu();
        //ChangeState(GameState.MENU);
    }

    public void ShowSpellsMenu()
    {
        uiManager.ShowSpellsMenu();
        ChangeState(GameState.MENU);
    }

    public void ShowEquipMenu()
    {
        uiManager.ShowEquipMenu();
        ChangeState(GameState.MENU);
    }

    public void ShowStatsMenu()
    {
        uiManager.ShowStatsMenu(playerInfo);
        ChangeState(GameState.MENU);
    }

    public void ShowKillsMenu()
    {
        uiManager.ShowKillsMenu();
        ChangeState(GameState.MENU);
    }

    public void ShowShopMenu(ShopkeeperInteraction shopKeeper)
    {
        UIManager.instance.ShowShopInventory(shopKeeper);
        ChangeState(GameState.MENU);
    }

    public void HideShopMenu()
    {
        UIManager.instance.HideShopInventory();
        ChangeState(GameState.NORMAL);
    }


    public void StartBattle(UnitInfo enemyInfo)
    {
        encounteredEnemy = enemyInfo;
        savedPlayerPosition = Player.position;
        lastLoadedScene = SceneManager.GetActiveScene().name;
        audioManager.SetBGM(battleBGM);
        StartCoroutine(FadeToNewScene("BattleScene", GameState.BATTLE));
    }

    public void ReturnFromBattle()
    {
        StartCoroutine(FadeToNewScene(lastLoadedScene, GameState.NORMAL));
        AddValleySpawn(encounteredEnemy);

        if(encounteredEnemy.unitName == "Slime King")
        {
            storyProgression.killedSlimeKing = true;
        }
    }

    private void AddValleySpawn(UnitInfo newEnemy)
    {
        bool alreadyAdded = false;
        for (int i = 0; i < valleySpawns.Count; i++)
        {
            if(valleySpawns[i].unitName == newEnemy.unitName)
            {
                alreadyAdded = true;
                break;
            }
        }

        if(!alreadyAdded)
        {
            valleySpawns.Add(newEnemy);
        }
    }

    public void EnterDialogState()
    {
        ChangeState(GameState.DIALOG);
    }

    public void ExitDialogState()
    {
        ChangeState(previousState);
    }

    public void EnterMenuState()
    {
        ChangeState(GameState.MENU);
    }

    public void ExitMenuState()
    {
        ChangeState(previousState);
    }

    public void EnterCutSceneState()
    {
        player.GetComponent<PlayerControls>().StopMoving();
        ChangeState(GameState.CUTSCENE);
    }

    public void ExitCutSceneState()
    {
        ChangeState(GameState.NORMAL);
    }

    public void StartNewGame()
    {
        uiManager.HideMainMenu();
        LoadNewScene(overworldSceneInfo, 0, Vector2.down);
    }

    public void LoadNewScene(SceneInfo sceneInfo, int entryPoint, Vector2 newFacing)
    {
        StartCoroutine(FadeToNewScene(sceneInfo.sceneName, GameState.NORMAL));
        currentScene = sceneInfo;
        savedPlayerPosition = sceneInfo.entrances[entryPoint];
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {

    }

    public void ChangeMap(Vector2 minBounds, Vector2 maxBounds, Vector2 playerChange, MapRegion mapRegion)
    {
        if (gameState == GameState.LOADING)
            return;
        StartCoroutine(TransitionToNewMap(minBounds, maxBounds, playerChange, mapRegion));
    }

    private IEnumerator TransitionToNewMap(Vector2 minBounds, Vector2 maxBounds, Vector2 playerChange, MapRegion mapRegion)
    {
        GameState previousState = gameState;
        ChangeState(GameState.LOADING);
        yield return new WaitForSeconds(.25f);
        Camera.main.GetComponent<CameraMovement>().SetMinBounds(minBounds);
        Camera.main.GetComponent<CameraMovement>().SetMaxBounds(maxBounds);
        Player.position += (Vector3)playerChange;
        if (currentMapRegion != null)
        {
            previousMapRegion = currentMapRegion;
            previousMapRegion.OnDeactivate();
        }
        currentMapRegion = mapRegion;
        currentMapRegion.OnActivate();
        ChangeState(previousState);
    }

    private IEnumerator FadeOut()
    {
        sceneTransitionAnimator.SetTrigger("circleWipeOut");
        yield return null;
    }

    private IEnumerator FadeIn()
    {
        sceneTransitionAnimator.SetTrigger("circleWipeIn");
        yield return null;
    }

    private IEnumerator FadeToNewScene(string sceneName, GameState newState)
    {
        ChangeState(GameState.LOADING);
        sceneTransitionAnimator.SetTrigger("fadeOut");
        float waitTime = 1f;
        yield return new WaitForSeconds(waitTime * .5f);
        SceneManager.LoadScene(sceneName);
        yield return new WaitForSeconds(waitTime * .5f);
        sceneTransitionAnimator.SetTrigger("fadeIn");
        if (newState == GameState.NORMAL)
        {
            Player.position = savedPlayerPosition;
            Camera.main.transform.position = new Vector3(savedPlayerPosition.x, savedPlayerPosition.y, -10f);
            RaycastHit2D[] hits = Physics2D.RaycastAll(player.position, Vector2.zero);
            foreach(RaycastHit2D hit in hits)
            {
                if(hit.transform.GetComponent<MapRegion>())
                {
                    currentMapRegion = hit.transform.GetComponent<MapRegion>();
                    break;
                }
            }
            Camera.main.GetComponent<CameraMovement>().SetMinBounds(currentMapRegion.GetMinBounds());
            Camera.main.GetComponent<CameraMovement>().SetMaxBounds(currentMapRegion.GetMaxBounds());
            currentMapRegion.OnActivate();
        }
        ChangeState(newState);
    }

    public void ButtonNotSetup()
    {
        DialogManager.instance.ShowSimpleDialog("This button hasn't been setup yet, sorry");
    }

    public void SaveGame()
    {
        GameState _state = gameState;
        ChangeState(GameState.LOADING);
        saveManager.SaveGameData();
        ChangeState(_state);
    }

    public void LoadGame()
    {
        uiManager.HideMainMenu();
        StartCoroutine(LoadFromSavedGame());
    }

    private IEnumerator LoadFromSavedGame()
    {
        float waitTime = 1f;
        StartCoroutine(FadeOut());
        yield return new WaitForSeconds(waitTime * .5f);
        saveManager.LoadGameData();
        uiManager.HidePauseMenu();
        StartCoroutine(FadeIn());
        yield return new WaitForSeconds(waitTime * .5f);
        ChangeState(GameState.NORMAL);
        player.position = savedPlayerPosition;
        Camera.main.transform.position = new Vector3(player.position.x, player.position.y, -10f);
    }
}