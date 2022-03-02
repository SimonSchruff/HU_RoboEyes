using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript instance; 
    private void Awake()
    {
        if(instance != null)
            Destroy(this);  
        else
            instance = this;         
    }

    public void LoadSceneByName(string name)
    {
        SceneManager.LoadScene(name); 
    }

    public void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index); 
    }
}
