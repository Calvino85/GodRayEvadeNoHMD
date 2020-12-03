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
    public GameObject goldPrefab;

    private bool player1HasGold = false;
    private bool player2HasGold = false;

    private string PLAYER_NAME = "ME";
    private string OTHER_NAME = "THE OTHER";

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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (IsServer)
            {
                CreateGold(1);
            }
            else
            {
                InvokeServerRpc(CreateGold, 2);
            }
        }
    }

    [ServerRPC]
    private void CreateGold(int player)
    {
        Vector3 position = new Vector3();
        RaycastHit hit;
        int layerMask = 1 << 10;
        bool hitFound = false;
        if (player == 1 && !player1HasGold)
        {
            if (Physics.Raycast(player1SpotLight.transform.position, player1SpotLight.transform.forward, out hit, Mathf.Infinity, layerMask))
            {
                position = hit.point;
                player1HasGold = true;
                hitFound = true;
            }
            
        }
        else if (player == 2 && !player2HasGold)
        {
            if (Physics.Raycast(player2SpotLight.transform.position, player2SpotLight.transform.forward, out hit, Mathf.Infinity, layerMask))
            {
                position = hit.point;
                player2HasGold = true;
                hitFound = true;
            }
        }

        if (hitFound)
        {
            GameObject goldObject = Instantiate(goldPrefab, position, Quaternion.identity);
            goldObject.GetComponent<NetworkedObject>().Spawn();
            goldObject.GetComponent<GodRayV2Energy>().assignServerPlayer(this.gameObject);
            goldObject.GetComponent<GodRayV2Energy>().assignPlayer(player);
        }
    }
}
