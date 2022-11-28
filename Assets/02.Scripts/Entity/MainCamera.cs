using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    const int fixedWidth = 1920;
    const int foxedHeight = 1080;
    private void Start()
    {
        SetResolution(); 
    }

    public void SetResolution()
    {
        int deviceWidth = Screen.width; 
        int deviceHeight = Screen.height; 

        Screen.SetResolution(fixedWidth, (int)(((float)deviceHeight / deviceWidth) * foxedHeight), true); 

        if ((float)fixedWidth / foxedHeight < (float)deviceWidth / deviceHeight) 
        {
            float newWidth = ((float)fixedWidth / foxedHeight) / ((float)deviceWidth / deviceHeight);
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); 
        }
        else 
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)fixedWidth / foxedHeight);
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); 
        }
    }
}
