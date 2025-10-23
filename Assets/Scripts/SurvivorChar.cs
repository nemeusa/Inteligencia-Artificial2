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
    public Transform ubication;

    [Header("Velocidad de edad")]
    public int ageDirection; 
    public float ageSpeed = 1f;
    public float baseAgeChangeTime = 0.1f;

    [SerializeField] TMP_Text textName;
    [SerializeField] TMP_Text textAge;
    Color naranja = new Color(1f, 0.39f, 0f);

    // registro eventos para usar SelectMany
    public List<string> events = new List<string>();


    private void Start()
    {
        ageDirection = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;
        age = ageDirection == 1 ? UnityEngine.Random.Range(40, 65) : UnityEngine.Random.Range(15, 60);
        StartCoroutine(AgeRoutine());
        StartCoroutine(TextPulse());
    }

    private void Update()
    {
        textName.text = survivorName + " edad: ";
        textAge.text = $"{age}";
        bool isTall = ageDirection == 1 ? true : false;

        if (isTall) textAge.color = age >= 30 && age < 60 ? Color.yellow : age >= 60 && age < 70 ? naranja : age >= 70 ? Color.red : Color.green;
        else textAge.color = age <= 50 && age > 20 ? Color.yellow : age <= 20 && age > 10 ? naranja : age <= 10 ? Color.red : Color.green;

    }

    private IEnumerator AgeRoutine()
    {
        while (isAlive)
        {
            int fireCount = GameManager.Instance.GetActiveCampfireCount();

            float currentWait = baseAgeChangeTime + (0.2f * fireCount);

            yield return new WaitForSeconds(currentWait); 
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
        if (GameManager.Instance.activeCampfires.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, GameManager.Instance.activeCampfires.Count);
            Transform dirSur = GameManager.Instance.activeCampfires[index].transform;
            GameManager.Instance.activeCampfires[index].Desactivate();
            transform.position += dirSur.position;
        }

        isAlive = false;
        Debug.Log($"{survivorName} ha muerto a los {age} años.");
        Destroy(gameObject, 1);
    }

    // tupla idea
    public (string name, int age) SurvivorData => (survivorName, age);

    // generator que devuelva eventos
    public IEnumerable<string> GetEvents()
    {
        foreach (var e in events)
            yield return e;
    }

    private IEnumerator TextPulse()
    {
        float minSize = 25;
        float maxSize = 27;
        while (true)
        {
            float t = Mathf.PingPong(Time.time, 1f);
            textAge.fontSize = Mathf.Lerp(minSize, maxSize, t);
            yield return null;
        }
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    CampFire campFire = other.GetComponent<CampFire>();

    //    if (campFire != null && campFire.isActive)
    //    {
    //        campFire.Desactivate();

    //        Destroy(gameObject);
    //    }
    //}
}
