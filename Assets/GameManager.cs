using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject survivorPrefab;

    [Header("Config")]
    public int maxSurvivors = 8;
    public int maxCampfires = 4;

    public List<Transform> spawnPoints;
    public List<Transform> freeSpawnPoints;

    // Lista concreta que también implementa IEnumerable<Survivor>
    public List<SurvivorChar> survivors = new List<SurvivorChar>();

    private float gameDuration = 300f; // 5 minutos

    void Start()
    {
        freeSpawnPoints = new List<Transform>(spawnPoints);

        // Lanzamos la coroutine (time-slicing)
        //StartCoroutine(SpawnSurvivorsCoroutine());
        for (int i = 0; i < 3; i++)
        {
            SpawnSurvivor();
        }
    }

    private void Update()
    {
        //StartCoroutine(SpawnSurvivorCoroutine());
    }

    // -------------------------
    // Time-slicing: coroutine que spawnea de a poco
    // -------------------------

    public void SpawnSurvivor()
    {
        if (survivors.Count >= maxSurvivors)
        {
            Debug.Log("Ya hay el máximo de sobrevivientes.");
            return;
        }

        if (freeSpawnPoints.Count == 0)
        {
            Debug.LogWarning("No hay puntos de spawn asignados.");
            return;
        }

        int index = Random.Range(0, freeSpawnPoints.Count);

        // Elegir un spawn random
        Transform spawn = freeSpawnPoints[index];

        // Instanciar
        GameObject survivorObj = Instantiate(survivorPrefab, spawn.position, Quaternion.identity);

        // Nombre random
        SurvivorChar survivor = survivorObj.GetComponent<SurvivorChar>();
        survivor.survivorName = "Survivor " + Random.Range(1, 100);

        // Agregar a la lista
        survivors.Add(survivor);
        freeSpawnPoints.RemoveAt(index);
    }

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

    //IEnumerator SpawnSurvivorDefiinitivo(System.Action<List<int>> callback)
    //{

    //}

    IEnumerable<(string name, int age)> GenerateSurvivorData()
    {
        for (int i = 0; i < maxSurvivors; i++)
        {
            // yield es un generator: la secuencia se crea perezosamente
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
        // Where es del Grupo 1
        return survivors.Where(s => s.isAlive);
    }

    // Devuelve nombres ordenados por edad (Group2: OrderBy, Group1: Where, luego Select)
    public IEnumerable<string> GetNamesOrderedByAge()
    {
        // OrderBy está en Grupo 2; Select en Grupo 1
        return GetAliveSurvivors()
            .OrderBy(s => s.age)   // Grupo 2
            .Select(s => s.survivorName); // Grupo 1
    }

    // Devuelve top 3 más jóvenes como List (uso de Take y ToList -> Grupo1 y Grupo3)
    public List<SurvivorChar> GetYoungestThree()
    {
        return GetAliveSurvivors()
            .OrderBy(s => s.age)    // Grupo 2
            .Take(3)                // Grupo 1: Take
            .ToList();              // Grupo 3: ToList
    }

    // Usa SelectMany para juntar todos los eventos de todos los survivores (Grupo 2: SelectMany)
    public IEnumerable<string> GetAllEvents()
    {
        // SelectMany es Grupo 2
        return survivors.SelectMany(s => s.GetEvents());
    }

    // Aggregate: suma de edades de los vivos
    public int SumAgesAggregate()
    {
        return GetAliveSurvivors()
            .Select(s => s.age)
            .Aggregate(0, (acc, next) => acc + next); // Aggregate explícito
    }

    // Any ejemplo (Grupo 3)
    public bool AnySurvivorAlive()
    {
        return survivors.Any(s => s.isAlive); // Grupo 3
    }

    // -------------------------
    // Ejemplo de uso en EndGame (tipo anónimo + orden y lista final)
    // -------------------------
    public void EndGame()
    {
        // Genero la lista final usando IEnumerable + LINQ
        var finalOrdered = GetAliveSurvivors()
            .OrderByDescending(s => s.age) // Grupo 2
            .Select(s => new { s.survivorName, s.age, Alive = s.isAlive }) // tipo anónimo
            .ToList(); // Grupo 3

        // Mostrar resultado
        foreach (var r in finalOrdered)
        {
            Debug.Log($"Nombre: {r.survivorName}, Edad: {r.age}, Vivo: {r.Alive}");
        }

        // Aggregate
        Debug.Log("Vitalidad total (aggregate): " + SumAgesAggregate());

        // SelectMany demo
        foreach (var ev in GetAllEvents())
            Debug.Log("Evento: " + ev);
    }
}
