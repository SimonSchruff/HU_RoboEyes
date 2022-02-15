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
        coneEye, 
        flatEye
    }
    public PointerType pointerType; 
    public int CurrentSquare = 0; 

    [Header("Time Variables")]
    public float TimeToMoveToSquare = 5f; 
    public float TimeToRecover = 5f; 
    public float BreakTime = 1f; 
    public List<int> Sequence = new List<int>(5); 
    public FollowSequenceArrow[] Arrows = new FollowSequenceArrow[2]; 
    public FollowSequenceEyes[] Eyes = new FollowSequenceEyes[2]; 
    public DesignerEye[] flatEyes = new DesignerEye[2]; 
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
        /// Roboter Ablauf: 
        /// Start -> 5s Pause
        /// Pro Bewegung: 
        ///     -> 1s Pause ( auf null position )
        ///     -> 5s Bewegung auf Ziel
        ///     -> 5s Bewegung auf Null

        m_timer += Time.deltaTime; 

        // Ensures that inital move takes a break of 5sec before starting to move
        if(m_initalMove && m_timer >= 5f)
        {
            m_initalMove = false; 
            m_timer = 1000f; 
        }

        //Gives Square Transform to each Pointer in scene according to pointerType enum
        if(m_timer >= (TimeToMoveToSquare + TimeToRecover + BreakTime) && !m_initalMove )
        {
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
                case PointerType.flatEye: 
                    foreach(DesignerEye eye in flatEyes)
                    {
                        if(eye == null)
                            return; 
                        int squareToShow = Sequence[CurrentSquare]; 
                        eye.StartCoroutine(eye.Sequence(Squares[squareToShow])); 
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
                foreach(DesignerEye e in flatEyes)
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
                foreach(DesignerEye e in flatEyes)
                {
                    e.gameObject.SetActive(false); 
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
                foreach(DesignerEye e in flatEyes)
                {
                    e.gameObject.SetActive(false); 
                }
            break; 
            case PointerType.flatEye: 
                foreach(FollowSequenceArrow a in Arrows)
                {
                    a.gameObject.SetActive(false); 
                }
                foreach(FollowSequenceEyes e in Eyes)
                {
                    e.gameObject.SetActive(false); 
                }
                foreach(DesignerEye e in flatEyes)
                {
                    e.gameObject.SetActive(true); 
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
