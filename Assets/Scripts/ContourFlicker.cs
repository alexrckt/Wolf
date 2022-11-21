using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContourFlicker : MonoBehaviour
{
    public GameObject contour;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnHungerFull += StartFlicker ;
        
    }

         private void OnDestroy() {
        EventManager.OnHungerFull -= StartFlicker;
    }
    // Update is called once per frame
    void StartFlicker()
    {
       StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
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
