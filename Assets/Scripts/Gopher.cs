using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gopher : MonoBehaviour
{
    GameManager gm;
    
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
        {
            other.GetComponent<Wolf_Emotes>().Emote(0); // call emotion "anim"
            gm.livesCurrent += 1;
            gm.UpdateLivesText();
            Destroy(gameObject);
        }
    }
}
