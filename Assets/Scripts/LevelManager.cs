using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int levelID;
    public int scoreGoal;
    public LevelData levelData;

    private GameManager gameManager;
    private GameObject inGameUI;
    private GrabAnimals wolfGrabber;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        var slider = FindObjectOfType<HungerSlider>();
        slider.ResetColor();

        if (gameManager.levelEntries.ContainsKey(levelID))
        {
            levelData = gameManager.levelEntries[levelID];
            DeleteAnimals();
            if(levelData.levelCleared)
            {
                slider.HungerComplited();
            }
        }
    }

    public List<GameObject> GetAnimals()
    {
        return GameObject.FindGameObjectsWithTag("Sheep")
                            .Concat(GameObject.FindGameObjectsWithTag("Chicken"))
                            .Concat(GameObject.FindGameObjectsWithTag("Gopher"))
                            .ToList();
    }
    private void DeleteAnimals()
    {
        foreach(var animal in levelData.aliveAnimals)
        {
            if(!animal.Value)
            {
                Destroy(GameObject.Find(animal.Key));
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        inGameUI = GameObject.Find("InGameUI");
        wolfGrabber = FindObjectOfType<GrabAnimals>();

        gameManager.UpdateLivesText();
        gameManager.UpdateHuntersCounterText();
        gameManager.UpdateScoreText();
        gameManager.huntersCounterOn = false;
        gameManager.huntersArrived = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (levelData != null && 
            !levelData.levelCleared 
            && levelData.levelScoreGoalGained >= scoreGoal)
        {
            LevelCleared();
        }

        if (Input.GetKeyDown(KeyCode.Space)
            && levelData.levelCleared
            && wolfGrabber.isTouchingDropPoint)
        {
            gameManager.LevelWin();
        }
    }

    private void LevelCleared()
    {
        levelData.levelCleared = true;
        // Now showing proposal to left level by running into forest and press Enter
    }

    public LevelData CreateLevelData()
    {
        levelData = new LevelData(levelID, scoreGoal);
        return levelData;
    }

    public void SetLevelData(LevelData data)
    {
        levelData = data;
    }

    public void SheepStolen(int i)
    {
        gameManager.AddScore(i * gameManager.scoreForSheep);
    }

    public void ChickenEaten()
    {
        gameManager.AddScore(gameManager.scoreForChicken);
    }

    public void GopherEaten()
    {
        gameManager.livesCurrent += 1;
        gameManager.AddScore(gameManager.scoreForGopher);
        gameManager.UpdateLivesText();
    }
}
