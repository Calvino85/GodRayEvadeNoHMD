using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkedVar;

public class GodRayV2Weapon1 : NetworkedBehaviour
{
    public NetworkedVar<int> player;
    private GameObject enemyPlayer;
    public float sightSpeedMultiplier;
    public float shootingBasicSpeed;
    public float bulletSpeed;
    private float shootDeltaTime;
    public NetworkedVar<float> sightDeltaTime;
    private bool lastFrameLookedAt;

    public GameObject bullet1Prefab;

    // Start is called before the first frame update
    void Start()
    {
        if(IsServer)
        {
            sightDeltaTime.Value = 0f;
        }
        shootDeltaTime = 0f;
        lastFrameLookedAt = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            if (sightDeltaTime.Value > 0f && !lastFrameLookedAt)
            {
                lastFrameLookedAt = false;
                sightDeltaTime.Value -= Time.deltaTime;
                if (sightDeltaTime.Value < 0f)
                {
                    sightDeltaTime.Value = 0f;
                }
            }
            lastFrameLookedAt = false;
        }

        if (enemyPlayer != null)
        {
            shootDeltaTime += Time.deltaTime;
            if (shootDeltaTime > shootingBasicSpeed * (1f - sightDeltaTime.Value * sightSpeedMultiplier))
            {
                shootDeltaTime = 0f;
                GameObject bullet1 = Instantiate(bullet1Prefab, transform);
                bullet1.GetComponent<Rigidbody>().velocity = Vector3.Normalize(transform.position - enemyPlayer.transform.position) * -bulletSpeed;
            }
        }
        else
        {
            GodRayV2PlayerValuesManager[] players = GameObject.FindObjectsOfType<GodRayV2PlayerValuesManager>();
            foreach (GodRayV2PlayerValuesManager tPlayer in players)
            {
                if (!tPlayer.IsOwner && player.Value == 1 && IsServer)
                {
                    enemyPlayer = tPlayer.gameObject;
                }
                else if (tPlayer.IsOwner && player.Value == 2 && IsServer)
                {
                    enemyPlayer = tPlayer.gameObject;
                }
                else if (!tPlayer.IsOwner && player.Value == 2 && !IsServer)
                {
                    enemyPlayer = tPlayer.gameObject;
                }
                else if (tPlayer.IsOwner && player.Value == 1 && !IsServer)
                {
                    enemyPlayer = tPlayer.gameObject;
                }
            }
        }
    }

    public void assignPlayer(int pPlayer)
    {
        player.Value = pPlayer;
    }

    public int getPlayer()
    {
        return player.Value;
    }

    public void beingLookedAt()
    {
        sightDeltaTime.Value += Time.deltaTime;
        lastFrameLookedAt = true;
        if(sightDeltaTime.Value > 1)
        {
            sightDeltaTime.Value = 1;
        }
    }
}
