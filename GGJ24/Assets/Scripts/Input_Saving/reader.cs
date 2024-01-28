using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class reader : MonoBehaviour
{
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

    public void click_submit() {
        player.sendAnswerToServer(ans1);
        player.sendAnswerToServer(ans2);
    }
}
