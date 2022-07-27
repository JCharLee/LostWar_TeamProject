using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    [SerializeField] private string startPoint;
    private PlayerInteraction thePlayer;
    private ThirdPersonOrbitCamBasic theCamera;

    public void Start()
    {
        thePlayer = FindObjectOfType<PlayerInteraction>();
        theCamera = FindObjectOfType<ThirdPersonOrbitCamBasic>();
        if (startPoint == thePlayer.currentMapName)
        {
            thePlayer.transform.position = transform.position;
            thePlayer.transform.rotation = Quaternion.identity;
            var camPos = thePlayer.transform.position + (transform.up * 3f) - (transform.forward * 5f);
            theCamera.transform.position = camPos;
        }
    }
}