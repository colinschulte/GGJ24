using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinnerScreen : MonoBehaviour
{
    public TMP_Text winner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showWinner(string name)
    {
        StartCoroutine(timer(name));
    }

    IEnumerator timer(string name)
    {
        yield return new WaitForSeconds(3);
        winner.text = name;
    }
}
