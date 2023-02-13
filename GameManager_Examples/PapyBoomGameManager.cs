using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * PAPY BOOM - all rights reserved
 * Currently in demo version
 * Code written by Pauline Andrault
 * 
 * Note : we refer to the playable characters as "boomers" in our design and code.
 */

public sealed class GameManager : MonoBehaviour
{
    private GameManager() { }
    public static GameManager Singleton { get; private set; }

    #region Variables

    [Header("Level Design Info")]
    [SerializeField] List<LevelInfo> _levels = new List<LevelInfo>();   // set by the game designer in the editor

    [Header("All playable boomers")]
    [SerializeField] GameObject _prefabLucien;  // demo version : all boomers prefabs held here. prod version : we may change the way we hold this data (specific class or scriptable object with prefab + equipment +...)
    [SerializeField] GameObject _prefabMarvin;
    [SerializeField] GameObject _prefabMaggie;
    
    // Current level relative data. Default values : item with lowest index in levels and boomers lists.
    int _currentLevelIndex = 0;
    GameObject _prefabBoomer1; // demo version : prefab used to hold the "current boomer" generic info.
    GameObject _prefabBoomer2;
    GameObject _boomer1;    // demo version : this GO is used to hold the current instance of the character in the Level scene
    GameObject _boomer2;

    // References
    InputManager _inputManager;
    BoomerData _boomerData1;
    BoomerData _boomerData2;
    [SerializeField] public InventoryBase PlayerInventory;  // demo version : inventory not fully implemented yet. Must be set in the editor.
    [SerializeField] public InventoryBase LucienInventory;
    [SerializeField] public InventoryBase MarvinInventory;
    [SerializeField] public InventoryBase MaggieInventory;

    // Public variables
    public GameObject Boomer1 { get => _boomer1; set => _boomer1 = value; } // used to communicate with other classes in Game scene
    public GameObject Boomer2 { get => _boomer2; set => _boomer2 = value; }
    public GameObject PrefabBoomer1 { get => _prefabBoomer1; set => _prefabBoomer1 = value; }   // used to communicate with other classes in Menu scenes
    public GameObject PrefabBoomer2 { get => _prefabBoomer2; set => _prefabBoomer2 = value; }
    public List<LevelInfo> Levels { get => _levels; set => _levels = value; }
    public int SelectedLevelIndex { get => _currentLevelIndex; set => _currentLevelIndex = value; }

    #endregion

    #region Initialization Game Manager
    // We chose the methods to execute when a scene is loaded depending on the scene name
    private void OnEnable()
    {
        SceneManager.sceneLoaded += InitializeScene;
    }

    private void InitializeScene(Scene scene, LoadSceneMode mode)
    {
        // When a level is loaded (game scene)
        if (scene.name == "Level" && gameObject.name == "GameManager") 
            OnStartLevel(); 

        // When the level selection Menu is loaded (menu scene)
        if (scene.name == "MenuLevels" && gameObject.name == "GameManager") 
            OnStartLevelSelection();
    }

    private void Awake()
    {
        // Singleton pattern
        if (Singleton != null && Singleton != this)
        {
            Destroy(gameObject);
            return;
        }
        Singleton = this;
        DontDestroyOnLoad(gameObject);

        // Save
        CheckSave();

        // Test environment - for test purpose, a game object called "GameManagerTest" is set in the Level scene. It should not execute everything the real GameManager does.
        if (gameObject.name != "GameManager")
            GameManagerTestAwake();
    }

    // Method used for test environment only
    private void GameManagerTestAwake()
    {
        // If the real GameManager exists (if the test is run from the Menu scene for example), destroy the GameManagerTest.
        if (gameObject.name == "GameManager")
            return;

        if (GameObject.Find("GameManager") != null)
        {
            Destroy(this);
            return;
        }
        else if (SceneManager.GetActiveScene().name == "Level")
        {
            OnStartLevel();
        }
    }

    #endregion

    #region Initialization Level

    public void OnStartLevel()
    {
        // Get all refs
        _inputManager = FindObjectOfType<InputManager>();

        // Instantiate boomers and get BoomerData references (0 = First boomer ; 1 = Second Boomer)
        _boomer1 = Instantiate(_prefabBoomer1);
        _boomer2 = Instantiate(_prefabBoomer2);
        _boomerData1 = _boomer1.GetComponent<BoomerData>();
        _boomerData2 = _boomer2.GetComponent<BoomerData>();

        // Update _boomers variables in InputManager
        _inputManager.InitializeInputManager();

        // Update _boomers variables in BoomerData
        _boomerData1.BoomerNumber = 0;
        _boomerData2.BoomerNumber = 1;

        // Update Level in WaveSystem
        FindObjectOfType<WaveSystem>().Level = _levels[_currentLevelIndex].CurrentLevelData;

        // Start the game in Level Manager
        FindObjectOfType<LevelManager>().IsStartTimerOn = true;
    }

    #endregion

    #region Initialization Menu LevelSelection 

    public void OnStartLevelSelection()
    {
        // demo version : only 1 mode, 1 world, 1 difficulty
        // For now : data updates in the LevelSelectionMenu scene are handled by the LevelSelectionMenu class. May move to the GameManager later.
    }
    #endregion

    #region Menu LevelSelection Methods
    
    public void SelectLevel(int index)
    {
        if (!_levels[index].Locked) 
            _currentLevelIndex = index;
    }
    #endregion

    #region Save Methods
    private void CheckSave()
    {
        // Get inventory saves (Json)
        GetInventorySave();

        // Is there an existing save file ?
        // If YES : get the latest data of Level (lock status, score), Boomers. Prod version : we should also get data about the latest World, Mode, Difficulty here.
        if (PlayerPrefs.GetString("HasSave") == "Yes")
        {
            GetLevelSave();
            GetBoomersSave();
        }

        // If NO SAVE : set the default values for Level (lock status, score), Boomers. Prod version : add World, Mode, Difficulty.
        else
        {
            Debug.Log("No save found");

            // Set new save
            PlayerPrefs.SetString("HasSave", "Yes");

            // Set the current level to 0
            PlayerPrefs.SetInt("CurrentLevel", 0);
            _currentLevelIndex = 0;

            // Lock every levels except the first level
            for (int i = 1; i < _levels.Count; i++)
            {
                _levels[i].Locked = true;
                // Set and save best score to 0
                PlayerPrefs.SetInt(i.ToString(), 0);
            }

            // Set the default boomers (playable characters) : Lucien 1, Marvin 2.
            _prefabBoomer1 = _prefabLucien;
            _prefabBoomer2 = _prefabMarvin;

            // Save current boomers data
            PlayerPrefs.SetString("Boomer1", BoomerName.Lucien.ToString());
            PlayerPrefs.SetString("Boomer2", BoomerName.Marvin.ToString());
        }
    }

    private void GetLevelSave()
    {
        // We iterate on all levels. We unlock level n+1 when level n saved score is >= 1.
        _currentLevelIndex = 0;
        for (int i = 0; i < _levels.Count; i++)
        {
            if (PlayerPrefs.GetInt(i.ToString()) > 0)
            {
                // We cache the best score in the Game Manager
                _levels[i].BestScore = PlayerPrefs.GetInt(i.ToString());

                if (i + 1 < _levels.Count)
                {
                    // We unlock the next level (if existing)
                    _levels[i + 1].Locked = false;

                    // The last unlocked level becomes the Current Level
                    _currentLevelIndex = i + 1;
                }
            }
            else
                break;
        }
    }

    private void GetBoomersSave()
    {
        // We set boomers 1 and 2 with the latest boomers used and saved.
        string boomer1 = PlayerPrefs.GetString("Boomer1");
        string boomer2 = PlayerPrefs.GetString("Boomer2");
        switch (boomer1)
        {
            case "Lucien":
                _prefabBoomer1 = _prefabLucien;
                break;
            case "Marvin":
                _prefabBoomer1 = _prefabMarvin;
                break;
            case "Maggie":
                _prefabBoomer1 = _prefabMaggie;
                break;
            default:
                break;
        }
        switch (boomer2)
        {
            case "Lucien":
                _prefabBoomer2 = _prefabLucien;
                break;
            case "Marvin":
                _prefabBoomer2 = _prefabMarvin;
                break;
            case "Maggie":
                _prefabBoomer2 = _prefabMaggie;
                break;
            default:
                break;
        }
    }
    private void GetInventorySave()
    {
        // Get saved data foreach inventory (Json)
        // Note : demo version, inventory system not fully implemented. Code must be refactorized later.
        PlayerInventory.Load();
        LucienInventory.Load();
        MarvinInventory.Load();
        MaggieInventory.Load();
    }

    public void SetScore(int score)
    {
        // Is the new score better than the last saved best score ? If not : do nothing.
        if (score <= _levels[_currentLevelIndex].BestScore) return;

        // New best score : set the new value in the Game Manager and save it.
        _levels[_currentLevelIndex].BestScore = score;
        PlayerPrefs.SetInt(_currentLevelIndex.ToString(), score);

        // Unlock next level (if existing) if score >= 1
        if (score > 0 && _currentLevelIndex + 1 < _levels.Count) 
            _levels[_currentLevelIndex + 1].Locked = false;

        // Prod version : unlock the same level in upper Difficulty if score == 3
        // ..................
    }
        #endregion
    
}