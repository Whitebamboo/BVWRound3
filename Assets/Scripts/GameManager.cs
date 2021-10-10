using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    Start,
    Intro,
    Guest,
    Ending
}

public enum GuestState
{
    Start,
    Dialog,
    WaitForGesture,
    Response,
    End
}


public class GameManager : MonoBehaviour
{
    static GameManager s_Instance;
    public static GameManager Instance => s_Instance;

    public PersonData[] data;
    public GameObject TextTyper;
    public GestureEnum gestureState; 
    public static int Reputation = 0;


    [HideInInspector] public GameState state;
    [HideInInspector] public GuestState guestState;

    private bool GestureFinished = false;
    //person index
    private int personIndex = 0;
    //dialog index
    private int dialogIndex = 0;

    void Awake()
    {
        if (s_Instance != null)
        {
            Destroy(this);
            return;
        }

        s_Instance = this;
    }

    //Start ----
    private void Start()
    {
        TextTyper = GameObject.FindGameObjectWithTag("Typer");

        StartCoroutine("TestCode");
    }
    private void Update()
    {
        print(state);
        print(guestState);
        if (state == GameState.Start)
        {
            state = GameState.Intro;
        }
        if (state == GameState.Intro)
        {
            state = GameState.Guest;
        }

        if (state == GameState.Guest)
        {
            if (guestState == GuestState.Start)
            {
                EachGuestStart();
            }
            else if(guestState == GuestState.Dialog)
            {
                
            }
            else if(guestState == GuestState.WaitForGesture)
            {
                //
                if (GestureFinished)
                {
                    guestState = GuestState.Response;
                    AskTextTyperShowResponse(data[personIndex]);
                }
            }
            else if (guestState == GuestState.Response)
            {
                
            }
            else if (guestState == GuestState.End)
            {
                guestLeaving();
            }
        }

        //Test, instead of gesture
        if (Input.GetKeyDown(KeyCode.A))
        {
            gestureState = GestureEnum.ThumbsUp;
            changeRepulation(GestureEnum.ThumbsUp);
            GestureFinished = true;
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            gestureState = GestureEnum.ThumbsDown;
            changeRepulation(GestureEnum.ThumbsDown);
            GestureFinished = true;
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            gestureState = GestureEnum.MiddleFinger;
            changeRepulation(GestureEnum.MiddleFinger);
            GestureFinished = true;
        }
    }
    
    
    void EachGuestStart()
    {
        //clear
        guestState = GuestState.Dialog;
        CreateCharacterImageAndMove();
        AskTextTyperShowDialogData(data[personIndex]);
    }

    //1 Someone comes
    void CreateCharacterImageAndMove()
    {
        //TODO
    }
    //2 Ask Text Typer to show
    void AskTextTyperShowDialogData(PersonData personData)
    {
        DialogPack dp = personData.DialogData[dialogIndex];
        //ugly
        object[] message = new object[2];
        message[0] = dp.Dialogs;
        message[1] = personData.PeopleName;

        TextTyper.SendMessage("ReceiveDialogEvent", message, SendMessageOptions.RequireReceiver);
    }

    //3 Wait for gesture
    public void WhenTypingCompletedEvent()
    {
        if (guestState == GuestState.Dialog)
        {
            guestState = GuestState.WaitForGesture;
        }
        else if (guestState == GuestState.Response)
        {
            //if other dialog
            if (dialogIndex < data[personIndex].DialogData.Length - 1)
            {
                dialogIndex++;
                EachGuestStart();
            }
            else
            {
                guestState = GuestState.End;
            }

            
        }
        //Dialog or response Completed
        
        //TO DO
        print("WhenDialogCompletedEvent");
    }


    void AskTextTyperShowResponse(PersonData personData)
    {
        print("AskTextTyperShowResponse");
        string[] stringArr = personData.DialogData[dialogIndex].Responses[(int)gestureState].Response;

        object[] message = new object[2];
        message[0] = stringArr;
        message[1] = personData.PeopleName;

        TextTyper.SendMessage("ReceiveDialogEvent", message, SendMessageOptions.RequireReceiver);

    }
    //Guest leaves
    void guestLeaving()
    {
        //TO DO
        GestureFinished = false;
        if (personIndex < data.Length - 1)
        {
            personIndex++;
            guestState = GuestState.Start;
        }
        else
        {
            //Enter ending
            state = GameState.Ending;
        }
    }
    public void changeRepulation(GestureEnum g)
    {
        Reputation += data[personIndex].DialogData[dialogIndex].Responses[(int)g].Reputation;
    }
    IEnumerator TestCode()
    {
        yield return new WaitForSeconds(5);
        //GestureFinished = true;
    }
}
