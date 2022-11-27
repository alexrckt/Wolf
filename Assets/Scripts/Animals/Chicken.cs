using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Chicken : MonoBehaviour, IEatableAnimal
{
    public float animRangeMin = 2f;
    public float animRangeMax = 7f;
    Animator anim;
    LevelManager levelManager;
    // public float stealthCD = 1f;
    public GameObject bloodObj;

    private SheepsClothing sheepsClothing;
    private SoundManager soundManager;

    // public int chickenHungerVal;

    void Start()
    {
        anim = GetComponent<Animator>();
        levelManager = FindObjectOfType <LevelManager>();
        sheepsClothing = FindObjectOfType<WolfController>().GetComponent<SheepsClothing>();
        RandomNum();
        soundManager = FindObjectOfType<SoundManager>();

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
        soundManager.PlayChew();
        sheepsClothing.Stealth(false);
        levelManager.ChickenEaten();
        levelManager.levelData.aliveAnimals[gameObject.name] = false;
        Instantiate(bloodObj, transform.position, Quaternion.identity);
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
