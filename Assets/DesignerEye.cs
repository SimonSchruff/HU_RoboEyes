using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesignerEye : MonoBehaviour
{
    public GameObject InnerCircle; 
    public GameObject Pupil; 
    private GameManager.PointerType _pointerType; 
    private float _radius = 10f; 
    private float _pupilRadius = 2f; 


    // Temp Debug Vars
    private Vector3 square0Pos = new Vector3(-3.2f,0f, 2.15f); 

    void Start()
    { 
       // _pointerType = GameManager.instance.pointerType; 
        StartCoroutine(Sequence()); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Sequence()
    {
        // Only for Testing new eye 
        GameObject square = new GameObject(); 
        square.transform.position = square0Pos; 

        Vector3 targetVector = new Vector3(square.transform.position.x - transform.position.x, square.transform.position.y - transform.position.y, 0 ).normalized;
        //print(targetVector); 

        float zValue = square.transform.position.z; 

        float ratio = 1 / zValue; // zValue is increases the further square is away -> ratio becomes closer to 0 -> pupil stays closer to center
        //print(ratio); 

        Vector3 pupilVector = new Vector3(transform.position.x + targetVector.x * ratio, transform.position.y + targetVector.y * ratio, transform.position.z); 
        Pupil.transform.localPosition = new Vector3(Mathf.Clamp(pupilVector.x, - 2.5f,  2.5f), Mathf.Clamp(pupilVector.y, - 2.5f,  2.5f), 0); 
        // TO DO : 
        //          - Calculate Clamp Values according to radius of eye parts
        //          - Fix Direction Issues -> +/- of Vectors
        //          - Do the same for inner circle
       
        yield return null; 
    }
}
