using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class SliderHelperScript : MonoBehaviour
{
    public int Value; 
    [SerializeField] private TMP_Text _valueText; 
    [SerializeField] private Slider _slider; 

    private void Start() 
    {
        UpdateText(); 
        _slider.onValueChanged.AddListener(delegate {UpdateText(); });
    }

    public void UpdateText()
    {
        Value =  (int)_slider.value; 
        _valueText.text = Value.ToString(); 
    }
}
