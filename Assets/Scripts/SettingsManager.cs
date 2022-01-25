using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class SettingsManager : MonoBehaviour
{
    public TextAsset settingsJSON; 

    [Serializable]
    public class Settings
    {
       public PositionVariables posVars; 
    }

    [Serializable]
    public class PositionVariables
    {
        public float Height; 
        public float DistanceToTable; 
        public float DistanceToFirstRow; 
        public float DistanceToSecondRow; 
        public float XDistanceBetweenSquares; 
        public float ZDistanceBetweenSquares; 
        public float TabletSizeX; 
        public float TabletSizeY; 
    }


    
    void Start()
    {
        Settings mySettings = new Settings(); 
        mySettings = JsonUtility.FromJson<Settings>(settingsJSON.text); 

        print(mySettings.posVars.Height); 




    }

    void Update()
    {
        
    }
}
