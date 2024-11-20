using CORE.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCurrentImage : MonoBehaviour
{

    [SerializeField]
    private PlaneGameController gameController;


    public RawImage rawImage;


    // Start is called before the first frame update
    void Start()
    {
        DisplayImage();
    }

    public void DisplayImage()
    {
        string randoWord = gameController.CurrentWord();
        Texture2D randoImg = ImageManager.GetImageFromWord(randoWord);
        GetImage(randoImg);
    }


    public void GetImage(Texture2D textureImg)
    {
        rawImage.texture = textureImg;
    }
}
