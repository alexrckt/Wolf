using System.Collections;
using System.Collections.Generic;
using QuantumTek.QuantumUI;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        Playing,
        PauseMenu,
        EndGame
    }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    private static GameManager instance = null;
    private SoundManager soundManager;

    public GameState currentGameState;
    public Difficulty difficulty;
    public Dictionary<Difficulty, float> difficultyScoreMultiplier = new Dictionary<Difficulty, float>()
        {
            { Difficulty.Easy, 1f },
            { Difficulty.Medium, 1.2f },
            { Difficulty.Hard, 1.5f }
        };
    public int maxLevel = 4;
    public int currentLevel;
    //public int livesInitial;
    public int score;
    public int bones;
    EventManager em;
    public bool tutStarted = false;
    public bool level1Started = false;
    public bool level2Started = false;
    public bool level3Started = false;

    [Header("Score")]
    public int scoreForSheep;
    public int scoreForChicken;
    public int scoreForGopher;
    public float scoreForLife = 200f;
    public HungerSlider hungerSlider;

    [Header("Difficulty settings")]
    [Header("FOV")]
    public float[] dogFOVAngle = new float[3];
    public float[] dogFOVRadius = new float[3];
    public float[] farmerFOVAngle = new float[3];
    public float[] farmerFOVRadius = new float[3];
    //[HideInInspector] public float currentDogFOVAngle;
    //[HideInInspector] public float currentDogFOVRadius;
    //[HideInInspector] public float currentFarmerFOVAngle;
    //[HideInInspector] public float currentFarmerFOVRadius;
    [Header("Hunters timer")]
    public float[] huntersTimer = new float[3];
    //[HideInInspector] public float currentHuntersTimer;
    [Header("Lives")]
    public int[] livesTop = new int[3];
    public int[] livesStart = new int[3];

    [HideInInspector] public Dictionary<int, LevelData> levelEntries;

    
    public int livesCurrent;
    
    [HideInInspector]
    public bool huntersArrived = false;
    [HideInInspector]
    public bool huntersCounterOn = false;
    [HideInInspector]
    public int deathsCounter = 0;

    [Header("Menus")]
    public GameObject pauseMenu;
    public GameObject pauseAudioSettingsMenu;
    public GameObject levelWinMenu;
    public GameObject levelFailMenu;
    public GameObject gameOverMenu;
    public GameObject gameWinMenu;
    private GameObject currentActiveMenu;
    public int pinataLevelID = 4;



    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        difficulty = Difficulty.Easy;
        DontDestroyOnLoad(gameObject);
        ResetGame();
        currentGameState = GameState.MainMenu;
        em = FindObjectOfType <EventManager>();
        levelEntries = new Dictionary<int, LevelData>();
        soundManager = FindObjectOfType<SoundManager>();
        soundManager.PlayBackgroundMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (currentGameState)
            {
                case GameState.Playing:
                    PauseMenu(true);
                    break;
                case GameState.PauseMenu:
                    PauseMenu(false);
                    break;
            }
        }
    }

    public void WolfInjured()
    {
        livesCurrent--;
        deathsCounter++;
        UpdateLivesText();
        if (livesCurrent <= 0)
        {
            GameOver();;
        }
        else
        {
            LevelFail();
        }
    }

    public IEnumerator HuntersCounter()
    {
        var textField = GameObject.Find("HuntersCounter").GetComponent<TextMeshProUGUI>();
        var duration = huntersTimer[(int)difficulty];
        if (currentLevel == pinataLevelID)
        {duration = 30f;}
        while (duration >= 0)
        {
            duration -= Time.deltaTime;
            textField.SetText($"Hunters arrive in " + duration.ToString("0.00"));
            yield return null;
        }
        textField.SetText("THE CAVALRY IS HERE!");
        huntersCounterOn = false;
        huntersArrived = true;
        HuntersArrival();
    }

    public void HuntersArrival()
    {
        GameObject.Find("NPCs").transform.Find("Hunters").gameObject.SetActive(true);
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        levelEntries[currentLevel].levelScoreGoalGained += scoreToAdd;
        hungerSlider.AddFood(scoreToAdd); // interacts with slider
        UpdateScoreText();
    }

    public void tryAddLife()
    {
        if (livesCurrent < livesTop[(int)difficulty])
            livesCurrent++;
    }

    #region Update UI Text
    public void UpdateLivesText()
    {
        GameObject.Find("WolfLives").GetComponent<TextMeshProUGUI>().SetText($"{livesCurrent} / {livesTop[(int)difficulty]}");
    }
    public void UpdateHuntersCounterText()
    {
        GameObject.Find("HuntersCounter").GetComponent<TextMeshProUGUI>().SetText("");
    }

    public void UpdateScoreText()
    {
        GameObject.Find("Score").GetComponent<TextMeshProUGUI>().SetText($"Score: {score}");
    }
    #endregion

    #region Start / End Game functions

    public void StartGame(int difficulty)
    {
        this.difficulty = (Difficulty)difficulty;
        LoadLevel();
    }

    /// <summary>
    /// Called when Level WIN state reached
    /// </summary>
    public void LevelWin()
    {
        PauseGame(true);
        currentGameState = GameState.EndGame;

        if (currentLevel >= maxLevel)
        {
            GameWin();
            return;
        }

        currentLevel++;
        levelWinMenu.SetActive(true);
        currentActiveMenu = levelWinMenu;
    }

    /// <summary>
    /// Called when Level FAIL state reached
    /// </summary>
    public void LevelFail()
    {
        PauseGame(true);
        currentGameState = GameState.EndGame;
        levelFailMenu.SetActive(true);
        currentActiveMenu = levelFailMenu;
    }

    /// <summary>
    /// Called when GAME OVER state reached (no more lives left)
    /// </summary>
    public void GameOver()
    {
        PauseGame(true);
        currentGameState = GameState.EndGame;
        gameOverMenu.SetActive(true);
        var finalScore = score * difficultyScoreMultiplier[difficulty];
        gameOverMenu.transform.GetComponentInChildren<TextMeshProUGUI>().SetText(
            "GAME OVER\n" +
            $"Score: {score}\n" +
            $"Difficulty: x{difficultyScoreMultiplier[difficulty]}\n" +
            $"<color=#65BB6B><b>Final score: {finalScore}</b>"
            );
        currentActiveMenu = gameOverMenu;
        ResetGame();
    }

    /// <summary>
    /// Called when all levels passed
    /// </summary>
    public void GameWin()
    {
        PauseGame(true);
        currentGameState = GameState.EndGame;
        gameWinMenu.SetActive(true);
        var finalScore = score * difficultyScoreMultiplier[difficulty] + (livesCurrent * scoreForLife);
        gameWinMenu.transform.GetComponentInChildren<TextMeshProUGUI>().SetText(
            "YOU WIN!\n" +
            $"Score: {score}\n" +
            $"Difficulty: x{difficultyScoreMultiplier[difficulty]}\n" +
            $"Lives left: {livesCurrent} x {scoreForLife}\n" +
            $"<color=#65BB6B><b>Final score: {finalScore}</b>"
            );
        currentActiveMenu = gameWinMenu;
        ResetGame();
    }

    #endregion

    #region Scene Loading
    public void LoadLevel()
    {
        var levelName = $"Level {currentLevel}";
        SceneManager.LoadScene(levelName);
        StartCoroutine("WaitForSceneLoad", levelName);
        if (currentLevel == 0)
        {
            livesCurrent = livesStart[(int)difficulty];
            if ( !tutStarted)
            {em.TutStart();
            tutStarted = true;}
            
        }
        if (currentLevel == 1 && !level1Started)
        {
            em.Level0Complete();
            level1Started = true;
        }
        if (currentLevel == 2 && !level2Started)
        {
            em.Level1Complete();
            level2Started = true;
        }
        if (currentLevel == 3 && !level3Started)
        {
            em.TutComplete();
            level3Started = true;
        }


        
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        currentGameState = GameState.MainMenu;
    }

    IEnumerator WaitForSceneLoad(string sceneName)
    {
        while (SceneManager.GetActiveScene().name != sceneName)
        {
            yield return null;
        }

        if (levelEntries.ContainsKey(currentLevel))
        {
            var levelData = levelEntries[currentLevel];
            hungerSlider.SetGoalHunger(levelData.levelScoreGoal);
            hungerSlider.AddFood(levelData.levelScoreGoalGained);
        }
        else
        {
            var levelManager = FindObjectOfType<LevelManager>();
            var levelData = new LevelData(currentLevel, levelManager.scoreGoal);
            var animals = levelManager.GetAnimals();
            foreach (var animal in animals)
            {
                levelData.aliveAnimals.Add(animal.name, true);
            }
            levelManager.levelData = levelData;

            levelEntries.Add(currentLevel, levelData);
            hungerSlider.SetGoalHunger(levelData.levelScoreGoal);
        }

        currentGameState = GameState.Playing;
    }
    #endregion

    #region Pause Menu
    public void PauseMenu(bool on)
    {
        if (on)
        {
            pauseMenu.SetActive(true);
            currentActiveMenu = pauseMenu;
            PauseGame(true);
            currentGameState = GameState.PauseMenu;
        }
        else
        {
            pauseMenu.SetActive(false);
            PauseGame(false);
            currentGameState = GameState.Playing;
        }
    }
    private void PauseGame(bool pause)
    {
        Time.timeScale = pause ? 0f : 1f;
    }
    #endregion

    #region Level Win/Fail Menu

    public void LoadNextLevel()
    {
        currentActiveMenu.SetActive(false);
        LoadLevel();
        PauseGame(false);
    }

    #endregion

    #region Common
    private void ResetGame()
    {
        levelEntries?.Clear();
        score = 0;
        deathsCounter = 0;
        currentLevel = 0;
        bones = 0;
        livesCurrent = livesStart[(int)difficulty];
        huntersArrived = false;
    }

    public void BackToMain()
    {
        PauseGame(false);
        currentActiveMenu.SetActive(false);
        ResetGame();
        LoadMainMenu();
    }
    #endregion

}
