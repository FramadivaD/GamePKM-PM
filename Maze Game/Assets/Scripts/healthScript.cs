using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    public int health;

    public Image[] hearts;

    void Start()
    {
        
    }

    void Update()
    {
        for(int i = 0; i < hearts.Length; i++){
            if(i < health){
                hearts[i].color = new Color(1f, 1f, 1f, 1f);
            }else hearts[i].color = new Color(1f, 1f, 1f, 0.2353f);
        }
    }
}
