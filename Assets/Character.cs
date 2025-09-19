using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string _name;
    public int _maxHP;
    public int _hp;
    public int yearsOld;
    public float yearsSpeed;
    [SerializeField] Color _color;
    [SerializeField] Vector2 _position;
    [SerializeField] Vector2 _facing;
    //[SerializeField] TextMeshPro textLife;
    [SerializeField] TMP_Text textLife;

    private void Update()
    {
        //yearsOld += (yearsSpeed * Time.deltaTime);
        //textLife.text = _name + ": " + yearsOld + " years old";
    }

    public Character(string name, int maxHP, Color color, Vector2 position, Vector2 facing)
    {
        this._name = name;
        this._maxHP = maxHP;
        _hp = maxHP;
        this._color = color;
        this._position = position;
        this._facing = facing;
    }

}
    public enum Health { Damaged, OK, NearDeath };
   // public enum Faction { Ally, Neutral, Enemy };
   // public enum Decision { Ignore, Follow, Attack }
   public enum Age { Baby, Young, Adult, Old}


