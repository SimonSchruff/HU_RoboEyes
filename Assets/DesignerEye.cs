using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesignerEye : MonoBehaviour
{
    public GameObject BackgroundCircle; 
    public GameObject Circle; 
    public GameObject Pupil; 
    private GameManager.PointerType _pointerType; 
    private Vector3 _eyeCenterPos; 
    private float _radius; 
    private float _pupilRadius; 
    private float _circleRadius; 


    // Temp Debug Vars
    public Vector3 square0Pos = new Vector3(-3.2f,0f, 2.15f); 

    void Start()
    { 
       // _pointerType = GameManager.instance.pointerType; 

        _eyeCenterPos = transform.position; 
        // Calculate Radius of Eye Part; Values come from Illustrator File
        _radius = BackgroundCircle.GetComponent<SpriteRenderer>().bounds.size.x / 2; 
        _circleRadius = _radius * 0.847431f; // 826.5px/975.3px
        _pupilRadius = _radius * 0.463754f; //  452.3px/975.3px
        print("Radius: " + _radius + "; Pupil Radius: " + _pupilRadius + "; InnerCircle Radius: " + _circleRadius); 

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
        Vector3 direction = new Vector3(square.transform.position.x - transform.position.x, square.transform.position.y - transform.position.y, 0 );
        //print("dir Vector: "+ directionVector); 

        // zValue increases the further square is away -> ratio becomes closer to 0 -> pupil stays closer to center
        float zValue = (square.transform.position.z < 1 ) ? zValue = 1 : zValue = square.transform.position.z; 
        float ratio =  1 / Mathf.Abs(zValue); 

        MovePupil(direction, ratio); 
        MoveCircle(direction, ratio); 

        yield return null; 
    }

    private void MovePupil(Vector3 dir, float ratio)
    {
         // Calculates the vector pupil is supposed to be moved
        Vector3 newPupilPos = new Vector3(Pupil.transform.position.x + (dir.x * ratio), Pupil.transform.position.y + (dir.y * ratio), 0); 

        // Calculate wether or not pupil is within _radius of the eye 
        float distToPupil = Vector3.Distance(newPupilPos, _eyeCenterPos); 
        if(distToPupil > (_radius - _pupilRadius))
        {
            // Calculate the Vector from Eye Center Pos to Pupil Center Pos
            Vector3 fromEyeToPupil = newPupilPos - _eyeCenterPos; 
            // Multiply Vector with radius - pupilRadius and divide it by distance
            fromEyeToPupil *= (_radius - _pupilRadius) / distToPupil; 
            Pupil.transform.position = fromEyeToPupil + _eyeCenterPos; 
        }
        else
        {
            Pupil.transform.position = newPupilPos; 
        }
    }

    private void MoveCircle(Vector3 dir, float ratio)
    {
         // Calculates the vector circle is supposed to be moved
        Vector3 newCirclePos = new Vector3(Circle.transform.position.x + (dir.x * ratio), Circle.transform.position.y + (dir.y * ratio), 0); 

        // Calculate wether or not circle is within _radius of the eye 
        float distToCircle = Vector3.Distance(newCirclePos, _eyeCenterPos); 
        if(distToCircle > (_radius - _circleRadius))
        {
            // Calculate the Vector from Eye Center Pos to Pupil Center Pos
            Vector3 fromEyeToCircle = newCirclePos - _eyeCenterPos; 
            // Multiply Vector with radius - pupilRadius and divide it by distance
            fromEyeToCircle *= (_radius - _circleRadius) / distToCircle; 
            Circle.transform.position = fromEyeToCircle + _eyeCenterPos; 
        }
        else
        {
            Circle.transform.position = newCirclePos; 
        }
    }
}
