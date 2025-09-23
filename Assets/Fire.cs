using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CampFire campFire = other.GetComponent<CampFire>();

        if (campFire != null && !campFire.isActive)
        {
            campFire.Activate();

            Destroy(gameObject);
        }
    }
}
