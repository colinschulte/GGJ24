using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Image titleScreen;
    [SerializeField] public GameObject lobbyConnectScreen;
    [SerializeField] public GameObject border;
    [SerializeField] public GameObject promptScreen;
    [SerializeField] public GameObject miniGameScreen;
    [SerializeField] public GameObject winnerScreen;
    [SerializeField] public GameObject madlibScreen;

    [SerializeField] RelayManager relay;
    [SerializeField] DisplayPrompt madLibManager;

    

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
        border.SetActive(true);
    }

    public void ClientStartGame(int p_index)
    {
        // Run on all clients once the host starts the game

        lobbyConnectScreen.SetActive(false);

        promptScreen.SetActive(true);

        promptScreen.GetComponent<promptDisplay>().set_Text(PlayerData.prompts[p_index]);
    }

    public void clientStartMiniGame()
    {
        promptScreen.SetActive(false);
        miniGameScreen.SetActive(true);

        miniGameScreen.GetComponent<MinigameScreen>().StartMinigame();
    }

    public void clientShowWinner(int index)
    {
        miniGameScreen.SetActive(false);
        winnerScreen.SetActive(true);

        winnerScreen.GetComponent<WinnerScreen>().showWinner(PlayerData.PlayerUsernames[index]);
    }

    public void toMadlibScreen()
    {
        winnerScreen.SetActive(false);
        madlibScreen.SetActive(true);

        madLibManager.StartMadlibs();
    }
}
