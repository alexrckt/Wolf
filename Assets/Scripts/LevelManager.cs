using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //public int sheepStolenGoal;
    //public int sheepStolenCounter = 0;
    public int levelID;
    public int scoreGoal;
    public LevelData levelData;

    private GameManager gameManager;
    private GameObject inGameUI;
    private GrabAnimals wolfGrabber;
    //private TextMeshProUGUI sheepStolenCounterUI;

    //[HideInInspector] public bool levelCleared = false;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        levelData = (gameManager.levelEntries.ContainsKey(levelID)) ?
            gameManager.levelEntries[levelID]:
            new LevelData(levelID, scoreGoal);
    }

    // Start is called before the first frame update
    void Start()
    {
        inGameUI = GameObject.Find("InGameUI");
        wolfGrabber = FindObjectOfType<GrabAnimals>();
        //sheepStolenCounterUI = inGameUI.GetComponentInChildren<TextMeshProUGUI>();

        //UpdateSheepCounterText();
        gameManager.UpdateLivesText();
        gameManager.UpdateHuntersCounterText();
        gameManager.UpdateScoreText();
        gameManager.huntersCounterOn = false;
        gameManager.huntersArrived = false;
        //FindObjectOfType<HungerSlider>().SetGoalHunger(scoreGoal);
    }

    // Update is called once per frame
    void Update()
    {
        //if (sheepStolenCounter >= sheepStolenGoal)
        //{
        //    gameManager.LevelWin();
        //    sheepStolenCounter = 0;
        //}
        if (!levelData.levelCleared && levelData.levelScoreGoalGained >= scoreGoal)
        {
            LevelCleared();
        }

        if (Input.GetKeyDown(KeyCode.Space)
            && levelData.levelCleared
            && wolfGrabber.isTouchingDropPoint)
        {
            gameManager.LevelWin();
            //Debug.Log($"Level cleared: {levelData.levelCleared}; isTouching: {wolfGrabber.isTouchingDropPoint}; Score gained: {levelData.levelScoreGoalGained}");
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
        //sheepStolenCounter += i;
        gameManager.AddScore(i * gameManager.scoreForSheep);
        //UpdateSheepCounterText();
    }

    //void UpdateSheepCounterText()
    //{
    //    sheepStolenCounterUI.text = $"{sheepStolenCounter} / {sheepStolenGoal}";
    //}

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
