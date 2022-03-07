using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows; 
using System; 
public class ScreenCapture : MonoBehaviour
{
    public static ScreenCapture instance; 

    public int TargetFPS = 25; 
    public bool IsRecording = false; 
    private int _width; 
    private int _height; 
    public string FilePath; 
    public string VideoName;

    private string _day; 
    private string _month; 
    private string _minute; 
    private string _hour; 
    private string _seconds; 

    private int _frame = 0; 


    void Awake()
    {
        if(instance != null)
            Destroy(this);  
        else
            instance = this;  

        DontDestroyOnLoad(gameObject); 
    }

    private void Start() 
    {
        Application.targetFrameRate = TargetFPS; 

        _width = Screen.width;
        _height = Screen.height;
        //print("Screen Dimensions; Width: " + _width + " Height: " + _height); 

        /*
        DateTime currentTime = DateTime.Now; 
        _day = currentTime.Day.ToString(); 
        _month = currentTime.Month.ToString(); 
        _hour = currentTime.Hour.ToString(); 
        _minute = currentTime.Minute.ToString(); 

        _filePath = Application.dataPath + "/Video-" + _day + "." + _month + "_" + _hour + "-" + _minute;  
        // Application.dataPath + "/Video-"; 
        */
        // returns a DirectoryInfo object
    }

    private void Update()
    {
        if(IsRecording)
            StartCoroutine(GetFramePNG()); 
    }


    public IEnumerator GetFramePNG()
    {   
        _frame++; 
        // We should only read the screen buffer after rendering is complete
        yield return new WaitForEndOfFrame();

        // Create a texture the size of the screen, RGB24 format
        Texture2D tex = new Texture2D(_width, _height, TextureFormat.RGB24, false);

        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, _width, _height), 0, 0);
        tex.Apply();

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        UnityEngine.Object.Destroy(tex);

        // Names JPGs for FFMPEG Video Conversion
        string fileName;  
        if(_frame < 10 )
            fileName = "0000"+_frame.ToString(); 
        else if(_frame >= 10 && _frame < 100)
            fileName = "000"+_frame.ToString(); 
        else if(_frame >= 100 && _frame < 1000)
            fileName = "00"+_frame.ToString(); 
        else if(_frame >= 1000 && _frame < 10000)
            fileName = "0"+_frame.ToString(); 
        else if(_frame >= 10000)
            fileName = _frame.ToString(); 
        else
            fileName = _frame.ToString(); // Causes Error in Video conversion but 99k Frames should be enough; 
             
        //File.WriteAllBytes(_filePath +"/"+fileName+".png", bytes);
        System.IO.File.WriteAllBytes(FilePath + "/" + VideoName + "/" +fileName+".png", bytes); 
    }

    public void ToggleIsRecording()
    {
        IsRecording = !IsRecording; 
    }
}
