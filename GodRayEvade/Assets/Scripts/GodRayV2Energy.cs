using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class GodRayV2Energy : NetworkedBehaviour
{
    private int player;
    private GameObject serverPlayer;
    private GameObject ownerPlayer;
    public float sightEnergyMultiplier;
    public float energyAddition;
    private float energyDeltaTime;
    private float sightDeltaTime;
    private bool lastFrameLookedAt;

    // Start is called before the first frame update
    void Start()
    {
        energyDeltaTime = 0;
        sightDeltaTime = 0;
        lastFrameLookedAt = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(sightDeltaTime > 0 && !lastFrameLookedAt)
        {
            lastFrameLookedAt = false;
            sightDeltaTime -= Time.deltaTime;
            if(sightDeltaTime < 0)
            {
                sightDeltaTime = 0;
            }
        }
        lastFrameLookedAt = false;
        if (IsServer)
        {
            if (ownerPlayer != null)
            {
                energyDeltaTime += Time.deltaTime;
                if(energyDeltaTime > 0.1f)
                {
                    int energytoAdd = (int)(energyAddition * (1 + sightEnergyMultiplier * sightDeltaTime));
                    ownerPlayer.GetComponent<GodRayV2PlayerValuesManager>().AddEnergy(energytoAdd);
                    energyDeltaTime = 0;
                }
                
            }
            else
            {
                GodRayV2PlayerValuesManager[] players = GameObject.FindObjectsOfType<GodRayV2PlayerValuesManager>();
                foreach (GodRayV2PlayerValuesManager tPlayer in players)
                {
                    if (tPlayer.IsOwner && player == 1)
                    {
                        ownerPlayer = tPlayer.gameObject;
                    }
                    else if (!tPlayer.IsOwner && player == 2)
                    {
                        ownerPlayer = tPlayer.gameObject;
                    }
                }
            }
        }
    }

    public void assignServerPlayer(GameObject pServerPlayer)
    {
        serverPlayer = pServerPlayer;
    }

    public void assignPlayer(int pPlayer)
    {
        player = pPlayer;
    }

    public int getPlayer()
    {
        return player;
    }

    public void beingLookedAt()
    {
        sightDeltaTime += Time.deltaTime;
        lastFrameLookedAt = true;
        if(sightDeltaTime > 1)
        {
            sightDeltaTime = 1;
        }
    }
}
