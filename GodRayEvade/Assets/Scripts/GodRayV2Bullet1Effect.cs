using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class GodRayV2Bullet1Effect : NetworkedBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            if(IsServer)
            {
                other.GetComponent<GodRayV2PlayerValuesManager>().TakeDamage(damage);
            }
            Destroy(this.gameObject);
        }
    }
}
