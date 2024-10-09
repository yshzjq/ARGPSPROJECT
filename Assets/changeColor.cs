using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class changeColor : MonoBehaviour
{
    public Image[] images;
    public TextMeshProUGUI text;

    private Color[] whiteColor;

    private bool isRed = false;
    public bool changeRed = false;
    private void OnEnable()
    {
        whiteColor = new Color[images.Length];

        for (int i = 0; i < images.Length; i++)
        {
            whiteColor[i] = images[i].color;
        }

        if (changeRed == true)
        {
            Red();
        }
    }


    public void Red()
    {
        if (isRed == false)
        {
            for (int i = 0; i < whiteColor.Length; i++)
            {
                images[i].color = new Color(1f, 0.5f, 0.5f, 0.5f);
            }
            text.color = new Color(1f,0.5f,0.5f,0.5f);

            isRed = true;
        }
    }
    public void White()
    {
        if (isRed == true)
        {
            for(int i = 0;i < whiteColor.Length;i++)
            {
                images[i].color = whiteColor[i];
            }
            text.color = Color.white;

            isRed = false;
        }
    }
}
