using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static int PlayerNumber;

    private List<(int, int)> miniGameGrade;

    private int msgReceivedCount;

    private List<(int, string)> answerList;

    private int totalPlayerNumber;

    // Start is called before the first frame update
    void Start()
    {
        msgReceivedCount = 1;
    }

    public void updateTotalPlayerNumber(int n)
    {
        totalPlayerNumber = n;
    }

    public void addAnswer(int n, string ans)
    {
        answerList.Add((n, ans));
        msgReceivedCount += 1;
    }

    public void addGrade(int n, int grade)
    {
        miniGameGrade.Add((n, grade));
    }
}
