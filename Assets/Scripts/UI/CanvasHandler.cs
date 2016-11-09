using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CanvasHandler : MonoBehaviour 
{
    public static CanvasHandler main = null;
    
    public CanvasScaler CanvasScaler { get; private set; }
    public Canvas Canvas { get; private set; }

    void Awake()
    {
        if (name.Equals("CanvasMain"))
        {
            main = this;
            Canvas = GetComponent<Canvas>();
            CanvasScaler = GetComponent<CanvasScaler>();
        }
    }

    void Start()
    { 
        if (Screen.width > 1136)
        {
            CanvasScaler = CanvasScaler ?? GetComponent<CanvasScaler>();
            float k = Screen.width / 1136f;
            CanvasScaler.scaleFactor *= k;
        }
    }

}
