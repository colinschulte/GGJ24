using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject lobbyConnectScreen;
    [SerializeField] private GameObject promptScreen;

    public void StartGame()
    {
        lobbyConnectScreen.SetActive(false);

        promptScreen.SetActive(true);
    }
}
