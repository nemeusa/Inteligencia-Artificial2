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
            var allStates = Life();
            foreach (var character in allNames)
            {
              //  Debug.Log(character);
            }
            var stats = StatsChars();
            foreach (var character in stats) Debug.Log(character);
            //foreach (var character in allStates) Debug.Log(character);
                //Debug.Log(allNames);
        }
    }

    public IEnumerable<string> CharNames()
    {
        //Generator lazy: devuelve los nombres uno por uno
        var statsChar = _characters.Where(c => (c._hp/c._maxHP) < 0.1f)
            .Select(c => c._name + c._hp);
        foreach( var nam in statsChar)
            yield return nam;
    }

    public IEnumerable<Health> Life()
    {
        //var ola = statesHP == Health.OK ? 10 : Health.OK;
        var statesHP = _characters.Select(c =>
        {
            var p = c._hp / c._maxHP;
            return p > 0.9f ? Health.OK : p > 0.5f ? Health.Damaged : Health.NearDeath;
        });
        return statesHP;
    }

    public IEnumerable<string> StatsChars()
    {
        var state = Life().Select(c => c.ToString());
        var stats = _characters.Select(c =>
        {
            var p = c._hp / c._maxHP;
            var state = p > 0.9f ? Health.OK : p >= 0.5f ? Health.Damaged : Health.NearDeath;
            return "pj " + c._name.ToString() + " life " + c._hp + " state " + state;
        });
        return stats;
    }
}
