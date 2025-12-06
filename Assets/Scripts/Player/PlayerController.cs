using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCheckLight checkLight;

    [Header("Player Stats")]
    private float health;
    private float healthMax;
    private float energy;

    public Image healthBar;

    private void Update()
    {
        TakeDamageByLight();
    }
    public void InitState(MapData data)
    {
        healthMax = data.healthPlayer;
        health = healthMax;
        energy = data.enrgyPlayer;
    }

    public void TakeDamageByLight()
    {
        if(checkLight.inLight)
        {
            health -= 1;
            healthBar.fillAmount = (float)health / healthMax;
            Debug.Log("Player Health: " + health);
        }
    }
}
