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
    private List<string> promptList;
    private Dictionary<string, int> inputList;
    private int promptEnum = 0;
    private int inputEnum = 0;

    // Start is called before the first frame update
    void Start()
    {
        promptList = new List<string>
        {
            "My favorite food is ______.",
            "An ________ a day keeps the goblins away.",
            "When life gives you ______ make ______-ade!",
            "Don't touch that! It's covered in ______!",
            "You know it's a bad day when the ______ is missing"
        };

        inputList = new List<(string, int)>
        {
            ("carrot", 0),
            ("apple", 1),
            ("tire", 2),
            ("pizza", 0),
            ("Chungus", 3),
            ("boot", 4),
            ("eagle", 5),  
            ("K-Mart", 6)
        };

        promptText.text = promptList[promptEnum];

        foreach((string, int) input in inputList)
        {
            Button button = buttons[inputEnum];
            button.gameObject.SetActive(true);
            button.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = input.Item1;
            inputEnum++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
        int playerID = inputList.Find(answer)
    }
}
