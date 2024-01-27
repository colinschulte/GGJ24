using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Writer : MonoBehaviour
{
    public TMP_Text Text;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("text"))
        {
            Text.text = PlayerPrefs.GetString("text");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
