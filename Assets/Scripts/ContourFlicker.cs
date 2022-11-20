using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContourFlicker : MonoBehaviour
{
    public GameObject contour;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Flicker());
    }

    // Update is called once per frame
    void Update()
    {
        
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
