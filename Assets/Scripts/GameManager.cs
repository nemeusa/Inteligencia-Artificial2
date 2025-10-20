using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private TMP_Text finalReportText; 
    [SerializeField] private TMP_Text finalReportTextEvents; 
    private bool gameEnded = false;

    public GameObject survivorPrefab;

    public int maxSurvivors = 8;
    public int maxCampfires = 4;

    public List<Transform> spawnPoints;
    public List<Transform> freeSpawnPoints;

    public List<SurvivorChar> survivors = new List<SurvivorChar>();

    private float gameDuration = 300f;

    [Header("Comida")]
    [SerializeField] GameObject foodPrefab, campFirePrefab;
    [SerializeField] Transform foodSpawnPoint, campFireSpawnPoint;
    [SerializeField] Button spawnFoodButton;
    [SerializeField] Button spawnCampFireButton;

    [Header("Clicker")]
    public int clicksNeededFood = 3;
    public int clicksNeededCampFire = 3;
    private int currentClicksFood, currentClicksCampFire;

    [SerializeField] TMP_Text clicksTextFood;
    [SerializeField] TMP_Text clicksTextFire;

    GameObject currentFood, currentCampFire;

    public List<CampFire> activeCampfires = new List<CampFire>();

    public float multiplicatorTime;

    [SerializeField] AudioSource _myAudioSource;

    [SerializeField] AudioClip _babySong, _oldSong;

    [SerializeField] TMP_Text avisoSounds;

    void Start()
    {
        Instance = this;
        freeSpawnPoints = new List<Transform>(spawnPoints);

        if (survivors.Count <= maxSurvivors || freeSpawnPoints.Count == 0) StartCoroutine(SpawnSurvivorDefiinitivo((List<SurvivorChar> finalList) =>
        {
            Debug.Log("Terminó el spawneo. Survivors creados: " + finalList.Count);
            foreach (var s in finalList)
                Debug.Log($"- {s.survivorName} ({s.age})");
        }));

        if (spawnFoodButton != null)
            spawnFoodButton.onClick.AddListener(OnClickSpawnFood);

        if (spawnCampFireButton != null)
            spawnCampFireButton.onClick.AddListener(OnClickSpawnCampFire);
    }

    private void Update()
    {
        clicksTextFood.text = $"{clicksNeededFood - currentClicksFood}";
        clicksTextFire.text = $"{clicksNeededCampFire - currentClicksCampFire}";
        //if (!gameEnded) EndGame2();

        if (currentFood == null) spawnFoodButton.gameObject.SetActive(true);
        if (currentCampFire == null) spawnCampFireButton.gameObject.SetActive(true);

        if(Input.GetKeyUp(KeyCode.Mouse0) && currentFood != null) currentFood.transform.position = foodSpawnPoint.position;
        if(Input.GetKeyUp(KeyCode.Mouse0) && currentCampFire != null) currentCampFire.transform.position = campFireSpawnPoint.position;

        probando();
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

    public IEnumerable<string> GetNamesOrderedByAgeGenerator()
    {
        var ordered = survivors
            .Where(s => s.isAlive)          // Grupo 1
            .OrderBy(s => s.age)                // Grupo 2
            .Select(s => s.survivorName)       // Grupo 1
            .ToList();                       // Grupo 3

        foreach (var name in ordered)
            yield return name;          // generator
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
        var col = GetAliveSurvivors()
            .OrderBy(s => s.age)
            .Select(s => s.survivorName + "(" + s.age + ")");

        if (col.Any())
        {
            yield return null;
        }

        foreach (var item in col)
        {
            yield return item;

        }
        //grupo 2 linq
    }

    public List<SurvivorChar> GetYoungestThree()
    {
        //usa los 3 grupos de linq
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

    public (bool baby,int joven, int adulto, AudioClip anciano) CountSurvivorsByAgeRangeTuple() //Tupla
    {
        //trabajar aca el cambio de sprites segun la edad (no fue una buenas idea xd)
        //aggregate: ver edad
        //tmb puedo hacer que mientras mas jovenes y adultos haya mayor va a ser el rendimiento de la aldea o la velocidad con la que se crean las cosas
        //spawnear una mamadera o pastillas para los bebes y los ancianos
        // que suene un sonido de bebe o de viejo cuando se conviertan en eso
        return GetAliveSurvivors()
            .Aggregate((baby: false, joven: 0, adulto: 0, anciano: _oldSong), (acc, s) =>
            {
                //int babyCount = 0;
                //if (s.age <= 20)
                //    acc.baby = _babySong[babyCount <= _babySong.Length ? babyCount++ : babyCount = 0];
                if (s.age <= 20)
                    acc.baby = true;

                else if (s.age > 50)
                    acc.anciano = _oldSong;

                 if (s.age <= 30 && s.age > 20)
                    acc.joven++;

                else if (s.age <= 50 && s.age > 30)
                    acc.adulto++;
                
                return acc;
            });
    }

    void probando()
    {
        var counts = CountSurvivorsByAgeRangeTuple();

        multiplicatorTime = counts.joven + counts.adulto;

        StartCoroutine(ruido());
    }

    IEnumerator ruido()
    {
        var counts = CountSurvivorsByAgeRangeTuple();

        yield return new WaitForSeconds(3);

        if (counts.baby)
        {
            //int babyCount = 0;
            //_myAudioSource.PlayOneShot(_babySong[babyCount <= _babySong.Length ? babyCount++ : babyCount = 0]);
            _myAudioSource.PlayOneShot(_babySong);
            avisoSounds.text = "ruido de bebe llorando: " + _babySong;
        }
        //AudioClip clip = counts.baby;
        //yield return new WaitForSeconds(3);
        //Debug.Log("me veo");
        //if (clip != null)
        //{
        //    Debug.Log("no me veo");
        //    _myAudioSource.PlayOneShot(clip);
        //    avisoSounds.text = "ruido de bebe llorando: " + clip;

        //}
        //yield return new WaitForSeconds(1);
        //if (counts.anciano != null)
        //{
        //    _myAudioSource.PlayOneShot(counts.anciano);
        //    avisoSounds.text = "ruido de viejo full molesto: " + counts.anciano;
        //}
    }

    public void EndGame2()
    {
        if (gameEnded) return; 

        var survivorsData = survivors.Select(s => new
        {
            s.survivorName,
            s.age,
            Alive = s.isAlive
        }).ToList();

        var counts = CountSurvivorsByAgeRangeTuple();



        StringBuilder sb = new StringBuilder();
        StringBuilder sbEvents = new StringBuilder();
        sb.AppendLine("=== RESULTADO FINAL ===");
        //sb.AppendLine($"Joven: {counts.joven}, Adulto: {counts.adulto}, Mayor: {counts.mayor}");
        var youngest = GetYoungestThree();
        foreach (var s in youngest) sb.AppendLine($"Uno de los más jóvenes: {s.survivorName} ({s.age})");
        foreach (var s in survivors)
        {
            var data = s.SurvivorData;   // tupla
            string status = s.isAlive ? "VIVO" : "MUERTO";
            sb.AppendLine($"{data.name} | Edad: {data.age} | {status}");
        }
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

        sbEvents.AppendLine("=== EVENTOS ===");
        foreach (var ev in GetAllEvents()) sbEvents.AppendLine(ev);

            finalReportText.text = sb.ToString();
            finalReportTextEvents.text = sbEvents.ToString();

        gameEnded = true;
    }

    void OnClickSpawnFood()
    {
        currentClicksFood++;

        if (currentClicksFood >= clicksNeededFood)
        {
            SpawnFood();
            currentClicksFood = 0;
            clicksNeededFood++;
        }
    }
    void OnClickSpawnCampFire()
    {
        currentClicksCampFire++;

        if (currentClicksCampFire >= clicksNeededCampFire)
        {
            SpawnCampFire();
            currentClicksCampFire = 0;
            clicksNeededCampFire++;
        }
    }

    void SpawnFood()
    {
        if ( foodPrefab == null) return;

        currentFood = Instantiate(foodPrefab, foodSpawnPoint.position, Quaternion.identity);
        Debug.Log("Comida spawneada");
        if (currentFood != null) spawnFoodButton.gameObject.SetActive(false);
    }

    void SpawnCampFire()
    {
        if (campFirePrefab == null) return;

        currentCampFire = Instantiate(campFirePrefab, campFireSpawnPoint.position, Quaternion.identity);
        Debug.Log("Fogata spawneada");
        if (currentCampFire != null) spawnCampFireButton.gameObject.SetActive(false);
    }

    public int GetActiveCampfireCount()
    {
        return activeCampfires.Count;
    }

    private List<string> allNames = new List<string>()
    {
        "Aiden","Lucas","Mateo","Benjamin","Isabella","Emma","Sophia","Mia","Amelia","Olivia",
        "Liam","Noah","Ethan","Mason","Logan","James","Oliver","Elijah","Alexander","Jacob",
        "Michael","Daniel","Henry", "feli", "breck","Sebastian","Jack","Levi","Owen","Wyatt","Julian","Leo",
        "Victoria","Grace","Aria","Scarlett","Chloe","Layla","Denise","Hannah","Nora","Stella",
        "Aurora","Penelope","Hazel","Ellie","Camila","Luna","Riley","Savannah","Violet","Nova"
    };
}
