using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    public static GameManager instance; 
    public List<int> Squares = new List<int>(5); 
    public Dictionary<int, Vector3> SquarePositions = new Dictionary<int, Vector3>(); 

    void Awake()
    {
        if(instance != null)
            Destroy(this); 
        else
            instance = this; 

        if(SceneManager.GetActiveScene().name == "ArrowSequence")
        {
            SquarePositions.Add(1, new Vector3(-9,-20,0)); 
            SquarePositions.Add(2, new Vector3(0,-20,0)); 
            SquarePositions.Add(3, new Vector3(9,-20,0)); 

            Squares[0] = 2; 
            Squares[1] = 1; 
            Squares[2] = 3; 
            Squares[3] = 2; 
            Squares[4] = 3; 
        }


        
    }

    public void LoadSceneByName(string name)
    {
        SceneManager.LoadScene(name); 
    }
}
