using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House_Transparent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void  OnTriggerEnter2D(Collider2D other) 
        
     {
        
        if (other.gameObject.tag == "Player")
        {
            
            Color tmp = GetComponent<SpriteRenderer>().color;
            tmp.a = 0.5f;
            GetComponent<SpriteRenderer>().color = tmp;
        }
    }

    public void OnTriggerExit2D (Collider2D other) {
        if (other.gameObject.tag == "Player")
        {
            Color tmp = GetComponent<SpriteRenderer>().color;
            tmp.a = 1f;
            GetComponent<SpriteRenderer>().color = tmp;
        }
    }
}
