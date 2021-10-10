using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;



public class HintGesture : MonoBehaviour
{
    static HintGesture instance;
    public static HintGesture Instance => instance;



    public static GameObject Gesture1;
    public static GameObject Gesture2;
    public static GameObject Gesture3;

    public static TextureData textureData;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }
    private void Start()
    {
        ShowNormalGestures();
    }


    public static void ShowNormalGestures()
    {
        Gesture1.transform.GetComponent<Image>().sprite = textureData.FindGesture("ThumbsUp") as Sprite;
        Gesture2.transform.GetComponent<Image>().sprite = textureData.FindGesture("ThumbsDown") as Sprite;
        Gesture3.transform.GetComponent<Image>().sprite = textureData.FindGesture("RaisedHand") as Sprite;
    }

    public static void ShowHintGestureUI(bool IsShow)
    {
        instance.gameObject.SetActive(IsShow);
    }

}
