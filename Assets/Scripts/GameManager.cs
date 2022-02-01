using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using System; 

public class GameManager : MonoBehaviour
{
    public static GameManager instance; 
    public enum PointerType
    {
        arrow, 
        eye, 
        coneEye
    }
    public PointerType pointerType; 
    public int CurrentSquare = 0; 

    [Header("Time Variables")]
    public float TimeToMoveToSquare = 4f; 
    public float TimeToRecover = 2f; 
    public List<int> Sequence = new List<int>(5); 
    public FollowSequenceArrow[] Arrows = new FollowSequenceArrow[2]; 
    public FollowSequenceEyes[] Eyes = new FollowSequenceEyes[2]; 
    private float m_timer; 
    private bool m_initalMove = true; 

    [Serializable]
    public struct Square
    {
        public int ID; 
        public Transform transform; 
    }
    public Square[] Squares = new Square[6]; 

    

    void Awake()
    {
        if(instance != null)
            Destroy(this);  
        else
            instance = this;         

        // Set active / false for pointerType selected in enum
        SetActivePointerType(); 
    }

    private void Update()
    {
        m_timer += Time.deltaTime; 

        //Gives Square Transform to each Pointer in scene according to pointerType enum
        if(m_timer >= (TimeToMoveToSquare + TimeToRecover) || m_initalMove)
        {
            m_initalMove = false; 
            
            if(CurrentSquare >= Sequence.Count)
            {
                GameOver(); 
                return; 
            }

            switch(pointerType)
            {
                case PointerType.arrow: 
                    foreach(FollowSequenceArrow arrow in Arrows)
                    {
                        if(arrow == null)
                            return; 
                        int squareToShow = Sequence[CurrentSquare]; 
                        arrow.StartCoroutine(arrow.Sequence(Squares[squareToShow])); 
                    }
                break; 
                case PointerType.eye: 
                    for (int i = 0; i < 2; i++)
                    {
                        if(Eyes[i] == null)
                            return; 
                        int squareToShow = Sequence[CurrentSquare]; 
                        Eyes[i].StartCoroutine(Eyes[i].Sequence(Squares[squareToShow])); 
                    }
                break; 
                case PointerType.coneEye: 
                    for (int i = 2; i < 4; i++)
                    {
                        if(Eyes[i] == null)
                            return; 
                        int squareToShow = Sequence[CurrentSquare]; 
                        Eyes[i].StartCoroutine(Eyes[i].Sequence(Squares[squareToShow])); 
                    }
                break; 
                
            }

            m_timer = 0; 
            CurrentSquare++;  
        }
    }

    private void SetActivePointerType()
    {
        switch (pointerType)
        {
            case PointerType.arrow: 
                foreach(FollowSequenceArrow a in Arrows)
                {
                    a.gameObject.SetActive(true); 
                }
                foreach(FollowSequenceEyes e in Eyes)
                {
                    e.gameObject.SetActive(false); 
                }
            break; 
            case PointerType.eye: 
                foreach(FollowSequenceArrow a in Arrows)
                {
                    a.gameObject.SetActive(false); 
                }
                for (int i = 0; i < 4; i++)
                {
                    if(i < 2)
                        Eyes[i].gameObject.SetActive(true); 
                    else
                        Eyes[i].gameObject.SetActive(false); 
                }
            break; 
            case PointerType.coneEye: 
                foreach(FollowSequenceArrow a in Arrows)
                {
                    a.gameObject.SetActive(false); 
                }
                for (int i = 0; i < 4; i++)
                {
                    if(i < 2)
                        Eyes[i].gameObject.SetActive(false); 
                    else
                        Eyes[i].gameObject.SetActive(true); 
                }
            break; 
        }
    }

    public void GameOver()
    {
        print("GAME OVER! Reload Current Scene!"); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void LoadSceneByName(string name)
    {
        SceneManager.LoadScene(name); 
    }
}
