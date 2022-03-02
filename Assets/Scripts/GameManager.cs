using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using System; 
using TMPro; 

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
    public List<int> Sequence = new List<int>(10);

    [Header("Time Variables")]
    public float StartTimerLength = 5f; 
    public float TimeToMoveToSquare = 5f; 
    public float TimeToRecover = 5f; 
    public float BreakTime = 1f; 

    [Header("Object References")]
    public FollowSequenceArrow[] Arrows = new FollowSequenceArrow[2]; 
    public FollowSequenceEyes[] Eyes = new FollowSequenceEyes[2]; 
    public DesignerEye[] flatEyes = new DesignerEye[2]; 
    public float ArrowAngleMultiplier; 

    [Serializable]
    public struct Square
    {
        public int ID; 
        public Transform transform; 
    }
    
    public Square[] Squares = new Square[6]; 
    public Vector3 ArmNullPos = new Vector3(); 
    [SerializeField] private TextMeshProUGUI m_timerText; 
    private bool m_isStartTimerOver = false; 
    private float m_timer; 
    private bool m_initalMove = true; 

    private void Awake()
    {
        if(instance != null)
            Destroy(this);  
        else
            instance = this;         

        // Starts Timer to Sync Robot properly
        // Also sets active selected Pointer Type
        StartCoroutine(StartTimer()); 

    }
    private void Start()
    {
        LoadSettings(SaveManager.instance.settingsData); 
    }

    private void Update()
    {
        /// Roboter Ablauf: 
        /// Start -> 5s Pause
        /// Pro Bewegung: 
        ///     -> 1s Pause ( auf null position )
        ///     -> 5s Bewegung auf Ziel
        ///     -> 5s Bewegung auf Null

        if(!m_isStartTimerOver)
            return; 

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
            m_timer = 0; 

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

    private void HideAllPointers()
    {
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
            e.gameObject.SetActive(false); 
        }
    }
    public void GameOver()
    {
        
        HideAllPointers(); 
        m_timerText.gameObject.SetActive(true);
        m_timerText.text ="Block 1 Finished!"; 
        
    
        //LoadSceneByName(SceneManager.GetActiveScene().name); 
    }

    /// <summary>
    /// Creates 3,2,1 Timer before game starts and sets active selected pointerType; 
    /// Length Determined by public StartTimerLength variable (in Seconds)
    /// </summary>
    private IEnumerator StartTimer()
    {
        if(m_timerText == null)
        {
            Debug.LogError("No Timer Text Set in GameManager!"); 
            yield break; 
        }

        m_timerText.gameObject.SetActive(true); 
        
        for(int i = 0; i < StartTimerLength; i++)
        {
            m_timerText.text = (StartTimerLength - i).ToString(); 
            yield return new  WaitForSeconds(1); 
        }

        m_isStartTimerOver = true; 
        m_timerText.gameObject.SetActive(false); 
        
        SetActivePointerType(); 
    }

    public void LoadSettings(SaveManager.SettingsData data)
    {
        pointerType = data.PointerType; 

        TimeToMoveToSquare = data.TimeToMoveToSquare; 
        TimeToRecover = data.TimeToRecover; 
        BreakTime = data.BreakTime; 

        ArrowAngleMultiplier = data.ArrowAngleMultiplier; 

        for(int i = 0; i < Sequence.Count; i++)
        {
            print("For Loop: " + i); 
            Sequence[i] = data.Sequence[i]; 
        }

    }
}
