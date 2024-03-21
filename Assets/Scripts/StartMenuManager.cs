using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class StartMenuManager : MonoBehaviour
{
    public static StartMenuManager Shared;

    [SerializeField] private TMP_InputField NameInputField;
    public string PlayerName = "";

    void Awake() {
        if(Shared == null) {
            Shared = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButtonPressed() {
        PlayerName = NameInputField.text;
        SceneManager.LoadScene(1);
    }
}
