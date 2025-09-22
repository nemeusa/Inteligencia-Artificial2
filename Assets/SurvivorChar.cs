using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SurvivorChar : MonoBehaviour
{
    [Header("Datos del sobreviviente")]
    public string survivorName = "oñan";
    public int age;
    public bool isAlive = true;

    [Header("Velocidad de edad")]
    public int ageDirection; 
    public float ageSpeed = 1f;

    [SerializeField] TMP_Text text;

    // registro eventos para usar SelectMany
    public List<string> events = new List<string>();


    private void Start()
    {
        ageDirection = Random.Range(0, 2) == 0 ? 1 : -1;
        age = ageDirection == 1 ? Random.Range(1, 25) : Random.Range(60, 79);
        StartCoroutine(AgeRoutine());
    }

    private void Update()
    {
        text.text = survivorName + " edad: " + age;
    }

    private IEnumerator AgeRoutine()
    {
        while (isAlive)
        {
            yield return new WaitForSeconds(ageSpeed); 
            age += ageDirection; 

            if (age >= 80 || age <= 0)
            {
                Die();
            }
        }
    }

    public void SetAgeDirection(int dir)
    {
        ageDirection = dir; 
    }

    public void SetAgeSpeed(float newSpeed)
    {
        ageSpeed = newSpeed;
    }

    private void Die()
    {
        isAlive = false;
        Debug.Log($"{survivorName} ha muerto a los {age} años.");
        // Destroy(gameObject);
    }

    // tupla idea
    public (string name, int age) SurvivorData => (survivorName, age);

    // generator que devuelva eventos
    public IEnumerable<string> GetEvents()
    {
        foreach (var e in events)
            yield return e;
    }
}
