using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfEmotes : MonoBehaviour
{
    public Sprite[] emotes; // 0 = yum, 1 = gotcham, 2 = back to forest
    
    GameObject currentEmote;
    public float emotingTimer = 2f;
    public float emotingCurrentTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        currentEmote = transform.Find("currentEmote").gameObject;
        EventManager.OnHungerFull += EmoteToForest;
    }

     private void OnDestroy() {
        EventManager.OnHungerFull -= EmoteToForest;
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
}
