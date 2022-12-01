using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerEmote : MonoBehaviour
{
    public Sprite[] emotes; // 0 = hunters, 1 = wolves, 2 = mysheep
    
    [SerializeField] GameObject currentEmote;
    public float emotingTimer = 2f;
    public float emotingCurrentTimer = 0f;
    
    void Start()
    {
         currentEmote = transform.Find("currentEmote").gameObject;
         EventManager.OnSeenPlayer += SeenPlayerEmote;
         EventManager.OnAgitated += AgitatedEmote;
    }

    void OnDestroy()
    {
        EventManager.OnSeenPlayer -= SeenPlayerEmote;
         EventManager.OnAgitated -= AgitatedEmote;
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

    void SeenPlayerEmote()
    {
     StartCoroutine(SeenPlayerRoutine());
    }
    IEnumerator SeenPlayerRoutine()
    {
      Emote(0);
      yield return new WaitForSeconds(2f);
      Emote(1);
    }
    void AgitatedEmote()
    {
        Emote(2);
    }
}
