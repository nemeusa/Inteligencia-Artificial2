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
    public int ageDirection; // 1 = avanzar, -1 = retroceder
    public float ageSpeed = 1f;  // segundos entre cada cambio de edad

    [SerializeField] TMP_Text text;

    // Registro simple de eventos para poder usar SelectMany
    public List<string> events = new List<string>();


    private void Start()
    {
        ageDirection = Random.Range(0, 2) == 0 ? 1 : -1;
        age = ageDirection == 1 ? Random.Range(1, 25) : Random.Range(60, 79);
        // Arrancamos la corutina que suma la edad
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
            yield return new WaitForSeconds(ageSpeed); // espera 1 segundo
            age += ageDirection; // Puede sumar o restar


            // Revisamos condiciones de muerte
            if (age >= 80 || age <= 0)
            {
                Die();
            }
        }
    }

    public void SetAgeDirection(int dir)
    {
        ageDirection = dir; // Llamado desde comida o fogata
    }

    public void SetAgeSpeed(float newSpeed)
    {
        ageSpeed = newSpeed; // Llamado desde fogata para ralentizar
    }

    private void Die()
    {
        isAlive = false;
        Debug.Log($"{survivorName} ha muerto a los {age} años.");
        // Podés hacer animación de muerte o destruir el objeto
        // Destroy(gameObject);
    }

    // Tupla de acceso rápido (consigna)
    public (string name, int age) SurvivorData => (survivorName, age);

    // Ejemplo de generator que devuelve eventos de este survivor (IEnumerable)
    public IEnumerable<string> GetEvents()
    {
        foreach (var e in events)
            yield return e;
    }
}
