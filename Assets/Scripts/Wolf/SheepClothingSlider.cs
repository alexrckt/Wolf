using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SheepClothingSlider : MonoBehaviour
{

     Slider slider;
     
    public float dressingTime = 1.5f;
    public bool isDressing = false;
    
    WolfController  wc;
    SheepsClothing sc;
     [SerializeField] Transform thisFillBar;
    //  Quaternion debugRot;
     


    
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = dressingTime;
        slider.value = 0;
        wc = FindObjectOfType<WolfController>();
        sc = wc.GetComponent<SheepsClothing>();
        StartCoroutine(WhereWolfIsLooking());
        // debugRot =  thisFillBar.localRotation;

    }

    private void Update() 
    {
        if (isDressing && slider.value <= slider.maxValue)
        {
            slider.value += Time.deltaTime;
        }
        if (slider.value >= slider.maxValue)
        {
          sc.Stealth(true);
          isDressing = false;
        }
       
        
    }

    public void IsPuttingOnClothes(bool yesno)
    {
        isDressing = yesno;
        if (yesno == false)
        {
            slider.value = 0f;
        }
      
    }

    IEnumerator WhereWolfIsLooking()
    {
        while (true){
        if (wc.horizontal == 1 )
        {
            thisFillBar.localScale = new Vector3(1f, thisFillBar.localScale.y, thisFillBar.localScale.z );
            thisFillBar.localRotation = new Quaternion (thisFillBar.localRotation.x, thisFillBar.localRotation.y, 0, thisFillBar.localRotation.w);
           //right
        }
        else if (wc.vertical == 1 )
        {
           thisFillBar.rotation =  Quaternion.Euler(0, 0, 90f);
           //up 90
        }
        else if (wc.horizontal == -1 )
        {
            thisFillBar.localScale = new Vector3(-1f, thisFillBar.localScale.y, thisFillBar.localScale.z );
            thisFillBar.localRotation = new Quaternion (thisFillBar.localRotation.x, thisFillBar.localRotation.y, 0, thisFillBar.localRotation.w);
            //left
        }
        
        else if (wc.vertical == -1 )
        {
            thisFillBar.rotation =  Quaternion.Euler(0, 0, -90f);
            //down -90
        }
        yield return new WaitForSeconds(0.1f);
        }
    }
}
