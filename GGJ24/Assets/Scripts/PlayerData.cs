using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    // reference of Relaymanager
    public RelayManager relay;

    // Static Integer representing (this) player's playerNumber
    public static int PlayerNumber;

    // Tuple List Contains the mini game grade;
    // For each Tuple, the item 1 is an integer representing Player Number, and the item 2 is the score that player got in the mini game.
    public List<(int, int)> miniGameGrade;
    
    // Integer recording how many "mini game grade"/"prompt answer" the host have received.
    private int gradeReceivedCount;
    private int ansReceivedCount;

    // Tuple List Contains the answer for the prompt.
    // Item 1 is Player Number and Item 2 is the input text.
    public List<(int, string)> answerList;

    // Bool representing whether the host has sent out the ranking of minigame or not.
    private bool gradeSent;

    // The total number of connected players in current game
    private int totalPlayerNumber;

    // **TODO** Array for the host to calculate final votes for each player.
    private int[] votes;

    // Array of Integers representing grades, but the order is after ranking.
    public int[] gradeList;

    // Array of integers representing the player numbers, in ranking order.
    public int[] ranking;

    public static string[] prompts =
    {
        "My favorite food is ______.",
        "An ________ a day keeps the goblins away.",
        "When life gives you ______ make ______-ade!",
        "Don't touch that! It's covered in ______!",
        "You know it's a bad day when the ______ is missing"
    };

    // Start is called before the first frame update
    void Start()
    {
        gradeReceivedCount = 0;
        ansReceivedCount = 0;
        gradeSent = false;
        miniGameGrade = new List<(int, int)>();
        answerList = new List<(int, string)>();
    }
    

    // After click the StartGame button, run this method to initialize some data.
    public void updateStartGame(int n)
    {
        totalPlayerNumber = PlayerSetup.ConnectedPlayers;
        votes = new int[totalPlayerNumber];
        for (int i = 0; i < totalPlayerNumber; i++)
        {
            votes[i] = 0;
        }
    }


    // For client to call. Aim to let host add the answer to the answer list.
    // n is the player number of the client, ans is the answer text;
    public void addAnswer(int n, string ans)
    {
        if (ansReceivedCount < 2 * totalPlayerNumber)
        {
            answerList.Add((n, ans));
            ansReceivedCount += 1;
        }
    }


    // Originally write for the host to add its own answer but I guess doesn't really need this.
    public void addOwnAnswer(string ans)
    {
        answerList.Add((PlayerNumber, ans));
    }

    // Same as addOwnAnswer()
    public void addOwnGrade(int grade)
    {
        miniGameGrade.Add((PlayerNumber, grade));
        gradeReceivedCount += 1;
    }

    // For client to call. Aim to let host add client's mini game grade to the grade list.
    // n is the player number of the client, grade is that client's mini game grade.
    public void addGrade(int n, int grade)
    {
        if (gradeReceivedCount < totalPlayerNumber)
        {
            miniGameGrade.Add((n, grade));
            gradeReceivedCount += 1;
        }
        else if (!gradeSent)
        {
            gradeSent = true;
            miniGameGrade.Sort((a,b) => a.Item2.CompareTo(b.Item2));
            sendGrade();
        }
    }

    // Host use this method to sort and reformat mini game grade and send to each client.
    void sendGrade()
    { 
        int[] playerNumbers = new int[miniGameGrade.Count];
        int[] playerGrades = new int[miniGameGrade.Count];
        for (int i = 0; i < miniGameGrade.Count; i++)
        {
            playerNumbers[i] = (miniGameGrade[i].Item1);
            playerGrades[i] = miniGameGrade[i].Item2;
        }
        relay.sendMiniGameGradeClientRpc(playerNumbers);
    } 

    // **TODO** supposed to do the same thing as sendGrade() but just change the target to the text answer.
    void sendAnswer()
    {
        string answer = "test";
        relay.sendAnswerClientRpc(PlayerNumber, answer);
    }

    // for clients to receive and store the results of grade/answer after host's processing.
    public void setArr(int index, int[] arr)
    {
        if (index == 0)
        {
            gradeList = arr;
        }
        else
        {
            ranking = arr;
        }
    }
}
