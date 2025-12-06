using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
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
    public float energyMax;
    public float Energy { get { return energy; } set { energy = value; } }

    public Image healthBar;
    public Image energyBar;
    public bool isPause;

    public void SetPause(bool pause)
    {
        isPause = pause;
        playerMovement.SetMove(pause);
    }

    private void Update()
    {
        if(isPause) return;

        TakeDamageByLight();
    }
    public void InitState(MapData data)
    {
        healthMax = data.healthPlayer;
        health = healthMax;
        energy = data.enrgyPlayer;
        energyMax = energy;
        healthBar.fillAmount = 1f;
        SetPause(false);
        energyBar.fillAmount = 1f;
    }
    public void SetEnergyBar()
    {
        energyBar.fillAmount = energy / energyMax;
    }

    public void TakeDamageByLight()
    {
        if(checkLight.inLight)
        {
            health -= 1;
            healthBar.fillAmount = (float)health / healthMax;
            
            if(health <= 0)
            {
                GameManagerInMap.Instance.LoseGame();
            }
        }
    }

    public void SetLight(GameObject map)
    {
        checkLight.SetLight2D(map);
    }
}
