using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance; 

    public struct SettingsData
    {
        public float TimeToMoveToSquare; 
        public float TimeToRecover; 
        public float BreakTime; 
        public GameManager.PointerType PointerType;  
        public float ArrowAngleMultiplier; 
        public int[] Sequence; 
    }
    public SettingsData settingsData = new SettingsData(); 

    public TMP_InputField IF_TimeToMoveToSquare; 
    public TMP_InputField IF_TimeToRecover; 
    public TMP_InputField IF_BreakTime; 
    public TMP_Dropdown DD_PointerType; 
    public TMP_InputField IF_ArrowMulitplier; 
    public TMP_InputField[] IF_Sequence = new TMP_InputField[10]; 

    private void Awake()
    { 
        if(instance != null)
            Destroy(this);  
        else
            instance = this; 

        DontDestroyOnLoad(gameObject); 
    }

    public void SaveSettings()
    {
        //Initialize Array;
        settingsData.Sequence = new int[IF_Sequence.Length]; 

        settingsData.TimeToMoveToSquare =  float.Parse(IF_TimeToMoveToSquare.text); 
        settingsData.TimeToRecover =  float.Parse(IF_TimeToRecover.text); 
        settingsData.BreakTime =  float.Parse(IF_BreakTime.text); 

        settingsData.ArrowAngleMultiplier = float.Parse(IF_ArrowMulitplier.text); 
        
        if(DD_PointerType.captionText.text == "Arrow")
        {
            settingsData.PointerType = GameManager.PointerType.arrow; 
            //print(settingsData.PointerType); 
        }
        else if(DD_PointerType.captionText.text == "Eye")
        {
            settingsData.PointerType = GameManager.PointerType.flatEye; 
            //print(settingsData.PointerType); 
        }

        for(int i = 0; i < IF_Sequence.Length; i++)
        {
            settingsData.Sequence[i] = int.Parse( IF_Sequence[i].text); 
            //print("Sequence: " + i + " Value: " + settingsData.Sequence[i]); 
        }

        
    }
}
