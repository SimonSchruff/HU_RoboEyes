using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesignerEye : MonoBehaviour
{
    public GameObject BackgroundCircle; 
    public GameObject InnerCircle; 
    public GameObject Pupil; 
    private GameManager.PointerType _pointerType; 
    private float _radius; 
    private float _pupilRadius; 
    private float _innerCircleRadius; 


    // Temp Debug Vars
    public Vector3 square0Pos = new Vector3(-3.2f,0f, 2.15f); 

    void Start()
    { 
       // _pointerType = GameManager.instance.pointerType; 

        // Calculate Radius of Eye Part; Values come from Illustrator File
        _radius = BackgroundCircle.GetComponent<SpriteRenderer>().bounds.size.x; 
        _innerCircleRadius = _radius * 0.847431f; 
        _pupilRadius = _radius * 0.463754f; 
        //print("Radius: " + _radius + "; Pupil Radius: " + _pupilRadius + "; InnerCircle Radius: " + _innerCircleRadius); 

        StartCoroutine(Sequence()); 

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Sequence()
    {   // TO DO : 
        //          - Calculate Clamp Values according to radius of eye parts
        //          - Do the same for inner circle

        // Only for Testing new eye 
        GameObject square = new GameObject(); 
        square.transform.position = square0Pos; 

        // Calculates direction towards target Square
        Vector3 directionVector = new Vector3(square.transform.position.x - transform.position.x, square.transform.position.y - transform.position.y, 0 );
        print("dir Vector: "+ directionVector); 

        // zValue increases the further square is away -> ratio becomes closer to 0 -> pupil stays closer to center
        float zValue = (square.transform.position.z < 1 ) ? zValue = 1 : zValue = square.transform.position.z; 
        float ratio =  1 / Mathf.Abs(zValue); 

        // Calculates the vector pupil is supposed to be moved and moves it
        Vector3 pupilMoveVector = new Vector3(Pupil.transform.localPosition.x + (directionVector.x * ratio), Pupil.transform.localPosition.y + (directionVector.y * ratio), 0); 
        Pupil.transform.localPosition = new Vector3(Mathf.Clamp(pupilMoveVector.x, - 2.5f,  2.5f),  Mathf.Clamp(pupilMoveVector.y, - 2.5f,  2.5f), 0); 

        Vector3 circleMoveVector = new Vector3(InnerCircle.transform.localPosition.x + (directionVector.x * ratio), InnerCircle.transform.localPosition.y + (directionVector.y * ratio), 0); 
        InnerCircle.transform.localPosition = new Vector3(Mathf.Clamp(circleMoveVector.x, - .6f,  .6f),  Mathf.Clamp(circleMoveVector.y, - .6f,  .6f), 0); 

        
        /*
        // NEEDS IMPLIMENTATION
        // Calculate wether or not pupil is within _radius bounds 
        Vector3 eyeCenterPos = transform.localPosition; 
        float pupilDistance = Vector3.Distance(Pupil.transform.position, eyeCenterPos); 

        if(pupilDistance > _radius)
        {
            Vector3 fromOriginToPupil = Pupil.transform.position - eyeCenterPos; 
            fromOriginToPupil *= _radius / pupilDistance; 

            Pupil.transform.position = eyeCenterPos + fromOriginToPupil; 
        }
        */

       
        yield return null; 
    }
}
