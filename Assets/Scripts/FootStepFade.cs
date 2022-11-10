using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepFade : MonoBehaviour
{
    public float lifeTime = 25f;
    SpriteRenderer sr;
    
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(Fade());
    }

    // Update is called once per frame
    void Update()
    {
        
       
    }

    IEnumerator Fade()
    {
        while (true)
        {
            lifeTime -= 0.5f;
            Color tmp = sr.color;
            tmp.a -= 0.02f;
            sr.color = tmp;
            if (lifeTime <= 0)
            {
                WolfController.Footsteps.Remove(this);
                Destroy(gameObject);
            }
                

            yield return new WaitForSeconds(0.5f);
        }
        
    }
}
