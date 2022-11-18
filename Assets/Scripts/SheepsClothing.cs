using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepsClothing : MonoBehaviour
{
    public bool isWearingClothing = false;
    public GameObject clothes;
    WolfController w_c;
    Wolf_Emotes w_e;
    public float defaultCD = 2f; // testing cd for sheep clothes
    public float currentCD;
    
    void Start()
    {
        w_c = GetComponent<WolfController>();
        w_e = GetComponent<Wolf_Emotes>();
    }

    
    void Update()
    {
        
        if (Input.GetKeyDown("space") && currentCD > 0)
        {
           w_e.Emote(2); // not yet emote if has cd
        }
        
        if (Input.GetKeyDown("space") && currentCD <= 0)
        {
            isWearingClothing = !isWearingClothing;
            if (isWearingClothing && !w_c.isCarryingSheep)
            {
                Stealth(true, defaultCD);
            }
            else
            {
                Stealth(false, defaultCD);
            }

        }
        if (currentCD > 0)
        {
            currentCD -= Time.deltaTime;
        }
    }


    public void Stealth(bool yesno, float CD)
    {

        clothes.SetActive(yesno);
        w_c.IsStealthed(yesno);
        currentCD = CD;
    }
}
