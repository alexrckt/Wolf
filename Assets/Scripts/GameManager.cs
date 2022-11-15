using System.Collections;
using System.Collections.Generic;
using QuantumTek.QuantumUI;
using TMPro;
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

    public GameObject pauseMenu;
    public GameObject levelWinMenu;
    public GameObject levelFailMenu;
    public GameObject gameOverMenu;
    public GameObject gameWinMenu;
    private GameObject currentActiveMenu;

    public GameState currentGameState;
    public int currentLevel;
    public int maxLevel;
    public int livesCurrent;
    public int livesInitial;
    public int score;
    public int bones;
    public float huntersCounter;
    public bool huntersCounterOn = false;
    public int deathsCounter;

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
            textField.SetText(duration.ToString("0.00"));
            yield return null;
        }
        textField.SetText("CAVALRY'S HERE!");
        huntersCounterOn = false;
    }

    public void UpdateLivesText()
    {
        GameObject.Find("WolfLives").GetComponent<TextMeshProUGUI>().SetText(livesCurrent.ToString());
    }
    public void UpdateHuntersCounterText()
    {
        GameObject.Find("HuntersCounter").GetComponent<TextMeshProUGUI>().SetText("");
    }

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
        SceneManager.LoadScene($"Level {currentLevel}");
        currentGameState = GameState.Playing;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        currentGameState = GameState.MainMenu;
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
    public void PauseGame(bool pause)
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
        currentLevel = 1;
        bones = 0;
        livesCurrent = livesInitial;
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
