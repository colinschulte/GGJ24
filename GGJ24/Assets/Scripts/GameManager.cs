using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Image titleScreen;
    [SerializeField] private GameObject lobbyConnectScreen;
    [SerializeField] private GameObject promptScreen;

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


    public void SelectStartGame()
    {
        lobbyConnectScreen.SetActive(false);

        promptScreen.SetActive(true);
    }
}
