using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPos();
        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;

        Collider2D hit = Physics2D.OverlapPoint(transform.position);
        if (hit != null)
        {
            SurvivorChar survivor = hit.GetComponent<SurvivorChar>();
            if (survivor != null && survivor.isAlive)
            {
                survivor.SetAgeDirection(survivor.ageDirection * -1);
                survivor.events.Add($"{survivor.survivorName} recibió comida arrastrada!");
                Destroy(gameObject);
            }
        }
    }

    void Update()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPos() + offset;
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Mathf.Abs(mainCamera.transform.position.z);
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }
}
