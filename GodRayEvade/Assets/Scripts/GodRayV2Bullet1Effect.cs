using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class GodRayV2Bullet1Effect : NetworkedBehaviour
{
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
