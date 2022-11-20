using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenAnim : MonoBehaviour, IEatableAnimal
{
    public float animRangeMin = 2f;
    public float animRangeMax = 7f;
    Animator anim;
    LevelManager levelManager;
    // public float stealthCD = 1f;
    public GameObject bloodObj;

    private SheepsClothing sheepsClothing;

    // public int chickenHungerVal;

    void Start()
    {
        anim = GetComponent<Animator>();
        levelManager = FindObjectOfType <LevelManager>();
        sheepsClothing = FindObjectOfType<WolfController>().GetComponent<SheepsClothing>();
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
        sheepsClothing.Stealth(false);
        levelManager.ChickenEaten();
        var blood = Instantiate(bloodObj, transform.position, Quaternion.identity);
        blood.GetComponent<BloodStain>().BloodSplatter();
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        (this as IEatableAnimal).OnTriggerStay2D(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        (this as IEatableAnimal).OnTriggerExit2D(other);
    }
}
