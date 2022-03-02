using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject Settings; 
    public GameObject StartScreen; 

    private void Start() 
    {
        Settings.SetActive(true); 
        StartScreen.SetActive(false); 
    }

    public void LoadScreen(int i)
    {
        switch(i)
        {
            case 1: 
                Settings.SetActive(true); 
                StartScreen.SetActive(false); 
            break; 
            case 2:
                Settings.SetActive(false); 
                StartScreen.SetActive(true);
            break; 
        }
    }

    public void Quit()
    {
        Application.Quit(); 
    }
}
