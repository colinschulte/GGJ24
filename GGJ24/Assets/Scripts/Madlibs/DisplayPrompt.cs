using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms.Impl;

public class DisplayPrompt : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> buttonTexts = new();
    [SerializeField] private List<TMP_Text> panelScores = new();


    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private List<Button> buttons;
    [SerializeField] private List<GameObject> resultPanels;
    [SerializeField] private RelayManager relayManager;
    [SerializeField] private List<Color32> playerColors;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip resultSFX;

    //private string[] promptList;
    public Dictionary<int, int> scoreList;
    private Dictionary<int, int> rankList;
    public List<(int, string)> inputList = new();

    //private int promptEnum = 0;
    //private int inputEnum = 0;
    //private int votesLeft = PlayerSetup.ConnectedPlayers;
    //private int votesLeft = 1;


    // Start is called before the first frame update
    public void StartMadlibs()
    {
        //promptList = PlayerData.prompts;

        inputList = playerData.answerToBeShown;

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

        promptText.text = playerData.prompt;

        List<string> randomizedInputs = new List<string>();
        for (int i = 0; i < inputList.Count; i++)
        {
            randomizedInputs.Add(inputList[i].Item2);
        }

        randomizedInputs = RandomizeInputs(randomizedInputs);


        for (int i = 0; i < inputList.Count; i++)
        {
            buttons[i].gameObject.SetActive(true);
            buttonTexts[i].text = randomizedInputs[i];
        }

        //foreach(KeyValuePair<string, int> input in inputList)
        //{
        //    Button button = buttons[inputEnum];
        //    button.gameObject.SetActive(true);
        //    button.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = input.Key;
        //    inputEnum++;
        //}
        CreateScoreList();
        rankList = new Dictionary<int, int>();

        audioManager.playMusic(music);
    }

    private List<string> RandomizeInputs(List<string> inputs)
    {
        List<string> shuffled = new List<string>();

        List<string> temp = new List<string>();
        temp.AddRange(inputs);

        for (int i = 0; i < inputs.Count; i++)
        {
            int index = UnityEngine.Random.Range(0, temp.Count - 1);
            shuffled.Add(temp[index]);
            temp.RemoveAt(index);
        }

        return shuffled;
    }




    public void CastVote(int buttonID)
    {
        Button button = buttons[buttonID];
        string answer = button.gameObject.GetComponentInChildren<TextMeshProUGUI>().text;

        for (int i = 0; i < inputList.Count; i++)
        {
            if (inputList[i].Item2 == answer)
            {
                AddVote(inputList[i].Item1);
                return;
            }
        }
    }

    public void AddVote(int playerID)
    {
        //scoreList[playerID]++;
        //votesLeft--;
        //if(votesLeft == 0)
        //{
        //    TallyVotes();
        //}

        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        promptText.text = "Waiting for host...";

        relayManager.SendResultsToHostServerRpc(playerID);
    }

    public void TallyVotes() // run on host
    {
        int hiScore = 0;
        foreach (KeyValuePair<int, int> score in scoreList)
        {
            if (score.Value > hiScore)
            {
                hiScore = score.Value;
            }
        }
        int rankNumber = 1;
        bool rankAdded = false;
        while (hiScore >= 0)
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
            if (rankAdded)
            {
                rankNumber++;
                rankAdded = false;
            }
        }

        promptText.text = "And the Winner is...";
        audioManager.playSFX(resultSFX);
        promptText.text = "Results";


        int[] playerNumbers = new int[8];
        int[] scores = new int[8];
        int i = 0;
        foreach (KeyValuePair<int, int> score in scoreList)
        {
            playerNumbers[i] = score.Key;
            scores[i] = score.Value;
            i++;
        }
        
        relayManager.SendScoresToClientsClientRpc(playerNumbers, scores);

        //DisplayResults();
    }

    public void DisplayResults(int[] playerNumbers, int[] scores)
    {
        foreach (GameObject panel in resultPanels)
            panel.SetActive(true);

        for (int i = 0; i < scores.Length; i++)
        {
            panelScores[playerNumbers[i]].text = scores[i].ToString();
        }
    }

    //public void DisplayResults()
    //{
    //    promptText.text = "And the Winner is...";
    //    audioManager.playSFX(resultSFX);
    //    promptText.text = "Results";
    //    //resultsPanel.SetActive(true);
    //    //string resultsText = "Player  Rank" + Environment.NewLine;
    //    int resultEnum = 0;
    //    foreach(KeyValuePair<int, int> rank in rankList)
    //    {
    //        GameObject panel = resultPanels[resultEnum];
    //        panel.SetActive(true);
    //        Image panelImage = panel.GetComponent<Image>();
    //        panelImage.color = playerColors[rank.Key];
    //        Image playerImage = panel.GetComponentInChildren<Image>();
    //        //playerImage.sprite = PlayerData.playerSprite;

    //        TextMeshProUGUI[] resultTexts = panel.GetComponentsInChildren<TextMeshProUGUI>();
    //        resultTexts[0].text = "Player" + (rank.Key + 1);
    //        resultTexts[1].text = rank.Value.ToString();
    //        //resultsText += "Player" + (rank.Key + 1) + "  " + rank.Value + Environment.NewLine;
    //        resultEnum++;
    //    }
    //}

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
        //inputList.Add(input, PlayerData.PlayerNumber);
    }
}
