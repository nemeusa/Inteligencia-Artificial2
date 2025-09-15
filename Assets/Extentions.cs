using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extentions : MonoBehaviour
{
    Action<int> verificationNmrMethods;
    bool easy;

    [SerializeField] List<GameObject> objects;
    [SerializeField] GameObject playerPos;
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            easy = !easy;
        }
        if ( Input.GetKeyDown(KeyCode.Space))
        {
            verificationNmrMethods = easy ? IsEven : IsEvenEasy;
            int newNmr = UnityEngine.Random.Range(0, 100);
            verificationNmrMethods(newNmr);
        }
        if (Input.GetKeyDown(KeyCode.A)) Debug.Log(objects.MyExtentionGameObject(playerPos.transform.position) + "esta mas cerca del player");

    }

    void IsEven(int nmr) => Debug.Log(nmr.MyExtention() + " Metodo 1");
    void IsEvenEasy(int nmr) => Debug.Log((nmr.MyExtentionBool() ? $"{nmr} es par " : $"{nmr} es impar ") + "Metodo 2");

}
