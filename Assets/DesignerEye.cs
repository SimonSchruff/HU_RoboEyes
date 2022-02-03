using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesignerEye : MonoBehaviour
{
    [Header("Object References")]
    public GameObject BackgroundCircle; 
    public GameObject Circle; 
    public GameObject Pupil; 
    public GameObject Line;

    #region privateVars
    private Vector3 _eyeCenterPos; 
    private float _radius; 
    private float _pupilRadius; 
    private float _circleRadius; 

    // Anchors
    private Vector3 _eyeTopAnchor; 
    private Vector3 _eyeBotAnchor; 
    private Vector3 _eyeLeftAnchor; 
    private Vector3 _eyeRightAnchor; 

    #endregion

    void Start()
    { 
        _eyeCenterPos = transform.position; 
        // Calculate Radius of Eye Part; Values come from Illustrator File
        _radius = BackgroundCircle.GetComponent<SpriteRenderer>().bounds.size.x / 2; 
        _circleRadius = _radius * 0.847431f; // 826.5px/975.3px
        _pupilRadius = _radius * 0.463754f; //  452.3px/975.3px
        //print("Radius: " + _radius + "; Pupil Radius: " + _pupilRadius + "; InnerCircle Radius: " + _circleRadius); 

        // Set Anchors for all sides -> Probably should Array later
        _eyeTopAnchor = _eyeCenterPos + new Vector3(0, _radius ,0); 
    }

    public IEnumerator Sequence(GameManager.Square square)
    {   
        // Calculates direction towards target Square
        Vector3 direction = new Vector3(square.transform.position.x - transform.position.x, square.transform.position.y - transform.position.y, 0 );
        //print("dir Vector: "+ directionVector); 

        // zValue increases the further square is away -> ratio becomes closer to 0 -> pupil stays closer to center
        float zValue = ( Mathf.Abs(square.transform.position.z) < 1 ) ? zValue = 1 : zValue = Mathf.Abs(square.transform.position.z); 
        float ratio =  1 / zValue; 
        print(ratio); 

        MovePupil(direction, ratio); 
        MoveCircle(direction, ratio); 
        SetLines(); 
        
        yield return new WaitForSeconds(GameManager.instance.TimeToMoveToSquare);

        ResetEye(); 
    }

    //// WIP ////
    private void SetLines()
    {
        // Position of Anchors for Lines
        Vector3 pupilTopAnchor = Pupil.transform.position + new Vector3(0, _pupilRadius ,0); 

        Vector3 eyeAncToPupilAnc = new Vector3(_eyeTopAnchor.x - pupilTopAnchor.x, _eyeTopAnchor.y - pupilTopAnchor.y, 0 ); 
        
        
        
        Line.transform.eulerAngles = new Vector3(0,0,-Mathf.Rad2Deg * Mathf.Atan((pupilTopAnchor.x - _eyeTopAnchor.x)/(pupilTopAnchor.y - _eyeTopAnchor.y)));
        Line.transform.position = _eyeTopAnchor; 
        // SET SCALE OF LINE // WIP
        //Line.transform.localScale = new Vector3(0,0,0); 
        

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

    private void ResetEye()
    {
        Pupil.transform.localPosition = Vector3.zero; 
        Circle.transform.localPosition = Vector3.zero; 

        Line.transform.eulerAngles = Vector3.zero;
        Line.transform.position = _eyeTopAnchor; 

    }
}
