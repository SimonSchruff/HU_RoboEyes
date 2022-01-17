using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using System; 

public class FollowSequenceArrow : MonoBehaviour
{
    [Header("Variables")]
    public float TimeToMoveToSquare; 
    public float TimeToRecover; 


    [Header("References")]
    public Sprite ArrowSprite; 
    public Sprite ArrowInactiveSprite; 
    public TextMeshProUGUI DebugText; 


    private SpriteRenderer m_sp; 
    private bool m_nextSequence = true; 
    private int m_currentSquare = 0; 

    void Start()
    {
        m_sp = GetComponentInChildren<SpriteRenderer>(); 
        m_sp.sprite = ArrowInactiveSprite; 

        DebugText.text = null; 
        CalculateDirVector(); 
    }

    void Update()
    {
        if(m_currentSquare >= GameManager.instance.Squares.Count && m_nextSequence)
        {
            GameManager.instance.LoadSceneByName("Menu"); 
            return; 
        }

        if(m_nextSequence)
        {
            StartCoroutine(Sequence(GameManager.instance.Squares[m_currentSquare])); 
            m_currentSquare++;  
        }
    }

    private IEnumerator Sequence(int square)
    {
        m_nextSequence = false; 
        m_sp.sprite = ArrowSprite;  
        DebugText.text = ("Robo Arm is moving towards square " + square); 

        //Get direction towards arrow
        //GameManager.instance.SquarePositions.TryGetValue(square, out Vector3 squarePos) ; 
        //Vector2 direction = squarePos - transform.position;  

        //Rotate Arrow in that direction
        //float angle = Vector2.SignedAngle(Vector2.right, direction); 
        float angle = transform.rotation.eulerAngles.z - CalculateDirVector(); 
        transform.eulerAngles = new Vector3(0,0,angle); 
        
        yield return new WaitForSeconds(TimeToMoveToSquare);

        // Robo has reached square
        transform.eulerAngles = Vector3.zero; 
        m_sp.sprite = ArrowInactiveSprite;  
        DebugText.text = ("Robo Arm is moving to default position"); 

        yield return new WaitForSeconds(TimeToRecover); 

        m_nextSequence = true; 
    }

    private float CalculateDirVector()
    {
        float y = 60; 
        float x = 30 ; 
        float z = 20; 

        float c = Mathf.Sqrt((x*x) + (z * z)); 
        print("c = " + c); 

        float angle = Mathf.Asin( x/c ) * Mathf.Rad2Deg ; 
        print(angle); 

        return angle; 




        
    }
}
