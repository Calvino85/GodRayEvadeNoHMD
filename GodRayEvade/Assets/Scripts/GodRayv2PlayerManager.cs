using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine.UI;

public class GodRayv2PlayerManager : NetworkedBehaviour
{
    private GameObject cameraWrapper;
    private GameObject camera1Pos;
    private GameObject camera2Pos;
    private GameObject player1Pos;
    private GameObject player2Pos;
    public GameObject spotlightPrefab;
    public Material player1Material;
    public Material player2Material;
    private GameObject player1SpotLight;
    private GameObject player2SpotLight;
    public GameObject energyPrefab;
    public GameObject weapon1Prefab;
    private GodRayV2PlayerValuesManager player1ValuesManager;
    private GodRayV2PlayerValuesManager player2ValuesManager;

    private bool player1HasEnergySource = false;
    private bool player2HasEnergySource = false;

    public string PLAYER_NAME = "ME";
    public string OTHER_NAME = "YOU";

    public int WEAPON1_COST = 1000;

    // Start is called before the first frame update
    void Start()
    {
        cameraWrapper = GameObject.Find("CameraWrapper");
        camera1Pos = GameObject.Find("Camera1Pos");
        camera2Pos = GameObject.Find("Camera2Pos");
        player1Pos = GameObject.Find("Player1Pos");
        player2Pos = GameObject.Find("Player2Pos");

        if (IsOwner)
        {
            if (IsServer)
            {
                transform.position = player1Pos.transform.position;
                cameraWrapper.transform.position = camera1Pos.transform.position;
                cameraWrapper.transform.rotation = camera1Pos.transform.rotation;
            }
            else
            {
                transform.position = player2Pos.transform.position;
                cameraWrapper.transform.position = camera2Pos.transform.position;
                cameraWrapper.transform.rotation = camera2Pos.transform.rotation;
            }
        }

        if (IsServer)
        {
            if (IsOwner)
            {
                GetComponentInChildren<Renderer>().material = player1Material;
                GetComponentInChildren<Text>().text = PLAYER_NAME;
            }
            else
            {
                GetComponentInChildren<Renderer>().material = player2Material;
                GetComponentInChildren<Text>().text = OTHER_NAME;
            }

        }
        else
        {
            if (IsOwner)
            {
                GetComponentInChildren<Renderer>().material = player2Material;
                GetComponentInChildren<Text>().text = OTHER_NAME;
            }
            else
            {
                GetComponentInChildren<Renderer>().material = player1Material;
                GetComponentInChildren<Text>().text = PLAYER_NAME;
            }
        }

        if (IsServer)
        {
            GameObject spotLight = Instantiate(spotlightPrefab, Vector3.zero, Quaternion.identity);
            spotLight.GetComponent<NetworkedObject>().SpawnWithOwnership(GetComponent<NetworkedObject>().OwnerClientId);
            if(IsOwner)
            {
                player1SpotLight = spotLight;
            }
            else
            {
                player2SpotLight = spotLight;
            }
        }
    }

    void Update()
    {
        if(IsServer)
        {
            if(IsOwner)
            {
                if (player1ValuesManager == null)
                {
                    player1ValuesManager = GetComponent<GodRayV2PlayerValuesManager>();
                }
                if (player2ValuesManager == null)
                {
                    GodRayV2PlayerValuesManager[] players = GameObject.FindObjectsOfType<GodRayV2PlayerValuesManager>();
                    foreach (GodRayV2PlayerValuesManager tPlayer in players)
                    {
                        if (!tPlayer.IsOwner)
                        {
                            player2ValuesManager = tPlayer;
                        }
                    }
                }
            }
                
        }    
        if(IsOwner)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (IsServer)
                {
                    CreateObject(1);
                }
                else
                {
                    InvokeServerRpc(CreateObject, 2);
                }
            }
        }
    }

    [ServerRPC]
    private void CreateObject(int player)
    {
        Vector3 position = new Vector3();
        RaycastHit hit;
        int layerMask = 1 << 10;
        bool hitEnergy = false;
        bool hitWeapon1 = false;

        if (player == 1)
        {
            if(!player1HasEnergySource)
            {
                if (Physics.Raycast(player1SpotLight.transform.position, player1SpotLight.transform.forward, out hit, Mathf.Infinity, layerMask))
                {
                    position = hit.point;
                    player1HasEnergySource = true;
                    hitEnergy = true;
                }
            }
            else if(player1ValuesManager.getEnergy() >= 1000)
            {
                if (Physics.Raycast(player1SpotLight.transform.position, player1SpotLight.transform.forward, out hit, Mathf.Infinity, layerMask))
                {
                    position = hit.point;
                    player1ValuesManager.RemoveEnergy(1000);
                    hitWeapon1 = true;
                }
            }
        }
        else if (player == 2)
        {
            if(!player2HasEnergySource)
            {
                if (Physics.Raycast(player2SpotLight.transform.position, player2SpotLight.transform.forward, out hit, Mathf.Infinity, layerMask))
                {
                    position = hit.point;
                    player2HasEnergySource = true;
                    hitEnergy = true;
                }
            }
            else if (player2ValuesManager.getEnergy() >= 1000)
            {
                if (Physics.Raycast(player2SpotLight.transform.position, player2SpotLight.transform.forward, out hit, Mathf.Infinity, layerMask))
                {
                    position = hit.point;
                    player2ValuesManager.RemoveEnergy(1000);
                    hitWeapon1 = true;
                }
            }
        }

        if (hitEnergy)
        {
            GameObject energySource = Instantiate(energyPrefab, position, Quaternion.identity);
            energySource.GetComponent<NetworkedObject>().Spawn();
            energySource.GetComponent<GodRayV2Energy>().assignPlayer(player);
        }
        if (hitWeapon1)
        {
            GameObject weapon1 = Instantiate(weapon1Prefab, position, Quaternion.identity);
            weapon1.GetComponent<NetworkedObject>().Spawn();
            weapon1.GetComponent<GodRayV2Weapon1>().assignPlayer(player);
        }
    }
}
