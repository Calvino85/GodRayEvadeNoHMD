using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class GodRayV2SightEffect : NetworkedBehaviour
{
    private RaycastHit hit;

    private int layerMaskGold;
    private int layerMaskWeapon1;

    // Start is called before the first frame update
    void Start()
    {
        layerMaskGold = 1 << 11;
        layerMaskWeapon1 = 1 << 12;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsServer)
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layerMaskGold))
            {
                GodRayV2Energy energySource = hit.collider.gameObject.GetComponent<GodRayV2Energy>();
                
                if (energySource.getPlayer() == 1  && IsOwner)
                {
                    energySource.beingLookedAt();
                }
                else if (energySource.getPlayer() == 2 && !IsOwner)
                {
                    energySource.beingLookedAt();
                }
            }
            else if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layerMaskWeapon1))
            {
                GodRayV2Weapon1 weapon1 = hit.collider.gameObject.GetComponent<GodRayV2Weapon1>();

                if (weapon1.getPlayer() == 1 && IsOwner)
                {
                    weapon1.beingLookedAt();
                }
                else if (weapon1.getPlayer() == 2 && !IsOwner)
                {
                    weapon1.beingLookedAt();
                }
            }
        }
        
    }
}
