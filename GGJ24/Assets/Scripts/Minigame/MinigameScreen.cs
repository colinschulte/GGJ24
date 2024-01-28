using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text minigameText;

    [SerializeField] private Image balloonImage;

    [SerializeField] private Button pumpButton;

    [SerializeField] private List<Sprite> balloonPhases;

    [SerializeField] private Sprite pumpUpSprite;
    [SerializeField] private Sprite pumpDownSprite;

    private readonly int countdownSeconds = 10;

    private int clicks;

    private bool pumpIsUp = true;


    private void Start() //temporary
    {
        StartMinigame();
    }

    // Called by GameManager
    public void StartMinigame()
    {
        StartCoroutine(MinigameText());
    }

    private IEnumerator MinigameText()
    {
        minigameText.text = "Click the Pump to inflate the ballooon as big as you can before the time runs out!";

        yield return new WaitForSeconds(3);

        minigameText.text = "The player with the best score will submit both of their prompts!";

        yield return new WaitForSeconds(3);

        minigameText.text = "Ready,";

        yield return new WaitForSeconds(1);

        minigameText.text = "Set,";

        yield return new WaitForSeconds(1);

        minigameText.text = "GO!";

        pumpButton.interactable = true;

        int countdown = countdownSeconds;

        for (int i = 0; i < countdownSeconds; i++)
        {
            minigameText.text = countdown.ToString();
            countdown -= 1;

            yield return new WaitForSeconds(1);
        }

        pumpButton.interactable = false;

        minigameText.text = "Total clicks: " + clicks;
    }

    public void SelectPump()
    {
        clicks += 1;

        pumpIsUp = !pumpIsUp;

        pumpButton.image.sprite = pumpIsUp ? pumpUpSprite : pumpDownSprite;
    }

    private void Update()
    {
        Debug.Log(clicks);


    }
}