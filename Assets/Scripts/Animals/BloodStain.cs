using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodStain : MonoBehaviour
{
    public Sprite  [] bloodSprites;
    SpriteRenderer sr;
    public float lifeTime = 15f;

    void Start()
    {
        
    }

    public void BloodSplatter()
    {
        sr = GetComponent<SpriteRenderer>();
        int randomNum = Random.Range(0, bloodSprites.Length);
        sr.sprite = bloodSprites[randomNum];
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
