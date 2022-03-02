using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using System; 

public class FollowSequenceArrow : MonoBehaviour
{
    [Header("Variables")]
    public bool IsLeftArrow; 
    private float _breakTime; 
    private float _timeToMoveToSquare; 
    private float _arrowAngleMultiplier; 

    [Header("References")]
    public Sprite ArrowSprite; 
    public Sprite ArrowInactiveSprite; 
    private SpriteRenderer m_sp; 

    // TO-DO: 
    // - Create Dictionary for Angles so it does not have to be calculated twice

    void Start()
    {
        m_sp = GetComponentInChildren<SpriteRenderer>(); 
        m_sp.sprite = ArrowInactiveSprite; 

        // Get Relevant Variables from GameManager
        _breakTime = GameManager.instance.BreakTime; 
        _timeToMoveToSquare = GameManager.instance.TimeToMoveToSquare; 
        _arrowAngleMultiplier = GameManager.instance.ArrowAngleMultiplier; 
    }

    public IEnumerator Sequence(GameManager.Square square)
    {
        m_sp.sprite = ArrowInactiveSprite;  

        yield return new WaitForSeconds(_breakTime);

        m_sp.sprite = ArrowSprite;  
        float angle = CalculateAngle(square); 
        transform.eulerAngles = new Vector3(0,0,angle); 
        
        yield return new WaitForSeconds(_timeToMoveToSquare);

        // Robo has reached square
        transform.eulerAngles = Vector3.zero; 
        m_sp.sprite = ArrowInactiveSprite;  
    }

    private float CalculateAngle(GameManager.Square square)
    {
        Vector3 direction = new Vector3(square.transform.position.x - transform.position.x, square.transform.position.y - transform.position.y, 0);
        float zValue = ( Mathf.Abs(square.transform.position.z) < 1 ) ? 1 : Mathf.Abs(square.transform.position.z); 
        
        float ratio = 1 / zValue; 

        float angle = Vector3.Angle(Vector3.down, direction); 

        if(square.ID == 1 || square.ID == 2)
            angle = -angle; 
        
        if(!IsLeftArrow)
        {
            if(square.ID == 3 || square.ID == 4)
                angle = -angle; 
        }

        if(ratio >= 1) // -> Square are close to Arrows
            angle = angle * _arrowAngleMultiplier; 
        /*
        else
            angle = angle * (1 + ratio); 
        */
        return angle; 
    }

}
