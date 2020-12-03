using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class GodRayV2Weapon1 : NetworkedBehaviour
{
    private int player;
    private GameObject enemyPlayer;
    public float sightSpeedMultiplier;
    public float basicSpeed;
    private float shootDeltaTime;
    private float sightDeltaTime;
    private bool lastFrameLookedAt;

    public GameObject bullet1Prefab;

    // Start is called before the first frame update
    void Start()
    {
        shootDeltaTime = 0;
        sightDeltaTime = 0;
        lastFrameLookedAt = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            if (sightDeltaTime > 0 && !lastFrameLookedAt)
            {
                lastFrameLookedAt = false;
                sightDeltaTime -= Time.deltaTime;
                if (sightDeltaTime < 0)
                {
                    sightDeltaTime = 0;
                }
            }
            lastFrameLookedAt = false;

            if (enemyPlayer != null)
            {
                shootDeltaTime += Time.deltaTime;
                if(shootDeltaTime > basicSpeed * (1f - sightDeltaTime * sightSpeedMultiplier))
                {
                    shootDeltaTime = 0f;
                    GameObject bullet1 = Instantiate(bullet1Prefab, transform);
                    bullet1.GetComponent<Rigidbody>().velocity = Vector3.Normalize(transform.position - enemyPlayer.transform.position) * -5f;
                }
            }
            else
            {
                GodRayV2PlayerValuesManager[] players = GameObject.FindObjectsOfType<GodRayV2PlayerValuesManager>();
                foreach (GodRayV2PlayerValuesManager tPlayer in players)
                {
                    if (!tPlayer.IsOwner && player == 1)
                    {
                        enemyPlayer = tPlayer.gameObject;
                    }
                    else if (tPlayer.IsOwner && player == 2)
                    {
                        enemyPlayer = tPlayer.gameObject;
                    }
                }
            }
        }
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
