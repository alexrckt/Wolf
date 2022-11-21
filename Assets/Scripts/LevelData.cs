using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    public int levelID;
    public int levelScoreGoal;
    public int levelScoreGoalGained = 0;
    public bool levelCleared = false;

    public Dictionary<string, bool> aliveAnimals;

    public LevelData(int levelID, int levelScoreGoal)
    {
        this.levelID = levelID;
        this.levelScoreGoal = levelScoreGoal;
        aliveAnimals = new Dictionary<string, bool>();
    }
}
