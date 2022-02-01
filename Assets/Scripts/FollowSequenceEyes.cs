using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using System; 

public class FollowSequenceEyes : MonoBehaviour
{
    [Header("Variables")]
    public float slerpSpeed = 1f; 
    public bool IsLeftEye; 
    public bool NextSequenceRdy = true; 

    [SerializeField]
    private Vector3 m_initRot; 
    private Vector3 m_playerPos = new Vector3(0,7f,-50f); 

    // TO-DO: 
    // - Create Dictionary for Angles so it does not have to be calculated twice

    
    public GameManager.PointerType pointerType; 

    private void Start() 
    {
        pointerType = GameManager.instance.pointerType;     
    }


    public IEnumerator Sequence(GameManager.Square square)
    {
        StartCoroutine(SmoothLookAt(square.transform.position)); 
        
        yield return new WaitForSeconds(GameManager.instance.TimeToMoveToSquare);

        // Robo has reached square
        //transform.eulerAngles = m_initRot; 
        StartCoroutine(SmoothLookAt(m_playerPos)); 
    }


    private IEnumerator SmoothLookAt(Vector3 position)
    {
        Vector3 targetVector = position - transform.position; 
        Quaternion rot = Quaternion.LookRotation(targetVector, Vector3.up);


        Vector3 rotEuler = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, rot.eulerAngles.z); 
        Vector3 eulerClamped = Vector3.zero; 
        if(rotEuler.x != 0)
        {
            eulerClamped = new Vector3( Mathf.Clamp(rotEuler.x, -9f, 9f), Mathf.Clamp(rotEuler.y, 170f , 190f), 0); 
        }     
        else
        {
            eulerClamped = new Vector3( Mathf.Clamp(rotEuler.x, -13f, 13f), Mathf.Clamp(rotEuler.y, 167f , 193f), 0); 
        }   
        Quaternion rotClmaped = Quaternion.Euler(eulerClamped); 

        float time = 0f; 
        while(time < 1)
        {
            transform.localRotation = Quaternion.Slerp(transform.rotation, rotClmaped, time ); 
            time += Time.deltaTime * slerpSpeed; 
            yield return null; 
        }
        print("Look at Rotation: " + rot.eulerAngles); 
    }


}
