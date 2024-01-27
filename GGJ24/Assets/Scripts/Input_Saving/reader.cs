using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class reader : MonoBehaviour
{
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void save(string str)
    {
        PlayerPrefs.SetString("text", str);
    }

    public void go_reader() {
        SceneManager.LoadScene("SampleScene");
    }
}
