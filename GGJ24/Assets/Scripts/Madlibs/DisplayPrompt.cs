using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class DisplayPrompt : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private List<Button> buttons;
    [SerializeField] private List<GameObject> resultPanels;
    [SerializeField] private RelayManager relayManager;
    [SerializeField] private GameObject resultsPanel;
    [SerializeField] private List<Color32> playerColors;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip resultSFX;

    private string[] promptList;
    private Dictionary<int, int> scoreList;
    private Dictionary<int, int> rankList;
    private Dictionary<string, int> inputList;
    private int promptEnum = 0;
    private int inputEnum = 0;
    //private int votesLeft = PlayerSetup.ConnectedPlayers;
    private int votesLeft = 8;


    // Start is called before the first frame update
    public void StartMadlibs()
    {
        promptList = PlayerData.prompts;

        inputList = new Dictionary<string, int>
        {
            { "carrot", 0 },
            { "apple", 1 },
            { "tire", 2 },
            { "pizza", 0 },
            { "Chungus", 3 },
            { "boot", 4 },
            { "eagle", 5 },
            { "K-Mart", 6 }
        };

        playerColors = new List<Color32>
        {
            new(200, 46, 46, 255),
            new(46, 73, 200, 255),
            new(46, 200, 55, 255),
            new(200, 199, 46, 255),
            new(200, 46, 200, 255),
            new(46, 200, 195, 255),
            new(200, 124, 46, 255),
            new(189, 190, 189, 255)
        };

        promptText.text = promptList[promptEnum];

        foreach(KeyValuePair<string, int> input in inputList)
        {
            Button button = buttons[inputEnum];
            button.gameObject.SetActive(true);
            button.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = input.Key;
            inputEnum++;
        }
        CreateScoreList();
        rankList = new Dictionary<int, int>();

        audioManager.playMusic(music);
    }

    public void NextPrompt()
    {
        promptEnum++;
        promptText.text = promptList[promptEnum];
    }

    public void CastVote(int buttonID)
    {
        Button button = buttons[buttonID];
        string answer = button.gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
        int playerID = inputList[answer];
        AddVote(playerID);
    }

    public void AddVote(int playerID)
    {
        scoreList[playerID]++;
        votesLeft--;
        if(votesLeft == 0)
        {
            TallyVotes();
        }
    }

    public void TallyVotes()
    {
        int hiScore = 0;
        foreach(KeyValuePair<int, int> score in scoreList)
        {
            if(score.Value > hiScore)
            {
                hiScore = score.Value;
            }
        }
        int rankNumber = 1;
        bool rankAdded = false;
        while(hiScore >= 0)
        {

            foreach (KeyValuePair<int, int> score in scoreList)
            {
                if (score.Value == hiScore) 
                {
                    rankList.Add(score.Key, score.Value);
                    rankAdded = true;
                }
            }
            hiScore--;
            if(rankAdded)
            {
                rankNumber++;
                rankAdded = false;
            }
        }
        DisplayResults();
    }

    public void DisplayResults()
    {
        foreach(Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        promptText.text = "And the Winner is...";
        audioManager.playSFX(resultSFX);
        promptText.text = "Results";
        //resultsPanel.SetActive(true);
        //string resultsText = "Player  Rank" + Environment.NewLine;
        int resultEnum = 0;
        foreach(KeyValuePair<int, int> rank in rankList)
        {
            GameObject panel = resultPanels[resultEnum];
            panel.SetActive(true);
            Image panelImage = panel.GetComponent<Image>();
            panelImage.color = playerColors[rank.Key];
            Image playerImage = panel.GetComponentInChildren<Image>();
            //playerImage.sprite = PlayerData.playerSprite;

            TextMeshProUGUI[] resultTexts = panel.GetComponentsInChildren<TextMeshProUGUI>();
            resultTexts[0].text = "Player" + (rank.Key + 1);
            resultTexts[1].text = rank.Value.ToString();
            //resultsText += "Player" + (rank.Key + 1) + "  " + rank.Value + Environment.NewLine;
            resultEnum++;
        }
    }

    public void CreateScoreList()
    {
        scoreList = new Dictionary<int, int>();
        //for (int i = 0; i < PlayerSetup.ConnectedPlayers; i++) 
        for (int i = 0; i < 8; i++)
        {
            scoreList.Add(i, 0);
        }
    }

    public void ProcessInput(string input)
    {
        inputList.Add(input, PlayerData.PlayerNumber);
    }
}
