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
        eye
    }
    public PointerType pointerType; 
    public int CurrentSquare = 0; 
    public List<int> Sequence = new List<int>(10);

    [Header("Time Variables")]
    public float StartTimerLength = 5f; 
    public float TimeToMoveToSquare = 5f; 
    public float TimeToRecover = 5f; 
    public float BreakTime = 1f; 
    public float StartTimeDelay = 5f; 

    [Header("Object References")]
    public FollowSequenceArrow[] Arrows = new FollowSequenceArrow[2]; 
    public DesignerEye[] Eyes = new DesignerEye[2]; 
    public float ArrowAngleMultiplier; 
    public bool LookAtArmNullPos; 

    [Serializable]
    public struct Square
    {
        public int ID; 
        public Transform transform; 
    }
    
    public Square[] Squares = new Square[6]; 
    public Vector3 ArmNullPos = new Vector3(); 
    [SerializeField] private TextMeshProUGUI m_timerText; 
    [SerializeField] private GameObject m_gameOverObjects; 

    private bool m_isStartTimerOver = false; 
    private float m_timer; 
    private bool m_initalMove = true; 
    private bool m_gameOver = false; 

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
        if(!m_isStartTimerOver)
            return; 

        m_timer += Time.deltaTime; 

        // Ensures that inital move takes a break of 5sec before starting to move
        if(m_initalMove && m_timer >= StartTimeDelay)
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
                        int squareToShow = Sequence[CurrentSquare] - 1; // - 1 of Index, because Numbers should match Markis numbering in config file; 
                        arrow.StartCoroutine(arrow.Sequence(Squares[squareToShow])); 
                    }
                break; 
                case PointerType.eye: 
                    foreach(DesignerEye eye in Eyes)
                    {
                        if(eye == null)
                            return; 
                        int squareToShow = Sequence[CurrentSquare] - 1; 
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
                foreach(DesignerEye e in Eyes)
                {
                    e.gameObject.SetActive(false); 
                }
            break; 
            case PointerType.eye: 
                foreach(FollowSequenceArrow a in Arrows)
                {
                    a.gameObject.SetActive(false); 
                }
                foreach(DesignerEye e in Eyes)
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
        foreach(DesignerEye e in Eyes)
        {
            e.gameObject.SetActive(false); 
        }
    }
    public void GameOver()
    {
        if(m_gameOver)
            return; 

        HideAllPointers(); 
        m_gameOverObjects.SetActive(true); 
        if(SaveManager.instance.gameObject != null)
            Destroy(SaveManager.instance.gameObject); // Ensures that old settings Data is not loaded again
        m_gameOver = true; 
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

    /// <summary>
    /// Loads Settings from SaveManager; 
    /// Called in Start(); 
    /// </summary>
    public void LoadSettings(SaveManager.SettingsData data)
    {
        pointerType = data.PointerType; 

        TimeToMoveToSquare = data.TimeToMoveToSquare; 
        TimeToRecover = data.TimeToRecover; 
        BreakTime = data.BreakTime; 
        StartTimeDelay = data.StartTimeDelay; 

        ArrowAngleMultiplier = data.ArrowAngleMultiplier; 
        LookAtArmNullPos = data.LookAtArmNullPos; 

        for(int i = 0; i < Sequence.Count; i++)
        {
            Sequence[i] = data.Sequence[i]; 
        }

    }
}
