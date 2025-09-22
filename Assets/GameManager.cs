using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text finalReportText; 
    private bool gameEnded = false;

    public GameObject survivorPrefab;

    public int maxSurvivors = 8;
    public int maxCampfires = 4;

    public List<Transform> spawnPoints;
    public List<Transform> freeSpawnPoints;

    public List<SurvivorChar> survivors = new List<SurvivorChar>();

    private float gameDuration = 300f;

    void Start()
    {
        freeSpawnPoints = new List<Transform>(spawnPoints);

        //for (int i = 0; i < 3; i++)
        //{
        //    SpawnSurvivor();
        //}

        if (survivors.Count <= maxSurvivors || freeSpawnPoints.Count == 0) StartCoroutine(SpawnSurvivorDefiinitivo((List<SurvivorChar> finalList) =>
        {
            Debug.Log("Terminó el spawneo. Survivors creados: " + finalList.Count);
            foreach (var s in finalList)
                Debug.Log($"- {s.survivorName} ({s.age})");
        }));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) EndGame2();
    }

    IEnumerator SpawnSurvivorDefiinitivo(System.Action<List<SurvivorChar>> callback)
    {
        List<SurvivorChar> collection = new List<SurvivorChar>();

        int count = maxSurvivors;

        while (count > 0)
        {
            SurvivorChar newSurvivor = SpawnOneSurvivor();
            collection.Add(newSurvivor);

            count--;

            var spawnInterval = Random.Range(5, 10);

            yield return new WaitForSeconds(spawnInterval);
        }

        callback(collection);
    }

    private SurvivorChar SpawnOneSurvivor()
    {
        int index = Random.Range(0, freeSpawnPoints.Count);
        Transform spawn = freeSpawnPoints[index];

        GameObject obj = Instantiate(survivorPrefab, spawn.position, Quaternion.identity);
        SurvivorChar survivor = obj.GetComponent<SurvivorChar>();
        survivor.survivorName = "Survivor " + Random.Range(1, 100);

        if (allNames.Count > 0)
        {
            int nameIndex = Random.Range(0, allNames.Count);
            survivor.survivorName = allNames[nameIndex];
            allNames.RemoveAt(nameIndex);
        }

        survivors.Add(survivor);
        freeSpawnPoints.RemoveAt(index);

        return survivor;
    }

    IEnumerable<(string name, int age)> GenerateSurvivorData()
    {
        for (int i = 0; i < maxSurvivors; i++)
        {
            yield return ($"Survivor_{i + 1}", Random.Range(10, 40));
        }
    }

    public IEnumerable<SurvivorChar> GetAliveSurvivors()
    {
        //grupo 1 linq
        return survivors.Where(s => s.isAlive);
    }

    public IEnumerable<string> GetNamesOrderedByAge()
    {
        //grupo 2 linq
        return GetAliveSurvivors()
            .OrderBy(s => s.age)  
            .Select(s => s.survivorName + "(" + s.age + ")"); 
    }

    public List<SurvivorChar> GetYoungestThree()
    {
        //grupo 3 linq
        return GetAliveSurvivors()
            .OrderBy(s => s.age)    
            .Take(3)                
            .ToList();              
    }

    public IEnumerable<string> GetAllEvents()
    {
        //grupo 2 linq
        return survivors.SelectMany(s => s.GetEvents());
    }

    // aggregate: sumar las edades
    public int SumAgesAggregate()
    {
        return GetAliveSurvivors()
            .Select(s => s.age)
            .Aggregate(0, (acc, next) => acc + next); 
    }

    public bool AnySurvivorAlive()
    {
        // grupo 3 linq
        return survivors.Any(s => s.isAlive); 
    }


    void EndGame2()
    {
        if (gameEnded) return; 
        gameEnded = true;

        var survivorsData = survivors.Select(s => new
        {
            s.survivorName,
            s.age,
            Alive = s.isAlive
        }).ToList();

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("=== RESULTADO FINAL ===");
        sb.AppendLine($"Tiempo de juego: {Time.time:F1} seg");
        sb.AppendLine($"Suma total de edades: {SumAgesAggregate()}");
        sb.AppendLine();
        sb.AppendLine("Sobrevivientes:");
        foreach (var s in survivorsData)
        {
            string status = s.Alive ? "VIVO" : "MUERTO";
            sb.AppendLine($"{s.survivorName} | Edad: {s.age} | {status}");
        }

        sb.AppendLine();
        sb.AppendLine("Ordenados por edad:");
        sb.AppendLine(string.Join(", ", GetNamesOrderedByAge()));

        finalReportText.text = sb.ToString();

        gameEnded = false;
    }

    private List<string> allNames = new List<string>()
    {
        "Aiden","Lucas","Mateo","Benjamin","Isabella","Emma","Sophia","Mia","Amelia","Olivia",
        "Liam","Noah","Ethan","Mason","Logan","James","Oliver","Elijah","Alexander","Jacob",
        "Michael","Daniel","Henry","Sebastian","Jack","Levi","Owen","Wyatt","Julian","Leo",
        "Victoria","Grace","Aria","Scarlett","Chloe","Layla","Zoe","Hannah","Nora","Stella",
        "Aurora","Penelope","Hazel","Ellie","Camila","Luna","Riley","Savannah","Violet","Nova"
    };
}
