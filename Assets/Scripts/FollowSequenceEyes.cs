using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using System; 

public class FollowSequenceEyes : MonoBehaviour
{
    [Header("Variables")]
    public float TimeToMoveToSquare; 
    public float TimeToRecover; 
    public bool IsLeftEye; 
    public bool NextSequenceRdy = true; 


    [Header("References")]
    public Sprite ArrowSprite; 
    public Sprite ArrowInactiveSprite; 
    public TextMeshProUGUI DebugText; 


    private int m_currentSquare = 0;
    private Vector3 m_ArrowPosOffset;  
    private Vector3 m_targetPos; 

    // TO-DO: 
    // - Create Dictionary for Angles so it does not have to be calculated twice

    void Start()
    {

        //DebugText.text = null; 
        
    }

    void Update()
    {
       
    }

    public IEnumerator Sequence(GameManager.Square square)
    {
        

        transform.LookAt(square.transform.position); 
        // Workaround for shitty texture        
        yield return new WaitForSeconds(GameManager.instance.TimeToMoveToSquare);

        // Robo has reached square
        transform.eulerAngles = new Vector3(0,180,0); 
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
        
        if(!IsLeftEye)
        {
            if(square.ID == 1 || square.ID == 4)
                angle = -angle; 
        }

        return angle; 


    }

}
