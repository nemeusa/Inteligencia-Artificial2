using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public bool isActive;
    SpriteRenderer sr;
    Color ogColor;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ogColor = sr.color;
        isActive = true;
    }

    public void Activate()
    {
        if (!isActive)
        {
            sr.color = ogColor;
            GameManager.Instance.activeCampfires.Add(this);
            isActive = true;
        }
    }

    public void Desactivate()
    {
        if (isActive)
        {
            Debug.Log($"fogata {name} desactivada");
            sr.color = new Color(0.6f, 0.3f, 0.1f);
            GameManager.Instance.activeCampfires.Remove(this);
            isActive = false;
        }
    }
}
