using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEatableAnimal
{
    public void IGotEaten() { }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GrabAnimals grabAnimals = other.GetComponent<GrabAnimals>();
            grabAnimals.isTouchingEatable = true;
            grabAnimals.eatableAnimal = this;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<GrabAnimals>().isTouchingEatable = false;
        }
    }
}
