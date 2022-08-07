using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleUIAnimation : MonoBehaviour
{
    public float fps = 12f;
    public Sprite[] sprites;
    private int index = 0;
    public Image image;

    private float timePerFrame;
    private float elapsedTime = 0f;

    private void Start()
    {
        timePerFrame = 1f / fps;    
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime > timePerFrame)
        {
            elapsedTime -= timePerFrame;
            AdvanceFrame();
        }
    }

    private void AdvanceFrame()
    {
        index++;
        if(index >= sprites.Length)
        {
            index = 0;
        }
        image.sprite = sprites[index];  
    }
}
