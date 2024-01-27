using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public RelayManager relay;

    public static int PlayerNumber;

    public List<(int, int)> miniGameGrade;

    private int gradeReceivedCount;
    private int ansReceivedCount;

    public List<(int, string)> answerList;

    private bool gradeSent;

    private int totalPlayerNumber;

    public int[] gradeList;
    public int[] ranking;

    // Start is called before the first frame update
    void Start()
    {
        gradeReceivedCount = 0;
        ansReceivedCount = 0;
        gradeSent = false;
        miniGameGrade = new List<(int, int)>();
        answerList = new List<(int, string)>();
    }

    public void updateTotalPlayerNumber(int n)
    {
        totalPlayerNumber = n;
    }

    public void addAnswer(int n, string ans)
    {
        if (ansReceivedCount < 2 * totalPlayerNumber)
        {
            answerList.Add((n, ans));
            ansReceivedCount += 1;
        }
    }

    public void addOwnAnswer(string ans)
    {
        answerList.Add((PlayerNumber, ans));
    }

    public void addOwnGrade(int grade)
    {
        miniGameGrade.Add((PlayerNumber, grade));
        gradeReceivedCount += 1;
    }

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

    void sendAnswer()
    {
        string answer = "test";
        relay.sendAnswerClientRPC(PlayerNumber, answer);
    }

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
