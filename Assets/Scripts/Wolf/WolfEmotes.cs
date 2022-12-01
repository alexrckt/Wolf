using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfEmotes : MonoBehaviour
{
    public Sprite[] emotes; // 0 = yum, 1 = gotcham, 2 = back to forest, 3 - lupus chickeni, 
                            // 4 - wolved down
    GameObject currentEmote;
    public float emotingTimer = 2f;
    public float emotingCurrentTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        currentEmote = transform.Find("currentEmote").gameObject;
        EventManager.OnHungerFull += EmoteToForest;
        EventManager.OnFirstBlood += EmoteFirstBlood;
        EventManager.OnSheepEaten += EmoteWolvedDown;
    }

     private void OnDestroy() {
        EventManager.OnHungerFull -= EmoteToForest;
        EventManager.OnFirstBlood -= EmoteFirstBlood;
        EventManager.OnSheepEaten -= EmoteWolvedDown;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (emotingCurrentTimer > 0)
        {
            emotingCurrentTimer -= Time.deltaTime;
        }
        if (emotingCurrentTimer <= 0 && currentEmote.activeInHierarchy == true)
        {
            currentEmote.SetActive(false);
        }
    }

    public void Emote(int emoteID)
    {
        
        currentEmote.GetComponent<SpriteRenderer>().sprite = emotes[emoteID];
        currentEmote.SetActive(true);
        emotingCurrentTimer = emotingTimer;
    }

    void EmoteToForest()
    {
        Emote(2);
    }

    void EmoteFirstBlood()
    {
        Emote(3);
    }

    void EmoteWolvedDown()
    {
        Emote(4);
    }
}
