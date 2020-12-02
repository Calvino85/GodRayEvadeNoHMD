using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class GodRayV2Gold : NetworkedBehaviour
{
    private int player;
    private GameObject serverPlayer;
    private GameObject ownerPlayer;
    public int energyMultiplier;
    public int energyAddition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            if (ownerPlayer != null)
            {
                ownerPlayer.GetComponent<GodRayV2PlayerValuesManager>().AddEnergy(energyAddition * energyMultiplier);
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
}
