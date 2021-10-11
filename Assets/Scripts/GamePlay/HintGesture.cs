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



    public GameObject Gesture1;
    public GameObject Gesture2;
    public GameObject Gesture3;
    public GameObject Model;
    public GameObject ContinueHintUI;

    public TextureData textureData;
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


    public void ShowNormalGestures()
    {
        Gesture1.transform.GetComponent<Image>().sprite = textureData.FindGesture("ThumbsUp") as Sprite;
        Gesture2.transform.GetComponent<Image>().sprite = textureData.FindGesture("ThumbsDown") as Sprite;
        Gesture3.transform.GetComponent<Image>().sprite = textureData.FindGesture("RaisedHand") as Sprite;
    }

    public void ShowHintGestureUI(bool IsShow)
    {
        Model.SetActive(IsShow);
        //instance.gameObject.SetActive(IsShow);
    }

    public void ShowHintContinueHintUI(bool IsShow)
    {
        ContinueHintUI.SetActive(IsShow);
    }

}
