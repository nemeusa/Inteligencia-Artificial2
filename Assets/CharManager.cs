using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharManager : MonoBehaviour
{
    [SerializeField] Character[] _characters;

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            var allNames = CharNames();
            foreach (var character in allNames)
            {
                Debug.Log(character);
            }
                //Debug.Log(allNames);
        }
    }

    public IEnumerable<string> CharNames()
    {
        var namesChar = _characters.Select(nChar => nChar._name);
        var hpChar = _characters.Select(c => c._hp.ToString());
        //Generator lazy: devuelve los nombres uno por uno
        foreach( var nam in namesChar)
            yield return nam;
    }

    //public IEnumerable<Health> Life()
    //{
    //    //var statesHP = _characters.Select(c => c._hp = c._maxHP);
    //    //var ola = statesHP == Health.OK ? 10 : Health.OK;
    //    //return statesHP < 10;
    //}
}
