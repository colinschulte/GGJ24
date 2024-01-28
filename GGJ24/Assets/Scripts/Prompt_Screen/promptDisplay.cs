using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class promptDisplay : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    [SerializeField] TMP_Text prompt;

    [SerializeField] TMP_InputField input1;
    [SerializeField] TMP_InputField input2;

    public Button submit;
    public void set_Text(string p) 
    {
        prompt.text = p;
        playerData.prompt = p;
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
        input1.interactable = false;
        input2.interactable = false;
        submit.interactable = false;
    }
}
