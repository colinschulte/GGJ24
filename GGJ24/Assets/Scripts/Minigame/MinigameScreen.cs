using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text minigameText;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private TMP_Text clicksText;

    [SerializeField] private Image balloonImage;

    [SerializeField] private Button pumpButton;

    [SerializeField] private List<Sprite> balloonPhases;

    [SerializeField] private Sprite pumpUpSprite;
    [SerializeField] private Sprite pumpDownSprite;

    private readonly int countdownSeconds = 10;

    private int clicks;

    private bool pumpIsUp = true;

    [SerializeField] private RelayManager relay;

    // Called by GameManager
    public void StartMinigame()
    {
        StartCoroutine(MinigameText());
    }

    private IEnumerator MinigameText()
    {
        clicksText.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(false);

        minigameText.text = "Click the Pump to inflate the ballooon as big as you can before the time runs out!";

        yield return new WaitForSeconds(3);

        minigameText.text = "The player with the best score will submit both of their prompts!";

        yield return new WaitForSeconds(3);

        minigameText.text = "Ready,";

        yield return new WaitForSeconds(1);

        minigameText.text = "Set,";

        yield return new WaitForSeconds(1);

        minigameText.text = "GO!";

        clicksText.gameObject.SetActive(true);
        countdownText.gameObject.SetActive(true);

        pumpButton.interactable = true;

        yield return new WaitForSeconds(1);

        int countdown = countdownSeconds;

        for (int i = 0; i < countdownSeconds; i++)
        {
            countdownText.text = countdown.ToString();
            countdown -= 1;

            yield return new WaitForSeconds(1);
        }

        pumpButton.interactable = false;

        countdownText.gameObject.SetActive(false);
        clicksText.gameObject.SetActive(false);

        minigameText.text = "Total clicks: " + clicks;

        Debug.Log("sent grade");

        yield return new WaitForSeconds(2);

        relay.sendGradeToServerRpc(PlayerData.PlayerNumber, clicks);

        
    }

    public void SelectPump()
    {
        clicks += 1;

        pumpIsUp = !pumpIsUp;

        pumpButton.image.sprite = pumpIsUp ? pumpUpSprite : pumpDownSprite;
    }

    private void Update()
    {
        int phase = Mathf.FloorToInt(clicks / 11);
        balloonImage.sprite = balloonPhases[phase];

        if (clicksText.gameObject.activeSelf)
            clicksText.text = "Pumps: " + clicks;
    }
}