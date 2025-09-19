//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public class Survivor
//{
//    public string Name { get; private set; }
//    public int Age { get; set; }
//    public bool IsAlive { get; set; }

//    public Survivor(string name, int age)
//    {
//        Name = name;
//        Age = age;
//        IsAlive = true;
//    }
//}

//public class GameManager : MonoBehaviour
//{
//    public List<Survivor> survivors = new List<Survivor>();
//    public int maxSurvivors = 8;
//    public int maxCampfires = 4;
//    private float timer;
//    private float gameDuration = 300f; // 5 minutos

//    void Start()
//    {
//        // Spawn inicial de 2 sobrevivientes
//        survivors.Add(new Survivor("Juan", 20));
//        survivors.Add(new Survivor("Maria", 30));
//    }

//    void Update()
//    {
//        timer += Time.deltaTime;

//        if (timer >= 1f) // Cada segundo
//        {
//            timer = 0f;
//            UpdateAges();
//        }

//        if (Time.time >= gameDuration)
//        {
//            EndGame();
//        }
//    }

//    void UpdateAges()
//    {
//        foreach (var s in survivors.Where(s => s.IsAlive)) // LINQ Grupo 1
//        {
//            s.Age++;
//            if (s.Age <= 0 || s.Age >= 80)
//                s.IsAlive = false;
//        }
//    }

//    void EndGame()
//    {
//        // Mostrar resultado final
//        foreach (var s in survivors.OrderBy(x => x.Age)) // LINQ Grupo 2
//        {
//            Debug.Log($"Sobreviviente: {s.Name}, Edad: {s.Age}, Vivo: {s.IsAlive}");
//        }

//        // Aggregate: combinar datos de manera significativa
//        var vitalidad = survivors
//            .Where(s => s.IsAlive)
//            .Select(s => s.Age)
//            .Aggregate(0, (acc, next) => acc + next); // suma total de edades
//        Debug.Log($"Vitalidad total de la aldea: {vitalidad}");

//        // LINQ Grupo 3
//        bool hayAlguienVivo = survivors.Any(s => s.IsAlive);
//        Debug.Log($"¿Quedó alguien vivo? {hayAlguienVivo}");

//        // Ejemplo Tupla
//        (string nombre, int edad) datos = ("Carlos", 25);
//        Debug.Log($"Tupla: {datos.nombre}, Edad: {datos.edad}");

//        // Ejemplo Tipo Anónimo
//        var accion = new { Survivor = "Juan", Evento = "Fogata", Efecto = "Edad más lenta" };
//        Debug.Log($"Tipo anónimo -> {accion.Survivor} hizo: {accion.Evento}, efecto: {accion.Efecto}");
//    }
//}