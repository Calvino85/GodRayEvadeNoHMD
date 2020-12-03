using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkedVar;

public class GodRayV2PlayerValuesManager : NetworkedBehaviour
{
    public int maxHealth;
    public int maxEnergy;
    public int damage;

    public NetworkedVar<int> currentHealth;
    public NetworkedVar<int> currentEnergy;
    public HealthBarPlayer healthBar;
    public HealthBarPlayer energyBar;

    void Start()
    {
        if (IsServer)
        {
            currentHealth.Value = maxHealth;
            currentEnergy.Value = 0;
            healthBar.SetMaxHealth(maxHealth);
            energyBar.SetMaxHealth(maxEnergy);
        }
    }

    public void TakeDamage(int damages)
    {
        currentHealth.Value -= damages;
    }

   public void AddEnergy(int energy)
    {
        currentEnergy.Value += energy;
        if(currentEnergy.Value > maxEnergy)
        {
            currentEnergy.Value = maxEnergy;
        }
    }

    public void RemoveEnergy(int energy)
    {
        currentEnergy.Value -= energy;
        if (currentEnergy.Value < 0)
        {
            currentEnergy.Value = 0;
        }
    }

    public int getEnergy()
    {
        return currentEnergy.Value;
    }

    void Update()
    {
        healthBar.SetHealth(currentHealth.Value);
        energyBar.SetHealth(currentEnergy.Value);
    }
}