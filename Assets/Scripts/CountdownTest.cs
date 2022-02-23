using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownTest : MonoBehaviour
{
    public TextMeshProUGUI TimerText; 
    private float _timer; 
    private int _timerValue = 0; 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime; 

        if(_timer >= 1f)
        {
            _timer = 0f; 
            _timerValue++; 
            TimerText.text = _timerValue.ToString(); 
        }

    }
}
