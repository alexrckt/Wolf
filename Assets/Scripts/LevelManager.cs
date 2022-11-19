using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int sheepStolenGoal;
    public int sheepStolenCounter = 0;

    private GameManager gameManager;
    private GameObject inGameUI;
    private TextMeshProUGUI sheepStolenCounterUI;

    [HideInInspector] public bool levelCleared = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        inGameUI = GameObject.Find("InGameUI");
        sheepStolenCounterUI = inGameUI.GetComponentInChildren<TextMeshProUGUI>();

        UpdateSheepCounterText();
        gameManager.UpdateLivesText();
        gameManager.UpdateHuntersCounterText();
        gameManager.UpdateScoreText();
        gameManager.huntersCounterOn = false;
        gameManager.huntersArrived = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (sheepStolenCounter >= sheepStolenGoal)
        {
            gameManager.LevelWin();
            sheepStolenCounter = 0;
        }
    }

    private void LevelCleared()
    {
        levelCleared = true;
    }

    public void SheepStolen(int i)
    {
        sheepStolenCounter += i;
        gameManager.AddScore(i * gameManager.scoreForSheep);
        UpdateSheepCounterText();
    }

    void UpdateSheepCounterText()
    {
        sheepStolenCounterUI.text = $"{sheepStolenCounter} / {sheepStolenGoal}";
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
