using System.Collections;
using System.Collections.Generic;
using QuantumTek.QuantumUI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Pause
    }

    private static GameManager instance = null;
    public GameObject pauseMenu;

    public GameState currentGameState;
    public int currentLevel;
    public int lives;
    public int points;
    public int bones;
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
        currentLevel = 1;
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
                    Pause();
                    break;
                case GameState.Pause:
                    Resume();
                    break;
            }
        }
    }

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

    #region Pause Menu
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        currentGameState = GameState.Pause;
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        currentGameState = GameState.Playing;
    }

    public void BackToMain()
    {
        Resume();
        LoadMainMenu();
    }
    #endregion

}
