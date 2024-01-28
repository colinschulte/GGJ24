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
}
