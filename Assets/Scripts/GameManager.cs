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

    private static GameManager instance = null;

    public GameState currentGameState;
    public int currentLevel;
    public int livesInitial;
    public int score;
    public int bones;

    [Header("Score")]
    public int scoreForSheep;
    public int scoreForChicken;
    public int scoreForGopher;
    public HungerSlider hungerSlider;

    [HideInInspector] public Dictionary<int, LevelData> levelEntries;

    [HideInInspector]
    public int maxLevel = 2;
    [HideInInspector]
    public int livesCurrent;
    
    public float huntersCounter = 40f;
    [HideInInspector]
    public bool huntersArrived = false;
    [HideInInspector]
    public bool huntersCounterOn = false;
    [HideInInspector]
    public int deathsCounter = 0;

    [Header("Menus")]
    public GameObject pauseMenu;
    public GameObject levelWinMenu;
    public GameObject levelFailMenu;
    public GameObject gameOverMenu;
    public GameObject gameWinMenu;
    private GameObject currentActiveMenu;

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

        DontDestroyOnLoad(gameObject);
        ResetGame();
        currentGameState = GameState.MainMenu;
        levelEntries = new Dictionary<int, LevelData>();
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
        var duration = huntersCounter;
        while (duration >= 0)
        {
            duration -= Time.deltaTime;
            textField.SetText($"Hunters arrive in " + duration.ToString("0.00"));
            yield return null;
        }
        textField.SetText("THE CAVALRY IS HERE!");
        huntersCounterOn = false;
        huntersArrived = true;
        livesCurrent--;
        LevelFail(); // temp solution

    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        levelEntries[currentLevel].levelScoreGoalGained += scoreToAdd;
        hungerSlider.AddFood(scoreToAdd); // interacts with slider
        UpdateScoreText();
    }

    #region Update UI Text
    public void UpdateLivesText()
    {
        GameObject.Find("WolfLives").GetComponent<TextMeshProUGUI>().SetText(livesCurrent.ToString());
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

    #region End Game functions

    /// <summary>
    /// Called when Level WIN state reached
    /// </summary>
    public void LevelWin()
    {
        PauseGame(true);
        currentGameState = GameState.EndGame;

        if (currentLevel == maxLevel)
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
        ResetGame();
        gameOverMenu.SetActive(true);
        currentActiveMenu = gameOverMenu;
    }

    /// <summary>
    /// Called when all levels passed
    /// </summary>
    public void GameWin()
    {
        PauseGame(true);
        currentGameState = GameState.EndGame;
        ResetGame();
        gameWinMenu.SetActive(true);
        currentActiveMenu = gameWinMenu;
    }

    #endregion

    #region Scene Loading
    public void LoadLevel()
    {
        var levelName = $"Level {currentLevel}";
        SceneManager.LoadScene(levelName);
        StartCoroutine("WaitForSceneLoad", levelName);
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
        currentLevel = 0; // temp
        bones = 0;
        livesCurrent = livesInitial;
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
