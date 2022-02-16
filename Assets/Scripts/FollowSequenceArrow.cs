using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using System; 

public class FollowSequenceArrow : MonoBehaviour
{
    [Header("Variables")]
    public bool IsLeftArrow; 

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

    public IEnumerator Sequence(GameManager.Square square)
    {
        m_sp.sprite = ArrowInactiveSprite;  

        yield return new WaitForSeconds(GameManager.instance.BreakTime);

        m_sp.sprite = ArrowSprite;  
        float angle = CalculateAngle(square); 
        transform.eulerAngles = new Vector3(0,0,angle); 
        
        yield return new WaitForSeconds(GameManager.instance.TimeToMoveToSquare);

        // Robo has reached square
        transform.eulerAngles = Vector3.zero; 
        m_sp.sprite = ArrowInactiveSprite;  
    }

    private float CalculateAngle(GameManager.Square square)
    {
        Vector3 direction = new Vector3(square.transform.position.x - transform.position.x, square.transform.position.y - transform.position.y, 0);
        float zValue = ( Mathf.Abs(square.transform.position.z) < 1 ) ? 1 : Mathf.Abs(square.transform.position.z); 
        
        float ratio = 1 / zValue; 
        print(ratio); 
        // Bigger Angle if zValue == 1


        float angle = Vector3.Angle(Vector3.down, direction); 
        /*
        if(zValue == 1)
            angle *= 1.5f;
        else if(zValue > 1)
            angle *= .75f; 
        */
        //print("Target Square: " + square.ID + "; Target Vector: " + direction + "; Angle: " + angle); 

        if(square.ID == 0 || square.ID == 3)
            angle = -angle; 
        
        if(!IsLeftArrow)
        {
            if(square.ID == 1 || square.ID == 4)
                angle = -angle; 
        }

        if(ratio < 1)
            angle = angle * (1 - ratio); 
        else
            angle = angle * (1 + ratio); 
             
        return angle; 
    }

}
