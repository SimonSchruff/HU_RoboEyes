using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using System; 

public class FollowSequenceEyes : MonoBehaviour
{
    [Header("Variables")]
    public bool IsLeftEye; 
    public bool NextSequenceRdy = true; 

    [SerializeField]
    private Vector3 m_initRot; 

    // TO-DO: 
    // - Create Dictionary for Angles so it does not have to be calculated twice

    
    public GameManager.PointerType pointerType; 

    private void Start() 
    {
        pointerType = GameManager.instance.pointerType;     
    }


    public IEnumerator Sequence(GameManager.Square square)
    {
        
        transform.LookAt(square.transform.position); 
        Vector3 targetRot = transform.eulerAngles; 
        //transform.eulerAngles = m_initRot; 
        
        
        //Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRot), .001f); 
        print("Look at Rotation: " + targetRot); 

        if(pointerType == GameManager.PointerType.coneEye)
        {
            // Modify Rotation value for cone eyes
            // Maybe also ball eyes
        }
        


        
        // Workaround for shitty texture        
        yield return new WaitForSeconds(GameManager.instance.TimeToMoveToSquare);

        // Robo has reached square
        transform.eulerAngles = m_initRot; 
        //DebugText.text = ("Robo Arm is moving to default position"); 

        //yield return new WaitForSeconds(GameManager.instance.TimeToRecover); 

        //NextSequenceRdy = true; 
    }


    /*
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
    */


}
