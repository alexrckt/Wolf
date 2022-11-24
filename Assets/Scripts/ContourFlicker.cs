using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContourFlicker : MonoBehaviour
{
    public GameObject contour;
    bool started = false;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnHungerFull += StartFlicker ;
        EventManager.OnGrabSheep += StartFlicker ;
    }

    private void OnDestroy()
    {
        EventManager.OnHungerFull -= StartFlicker;
        EventManager.OnGrabSheep -= StartFlicker ;
    }
    // Update is called once per frame
     void StartFlicker()
    {
       if (!started)
       { 
        started = true;
        StartCoroutine(Flicker());
       }
       
    }

    IEnumerator Flicker()
    {
        contour.SetActive(true);
        while (true)
        {
            if (contour.activeInHierarchy)
            contour.SetActive(false);
            else
            contour.SetActive(true);

            yield return new WaitForSeconds(1f);
        }
    }
}
