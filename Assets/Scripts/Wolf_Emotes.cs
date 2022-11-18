using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf_Emotes : MonoBehaviour
{
    public Sprite[] emotes; // 0 = yum, 1 = gotcha
    
    public GameObject currentEmote;
    public float emotingTimer = 2f;
    public float emotingCurrentTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (emotingCurrentTimer > 0)
        {emotingCurrentTimer -= Time.deltaTime;}
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
}
