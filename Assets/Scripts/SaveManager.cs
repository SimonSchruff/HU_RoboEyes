using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using UnityEngine.UI; 

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance; 

    public struct SettingsData
    {
        public float TimeToMoveToSquare; 
        public float TimeToRecover; 
        public float BreakTime; 
        public float StartTimeDelay; 
        public GameManager.PointerType PointerType;  
        public float ArrowAngleMultiplier; 
        public bool LookAtArmNullPos; 
        public int[] Sequence; 
    }
    public SettingsData settingsData; 

    public TMP_InputField IF_TimeToMoveToSquare; 
    public TMP_InputField IF_TimeToRecover; 
    public TMP_InputField IF_BreakTime; 
    public TMP_InputField IF_StartTimeDelay; 
    public TMP_Dropdown DD_PointerType; 
    public TMP_InputField IF_ArrowMulitplier; 
    public Toggle T_LookAtArm; 
    public TMP_InputField[] IF_Sequence = new TMP_InputField[10]; 
    public SliderHelperScript SequenceSlider; 
    private Slider _slider; 
    private int _value; 

    private void Awake()
    { 
        if(instance != null)
            Destroy(this);  
        else
            instance = this; 

        DontDestroyOnLoad(gameObject); 
    }
    private void Start()
    {
        // Handle all Slider References and Set InputFields interactable
        _slider = SequenceSlider.GetComponentInChildren<Slider>(); 
        _slider.onValueChanged.AddListener(delegate {ToggleActiveInputFields(); });
        ToggleActiveInputFields(); 
    }

    /// <summary> 
    /// Saves all Settings Data into settingsData struct on Button Click
    /// </summary>
    public void SaveSettings()
    {
        settingsData = new SettingsData(); 

        //Initialize Array;
        settingsData.Sequence = new int[_value]; 

        settingsData.TimeToMoveToSquare =  float.Parse(IF_TimeToMoveToSquare.text); 
        settingsData.TimeToRecover =  float.Parse(IF_TimeToRecover.text); 
        settingsData.BreakTime =  float.Parse(IF_BreakTime.text); 
        settingsData.StartTimeDelay = float.Parse(IF_StartTimeDelay.text); 

        settingsData.ArrowAngleMultiplier = float.Parse(IF_ArrowMulitplier.text); 
        
        if(T_LookAtArm.isOn)
            settingsData.LookAtArmNullPos = true; 
        else
            settingsData.LookAtArmNullPos = false; 

        
        if(DD_PointerType.captionText.text == "Arrow")
        {
            settingsData.PointerType = GameManager.PointerType.arrow; 
            //print(settingsData.PointerType); 
        }
        else if(DD_PointerType.captionText.text == "Eye")
        {
            settingsData.PointerType = GameManager.PointerType.eye; 
            //print(settingsData.PointerType); 
        }

        for(int i = 0; i < _value; i++)
        {
            settingsData.Sequence[i] = int.Parse( IF_Sequence[i].text); 
            //print("Sequence: " + i + " Value: " + settingsData.Sequence[i]); 
        }
    }

    /// <summary> 
    /// Sets value according to slider and toggles the interactability of Input Fields
    /// </summary>
    void ToggleActiveInputFields()
    {
        _value = SequenceSlider.Value; 
        for(int i = 0; i < IF_Sequence.Length; i++ )
        {
            if(i < _value)
            {
                IF_Sequence[i].interactable = true; 
            }
            else 
            {
                IF_Sequence[i].interactable = false; 
            }
        }
    }
}
