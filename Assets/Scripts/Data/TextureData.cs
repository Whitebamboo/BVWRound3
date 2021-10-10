using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextureData", menuName = "GameData/Texture", order = 2)]
public class TextureData : ScriptableObject
{
    //[SerializeField] string textureName;
    [SerializeField] Gesture_Group[] gesture_List;

    public Gesture_Group[] Gesture_List => gesture_List;

    public Sprite FindGesture(string _name)
    {
        int i = 0;
        while (i < Gesture_List.Length)
        {
            if (Gesture_List[i].Gesture_Name == _name)
            {
                return Gesture_List[i].texture;
            }
            i++;
        }
        return Gesture_List[0].texture;//shit
    }
}


[Serializable]
public struct Gesture_Group
{
    public string Gesture_Name;
    public Sprite texture;
}

