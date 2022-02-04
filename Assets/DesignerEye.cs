using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesignerEye : MonoBehaviour
{
    [Header("Object References")]
    public GameObject BackgroundCircle; 
    public GameObject Circle; 
    public GameObject Pupil; 
    public GameObject[] Lines = new GameObject[4]; 

    #region privateVars
    private Vector3 _eyeCenterPos; 
    private float _radius; 
    private float _pupilRadius; 
    private float _circleRadius; 
    private Vector3[] _eyeAnchors = new Vector3[4]; 
    private Vector3[] _pupilAnchors = new Vector3[4]; 

    #endregion

    void Start()
    { 
        _eyeCenterPos = transform.position; 
        // Calculate Radius of Eye Part; Values come from Illustrator File
        _radius = ( BackgroundCircle.GetComponent<SpriteRenderer>().bounds.size.x / 2 ) - 0.03f;  // Offset bc Sprite Size is not exactly size of circle in-game
        _circleRadius = _radius * 0.847431f; // 826.5px/975.3px
        _pupilRadius = _radius * 0.463754f; //  452.3px/975.3px

        // Set Eye Anchor Positions; Top, Bot, Left, Right
        _eyeAnchors[0] = _eyeCenterPos + new Vector3(0, _radius,0);
        _eyeAnchors[1] = _eyeCenterPos - new Vector3(0, _radius,0); 
        _eyeAnchors[2] = _eyeCenterPos - new Vector3(_radius, 0 , 0); 
        _eyeAnchors[3] = _eyeCenterPos + new Vector3(_radius, 0 , 0); 
    }

    /// <summary> 
    /// Called by Game Manager with square to look at
    /// Called once per time that it takes to look at square and go back
    /// </summary>
    public IEnumerator Sequence(GameManager.Square square)
    {   
        LookAt(square.transform.position); 

        yield return new WaitForSeconds(GameManager.instance.TimeToMoveToSquare);

        ResetEye(); 
    }

    /// <summary> 
    /// Moves pupil, inner circle and sets lines according to target position
    /// </summary>
    private void LookAt(Vector3 targetPos)
    {
        // Calculates direction towards target Square
        Vector3 direction = new Vector3(targetPos.x - transform.position.x, targetPos.y - transform.position.y, 0 );

        // zValue increases the further square is away -> ratio becomes closer to 0 -> pupil stays closer to center
        float zValue = ( Mathf.Abs(targetPos.z) < 1 ) ? 1 : Mathf.Abs(targetPos.z); 
        float ratio =  1 / zValue; 

        #region Move Pupil
        // Calculates the vector pupil is supposed to be moved
        Vector3 newPupilPos = new Vector3(Pupil.transform.position.x + (direction.x * ratio), Pupil.transform.position.y + (direction.y * ratio), 0);  

        // Calculate wether or not pupil is within _radius of the eye 
        float distToPupil = Vector3.Distance(newPupilPos, _eyeCenterPos); 

        if(distToPupil > (_radius - _pupilRadius)) // Pupil is NOT within the eye
        {   
            // Calculate the Vector from Eye Center Pos to Pupil Center Pos
            Vector3 fromEyeToPupil = newPupilPos - _eyeCenterPos; 
            // Multiply Vector with radius - pupilRadius and divide it by distance to get max distance pupil can be moved
            fromEyeToPupil *= (_radius - _pupilRadius) / distToPupil; 
            Pupil.transform.position = fromEyeToPupil + _eyeCenterPos; 
        }
        else // Pupil is within eye
        {
            Pupil.transform.position = newPupilPos; 
        }
        #endregion

        #region Set Lines
        // Set Pupil Anchors to be used by Lines; Top, Bot, Left, Right
        _pupilAnchors[0] = Pupil.transform.position + new Vector3(0, _pupilRadius - 0.05f, 0);
        _pupilAnchors[1] = Pupil.transform.position - new Vector3(0, _pupilRadius - 0.05f, 0);
        _pupilAnchors[2] = Pupil.transform.position - new Vector3( _pupilRadius - 0.05f, 0 , 0);
        _pupilAnchors[3] = Pupil.transform.position + new Vector3( _pupilRadius - 0.05f, 0 , 0);

        for(int i = 0; i < Lines.Length; i++)
        {
            Vector3 fromEyeAncToPupilAnc = new Vector3( _pupilAnchors[i].x - _eyeAnchors[i].x , _pupilAnchors[i].y - _eyeAnchors[i].y , 0 ); 

            if(i < 2) // Vertical Lines
            {
                Lines[i].transform.eulerAngles = new Vector3(0,0, -Mathf.Rad2Deg * Mathf.Atan(fromEyeAncToPupilAnc.x / fromEyeAncToPupilAnc.y)); 
                Lines[i].transform.localScale = new Vector3(1, fromEyeAncToPupilAnc.magnitude, 1); 
            }
            else // Horizontal Lines
            {
                Lines[i].transform.eulerAngles = new Vector3(0,0, Mathf.Rad2Deg * Mathf.Atan( fromEyeAncToPupilAnc.y / fromEyeAncToPupilAnc.x )); 
                Lines[i].transform.localScale = new Vector3(fromEyeAncToPupilAnc.magnitude, 1 , 1); 
            }
        }
        #endregion

        #region Move inner Circle
        // Calculates the vector circle is supposed to be moved
        Vector3 circlePos = new Vector3(Circle.transform.position.x + (direction.x * ratio), Circle.transform.position.y + (direction.y * ratio), 0); 

        // Calculate wether or not circle is within radius of the eye 
        float distToCircle = Vector3.Distance(circlePos, _eyeCenterPos); 

        if(distToCircle > (_radius - _circleRadius)) // Circle is NOT within radius of eye
        {   
            // Calculate the Vector from Eye Center Pos to Pupil Center Pos
            Vector3 fromEyeToCircle = circlePos - _eyeCenterPos; 
            // Multiply Vector with radius - circleRadius and divide it by distance to get max distance circle can be moved
            fromEyeToCircle *= (_radius - _circleRadius) / distToCircle; 
            Circle.transform.position = fromEyeToCircle + _eyeCenterPos; 
        }
        else // Circle is within eye
        {
            Circle.transform.position = circlePos; 
        }
        #endregion
    }

    /// <summary> 
    /// Resets the eye to the original position
    /// </summary>
    private void ResetEye()
    {
        Pupil.transform.localPosition = Vector3.zero; 
        Circle.transform.localPosition = Vector3.zero; 

        for(int i = 0; i < Lines.Length; i++)
        {
            if( i < 2 ) // Vertical Lines
            {
                Lines[i].transform.eulerAngles = Vector3.zero; 
                Lines[i].transform.localScale = new Vector3(1,0.35f,1); 
            }
            else // Horizontal Lines
            {
                Lines[i].transform.eulerAngles = Vector3.zero; 
                Lines[i].transform.localScale = new Vector3(0.35f,1,1); 
            }
        }
    }
}
