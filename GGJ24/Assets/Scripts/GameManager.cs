using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Image titleScreen;
    [SerializeField] public GameObject lobbyConnectScreen;
    [SerializeField] public GameObject promptScreen;

    [SerializeField] RelayManager relay;

    private int fade;

    private void Start()
    {
        StartCoroutine(TitleScreenFade());
    }
    private void Update()
    {
        if (fade != 0)
        {
            Color color = titleScreen.color;
            color.a += fade * Time.deltaTime;
            titleScreen.color = color;
        }
    }
    private IEnumerator TitleScreenFade()
    {
        fade = 1;

        yield return new WaitForSeconds(2);

        fade = 0;
        
        yield return new WaitForSeconds(1);

        fade = -1;

        yield return new WaitForSeconds(2);

        fade = 0;

        titleScreen.gameObject.SetActive(false);
        lobbyConnectScreen.SetActive(true);
    }

    public void ClientStartGame(int p_index)
    {
        // Run on all clients once the host starts the game

        lobbyConnectScreen.SetActive(false);

        promptScreen.SetActive(true);
    }
}
