using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Homework
{
    public static string MyExtention(this int nmr)
    {
        string nmrText;
        if (nmr % 2 == 0)
        {
            nmrText = "par";
        }
        else
        {
            nmrText = "impar";
        }
        return "El numero " + nmr + " es " + nmrText;
    }  
    public static bool MyExtentionBool(this int nmr)
    {
 
        return nmr % 2 == 0;
    }

    public static GameObject MyExtentionGameObject(this List<GameObject> gameObjects, Vector2 pos)
    {
        var closest = gameObjects[0];
        var dist = Vector2.Distance(pos, closest.transform.position);

        for (int i = 1; i < gameObjects.Count; i++)
        {
            var currentDist = Vector2.Distance(pos, gameObjects[i].transform.position);
            if (currentDist < dist)
            {
                closest = gameObjects[i];
                dist = currentDist;
            }
        }

        return closest;
    }

    //public static IEnumerable<T> nume(this IEnumerable<T> oña, T hola )
    //{
    //    foreach()
    //}
}

