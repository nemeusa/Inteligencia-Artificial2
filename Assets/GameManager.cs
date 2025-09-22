using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
    }

    
    //public void SpawnSurvivor()
    //{
    //    if (survivors.Count >= maxSurvivors)
    //    {
    //        Debug.Log("hay muchos sobrevivientes");
    //        return;
    //    }

    //    if (freeSpawnPoints.Count == 0)
    //    {
    //        Debug.LogWarning("no hay mas spawns");
    //        return;
    //    }

    //    int index = Random.Range(0, freeSpawnPoints.Count);

    //    Transform spawn = freeSpawnPoints[index];

        
    //    GameObject survivorObj = Instantiate(survivorPrefab, spawn.position, Quaternion.identity);

    //    SurvivorChar survivor = survivorObj.GetComponent<SurvivorChar>();
    //    survivor.survivorName = "Survivor " + Random.Range(1, 100);

    //    survivors.Add(survivor);
    //    freeSpawnPoints.RemoveAt(index);
    //}

    //IEnumerator SpawnSurvivorCoroutine(System.Action<List<int>> callback)
    //{
    //    List<SurvivorChar> collection = new List<SurvivorChar>();

    //    while (count > 0)

    //    {

    //        count;

    //        collection.Add(spawnMethod());

    //        yield return new WaitForSeconds(time);

    //    }

    //    callback(collection);
    //}

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
        Transform spawn = spawnPoints[index];

        GameObject obj = Instantiate(survivorPrefab, spawn.position, Quaternion.identity);
        SurvivorChar survivor = obj.GetComponent<SurvivorChar>();
        survivor.survivorName = "Survivor " + Random.Range(1, 100);

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

    Vector3 GetRandomPosition()
    {
        Vector2 r = Random.insideUnitCircle * 2f;
        return new Vector3(r.x, r.y, 0f) + transform.position;
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
            .Select(s => s.survivorName); 
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

  
    public void EndGame()
    {
        var finalOrdered = GetAliveSurvivors()
            .OrderByDescending(s => s.age) 
            .Select(s => new { s.survivorName, s.age, Alive = s.isAlive }) // anonimous type
            .ToList(); 

        // mostrar resultado
        foreach (var r in finalOrdered)
        {
            Debug.Log($"Nombre: {r.survivorName}, Edad: {r.age}, Vivo: {r.Alive}");
        }

        // aggregate
        Debug.Log("Vitalidad total (aggregate): " + SumAgesAggregate());

        // selectMany demo
        foreach (var ev in GetAllEvents())
            Debug.Log("Evento: " + ev);
    }
}
