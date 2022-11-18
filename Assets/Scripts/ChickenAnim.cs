using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenAnim : MonoBehaviour
{
    public float animRangeMin = 2f;
    public float animRangeMax = 7f;
    Animator anim;
    GameManager gm;
    public int chickenScoreVal = 50;
    public float stealthCD = 1f;
    public GameObject bloodObj;

   // public int chickenHungerVal;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        gm = FindObjectOfType <GameManager>();
        RandomNum();

    }
     
     IEnumerator PlayIdleChicken(float when)
     {
        yield return new WaitForSeconds(when);
        int randomNum = Random.Range(1, 5);
        anim.SetTrigger("idle" + randomNum);
        RandomNum();
        
     }

     void RandomNum()
     {
        float randomNum = Random.Range(animRangeMin, animRangeMax);
        StartCoroutine(PlayIdleChicken(randomNum));
     }
    


    public void IGotEaten()
    {  
        gm.score += chickenScoreVal;
        Instantiate(bloodObj, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
        

          
            
        
    

    private void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Player")
        {
         
            GrabSheep gs = other.GetComponent<GrabSheep>();
            gs.isTouchingChicken = true;
            gs.cAnim = this;

           
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player")
        {
            other.GetComponent<GrabSheep>().isTouchingChicken = false;
        }
        
    }
}
