using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class GodRayV2SightEffect : NetworkedBehaviour
{
    private RaycastHit hit;

    private int layerMaskGold;

    // Start is called before the first frame update
    void Start()
    {
        layerMaskGold = 1 << 11;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsServer)
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layerMaskGold))
            {
                GodRayV2Energy gold = hit.collider.gameObject.GetComponent<GodRayV2Energy>();
                
                if (gold.getPlayer() == 1  && IsOwner)
                {
                    gold.beingLookedAt();
                }
                else if (gold.getPlayer() == 2 && !IsOwner)
                {
                    gold.beingLookedAt();
                }
            }
        }
        
    }
}
