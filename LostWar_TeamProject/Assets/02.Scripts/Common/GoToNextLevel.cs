using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToNextLevel : MonoBehaviour
{
    [SerializeField] private string transferMapName;

    private PlayerInteraction thePlayer;

    private void Start()
    {
        thePlayer = FindObjectOfType<PlayerInteraction>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            thePlayer.currentMapName = transferMapName;
            SceneLoader.LoadScene(transferMapName);
        }
    }
}
