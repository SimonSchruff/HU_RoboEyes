using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using System; 

public class GameManager : MonoBehaviour
{
    public bool isArrowScene; 
    public static GameManager instance; 
    public int CurrentSquare = 0; 

    [Header("Time Variables")]
    public float TimeToMoveToSquare = 4f; 
    public float TimeToRecover = 2f; 
    private float m_timer; 
    public List<int> Sequence = new List<int>(5); 
    public FollowSequenceArrow[] Arrows = new FollowSequenceArrow[2]; 
    public FollowSequenceEyes[] Eyes = new FollowSequenceEyes[2]; 

    [Serializable]
    public struct Square
    {
        public int ID; 
        public Transform transform; 
    }
    public Square[] Squares = new Square[6]; 
   
    [Header("Position Variables")]
    public float Height = 70f; // In centemeters
    public float DistanceToTable = 30f; 
    public float DistanceToFirstRow = 17f; 
    public float DistanceToSecondRow = 60f; 
    public float XDistanceBetweenSquares = 32f; 
    public float ZDistanceBetweenSquares = 43f; 
    public float TabletSizeX = 25.5f; 
    public float TabletSizeY = 16f; 

    void Awake()
    {
        if(instance != null)
            Destroy(this); 
        else
            instance = this;         
    }

    private void Start() 
    {
        
    }

    private void Update()
    {
        m_timer += Time.deltaTime; 

        //Gives Square Transform to each Arrow in scene
        if(m_timer >= (TimeToMoveToSquare + TimeToRecover))
        {
            if(CurrentSquare >= Sequence.Count)
            {
                GameOver(); 
                return; 
            }

            if(isArrowScene)
            {
                foreach(FollowSequenceArrow arrow in Arrows)
                {
                    if(arrow == null)
                        return; 
                    int squareToShow = Sequence[CurrentSquare]; 
                    arrow.StartCoroutine(arrow.Sequence(Squares[squareToShow])); 
                }
            }
            else if(!isArrowScene)
            {
                //Reference to eyes
                foreach (FollowSequenceEyes eye in Eyes)
                {
                    if(eye == null)
                        return; 
                    int squareToShow = Sequence[CurrentSquare]; 
                    eye.StartCoroutine(eye.Sequence(Squares[squareToShow])); 
                }
            }
            m_timer = 0; 
            CurrentSquare++;  
        }
    }

    public void GameOver()
    {
        print("GAME OVER!!"); 
    }

    public void LoadSceneByName(string name)
    {
        SceneManager.LoadScene(name); 
    }
}
