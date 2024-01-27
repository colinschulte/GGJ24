using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TappingManager : MonoBehaviour
{
    // int to store the number of counts
    int tap_count;
    // Text UI for countdown notation.
    public TMP_Text countdown_UI;
    // time for minigame
    public int time;

    private void Start()
    {
        countdown_UI.text = time.ToString();
        tap_count = 0;
        Start_timer();
    }

    // on_click() for the button
    public void tap()
    {
        tap_count += 1;
        Debug.Log(tap_count);
    }

    // Start the timer Coroutine
    private void Start_timer()
    {
        StartCoroutine(timer(time));
    }

    // The timer to count seconds, n -> the number of seconds to count down
    IEnumerator timer(int n)
    {
        int time_left = time;
        for (int i = 0; i < n; i++)
        {
            yield return new WaitForSeconds(1);
            time_left -= 1;
            countdown_UI.text = time_left.ToString();
        }
        // yield return new WaitForSeconds(n);
        SceneManager.LoadScene("SampleScene");
    }
}
