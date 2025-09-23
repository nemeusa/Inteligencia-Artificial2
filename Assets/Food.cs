using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SurvivorChar survivor = other.GetComponent<SurvivorChar>();

        if (survivor != null && survivor.isAlive)
        {
            survivor.SetAgeDirection(survivor.ageDirection * -1);

            survivor.events.Add($"{survivor.survivorName} comió comida y ahora invierte su edad!");

            Destroy(gameObject);
        }
    }
}
