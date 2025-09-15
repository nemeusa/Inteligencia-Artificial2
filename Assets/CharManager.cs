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
        }
    }

    public 

    public IEnumerable<string> CharNames()
    {
        var namesChar = _characters.Select(nChar => nChar._name);
        var hpChar = _characters.Select(c => c._hp.ToString());
        var datesChar = namesChar + hpChar;
        return namesChar;
    }
}
