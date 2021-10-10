using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "GameData/person", order = 1)]
public class PersonData : ScriptableObject
{
    [SerializeField] string peopleName;
    [SerializeField] DialogPack[] dialogData;

    public string PeopleName => peopleName;
    public DialogPack[] DialogData => dialogData;
}

[Serializable]
public class DialogPack
{
    [SerializeField] string[] dialogs;
    [SerializeField] Sprite portrait;
    [SerializeField] AudioClip sound;
    [SerializeField] GestureResponse[] responses;

    public string[] Dialogs => dialogs;
    public Sprite Portraits => portrait;
    public AudioClip Sound => sound;
    public GestureResponse[] Responses => responses;
}

[Serializable]
public class GestureResponse
{
    [SerializeField] GestureEnum gesture;
    [SerializeField] string[] response;
    [SerializeField] Sprite portrait;
    [SerializeField] AudioClip sound;
    [SerializeField] int reputation;

    public GestureEnum Gesture => gesture;
    public string[] Response => response;
    public Sprite Portrait => portrait;
    public AudioClip Sound => sound;
    public int Reputation => reputation;
}

