using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightManager : NetworkedBehaviour
{
    private GameObject mainCamera;

    private EyeTracker eyeTracker;
    public bool isHMD;

    // Start is called before the first frame update
    void Start()
    {
        if (IsOwner)
        {
            if (isHMD)
            {
                eyeTracker = GameObject.Find("SRanipal").GetComponent<EyeTracker>();
                mainCamera = GameObject.Find("MainCamera");
            }
            else
            {
                mainCamera = GameObject.Find("CameraWrapper");
            }
            transform.position = mainCamera.transform.position;
        }

        if (IsServer)
        {
            if (IsOwner)
                GetComponentInChildren<Light>().color = new Color(1f, 0f, 0f);
            else
                GetComponentInChildren<Light>().color = new Color(0f, 1f, 0f);
        }
        else
        {
            if (IsOwner)
                GetComponentInChildren<Light>().color = new Color(0f, 1f, 0f);
            else
                GetComponentInChildren<Light>().color = new Color(1f, 0f, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            if(isHMD)
            {
                Ray rightEyeRay = eyeTracker.GetRay(EyeTracker.Source.Right);
                Ray leftEyeRay = eyeTracker.GetRay(EyeTracker.Source.Left);
                Ray combinedEyeRay = eyeTracker.GetRay(EyeTracker.Source.Combined);
                Vector3 lookAtPosition = mainCamera.transform.position + Vector3.Slerp(rightEyeRay.direction, leftEyeRay.direction, 0.5f) * 1.5f;
                transform.position = mainCamera.transform.position;
                transform.LookAt(lookAtPosition);
            }
            else
            {
                transform.rotation = mainCamera.transform.rotation;
            }
        }

    }
}