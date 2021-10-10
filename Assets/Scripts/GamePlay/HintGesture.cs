using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;



public class HintGesture : MonoBehaviour
{
    public GameObject Gesture1;
    public GameObject Gesture2;
    public GameObject Gesture3;

    public TextureData textureData;

    private void Start()
    {
        ShowNormalGestures();
    }


    void ShowNormalGestures()
    {
        Gesture1.transform.GetComponent<Image>().sprite = textureData.FindGesture("ThumbsUp") as Sprite;
        Gesture2.transform.GetComponent<Image>().sprite = textureData.FindGesture("ThumbsDown") as Sprite;
        Gesture3.transform.GetComponent<Image>().sprite = textureData.FindGesture("RaisedHand") as Sprite;
    }

    public void ShowHintGestureUI(bool IsShow)
    {
        this.gameObject.SetActive(IsShow);
    }

}
