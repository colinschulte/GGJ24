using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class promptDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text prompt;

    public void set_Text(string p) 
    {
        prompt.text = p;
    }

    private string ans1;
    private string ans2;

    [SerializeField] private PlayerData player;

    public void save1(string str)
    {
        ans1 = str;
    }

    public void save2(string str)
    {
        ans2 = str;
    }

    public void click_submit()
    {
        player.sendAnswerToServer(ans1);
        player.sendAnswerToServer(ans2);
    }
}
