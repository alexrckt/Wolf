using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public float lifeTime = 2f;
    SpriteRenderer sr;
    bool isFadingDown = false;
    void Start()
    {
         sr = GetComponent<SpriteRenderer>();
        StartCoroutine(Fade());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
         if (isFadingDown)
            {
            Color tmp = sr.color;
            tmp.a -= 1f * Time.deltaTime;
            sr.color = tmp;
            }
            if (!isFadingDown)
            {
            Color tmp = sr.color;
            tmp.a += 1f * Time.deltaTime;
            sr.color = tmp;  
            }
    }

    IEnumerator Fade()
    {
        while (true)
        {
            switch (isFadingDown)
            {
                case true: isFadingDown = false;
                break;
                case false: isFadingDown = true;
                break;
            }
            

            lifeTime -= 0.5f;
            
            if (lifeTime <= 0)
            {
               
                Destroy(gameObject);
            }
             yield return new WaitForSeconds(0.5f);
        }
       
    }
}
