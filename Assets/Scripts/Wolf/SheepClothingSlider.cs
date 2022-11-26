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
     Image fillbarImage;
    //  Quaternion debugRot;
     


    
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = dressingTime;
        slider.value = 0;
        wc = FindObjectOfType<WolfController>();
        sc = wc.GetComponent<SheepsClothing>();
        fillbarImage = thisFillBar.GetComponent<Image>();
        StartCoroutine(WhereWolfIsLooking());
        // debugRot =  thisFillBar.localRotation;

    }

    private void Update() 
    {
        if (isDressing && slider.value <= slider.maxValue)
        {
            slider.value += Time.deltaTime;
             Color tmp = fillbarImage.color;
            tmp.a += 0.5f * Time.deltaTime;

            thisFillBar.GetComponent<Image>().color = tmp;
            
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
        if (!yesno)
        {
            slider.value = 0f;
            Color tmp = fillbarImage.color;
            tmp.a = 0.7f;

            fillbarImage.color = tmp;
        }
        if (yesno)
        {
            Color tmp = fillbarImage.color;
            tmp.a = 0.1f;

            fillbarImage.color = tmp;
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
           thisFillBar.localScale = new Vector3(1f, thisFillBar.localScale.y, thisFillBar.localScale.z );
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
            thisFillBar.localScale = new Vector3(1f, thisFillBar.localScale.y, thisFillBar.localScale.z );
            //down -90
        }
        yield return new WaitForSeconds(0.1f);
        }
    }
}
