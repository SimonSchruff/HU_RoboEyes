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
    public bool IsLeftArrow; 
    public bool NextSequenceRdy = true; 


    [Header("References")]
    public Sprite ArrowSprite; 
    public Sprite ArrowInactiveSprite; 
    public TextMeshProUGUI DebugText; 


    private SpriteRenderer m_sp; 
    private int m_currentSquare = 0;
    private Vector3 m_ArrowPosOffset;  
    private Vector3 m_targetPos; 

    // TO-DO: 
    // - Create Dictionary for Angles so it does not have to be calculated twice

    void Start()
    {
        m_sp = GetComponentInChildren<SpriteRenderer>(); 
        m_sp.sprite = ArrowInactiveSprite; 

        //DebugText.text = null; 
        
    }

    void Update()
    {
       
    }

    public IEnumerator Sequence(GameManager.Square square)
    {
        //NextSequenceRdy = false; 
        m_sp.sprite = ArrowSprite;  
        //DebugText.text = ("Robo Arm is moving towards square " + square.gameObject.name); 

        //Get direction towards arrow
        //GameManager.instance.SquarePositions.TryGetValue(square, out Vector3 squarePos) ; 
        //Vector2 direction = squarePos - transform.position;  

        //Rotate Arrow in that direction
        //float angle = Vector2.SignedAngle(Vector2.right, direction); 
        float angle = CalculateAngle(square); 
        transform.eulerAngles = new Vector3(0,0,angle); 
        
        yield return new WaitForSeconds(GameManager.instance.TimeToMoveToSquare);

        // Robo has reached square
        transform.eulerAngles = Vector3.zero; 
        m_sp.sprite = ArrowInactiveSprite;  
        //DebugText.text = ("Robo Arm is moving to default position"); 

        //yield return new WaitForSeconds(GameManager.instance.TimeToRecover); 

        //NextSequenceRdy = true; 
    }

    private float CalculateAngle(GameManager.Square square)
    {
        Vector3 targetVector = new Vector3(square.transform.position.x - transform.position.x, square.transform.position.y - transform.position.y, square.transform.position.z - transform.position.z);
        

        float angle = Vector3.Angle(Vector3.down, targetVector); 
        print("Target Square: " + square.ID + "; Target Vector: " + targetVector + "; Angle: " + angle); 

        if(square.ID == 0 || square.ID == 3)
            angle = -angle; 
        
        if(!IsLeftArrow)
        {
            if(square.ID == 1 || square.ID == 4)
                angle = -angle; 
        }

        return angle; 


    }

}
