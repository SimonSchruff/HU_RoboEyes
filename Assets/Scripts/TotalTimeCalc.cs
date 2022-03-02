using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class TotalTimeCalc : MonoBehaviour
{
    public TMP_InputField TimeToMove; 
    public TMP_InputField TimeToRecover; 
    public TMP_InputField BreakTime; 
    private TMP_InputField _totalTime; 

    private void Start() 
    {
        _totalTime = GetComponentInChildren<TMP_InputField>(); 
        if(_totalTime == null) {print("total Time Field not set!");}
        CalculateTotalTime(); 
    }

    public async void CalculateTotalTime()
    {   
        float t1 = 0; float t2 = 0; float t3 = 0; 

        // Add up all times to get total time; 
        if(float.TryParse(TimeToMove.text, out t1) && float.TryParse(TimeToRecover.text, out t2) && float.TryParse(BreakTime.text, out t3))
        {
            _totalTime.text = (t1+t2+t3).ToString(); 
        }
        else
        {
            print("Parse Failed!"); 
        }
    }
}
