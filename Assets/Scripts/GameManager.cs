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

    public GestureDectection gestureDectection;
    public GameObject TitleUI;
    public GameObject IntroUI;
    public GameObject GuestUI;
    public GameObject ReputationUI;
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

        //StartCoroutine("TestCode");

        gestureDectection = FindObjectOfType<GestureDectection>();

        StartCoroutine("DelayIntroUI");
    }
    private void Update()
    {
        //print(state);
        if (state == GameState.Start)
        {
            
        }
        if (state == GameState.Intro)
        {
            //To Do 
            if (gestureDectection.matchedGesture == GestureType.MoveNext)
            {
                state = GameState.Guest;

                IntroUI.SetActive(false);
                GuestUI.SetActive(true);
                ReputationUI.SetActive(true);
            }
        }
        else if (state == GameState.Ending)
        {
            GuestUI.GetComponent<Canvas>().enabled = false;
            EndSceneUI.instance.GetComponent<Canvas>().enabled = true;
            EndSceneUI.ShowEndSceneUI();
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
                gestureDectection.BeginRecognize();
                Debug.LogWarning("WaitForGesture");
                if (GestureFinished)
                {
                    gestureDectection.StopRecognize();
                    guestState = GuestState.Response;
                    ChangeRepulation(gestureState);
                    HintGesture.Instance.ShowHintGestureUI(false);
                    AskTextTyperShowResponse(data[personIndex]);
                }
            }
            else if (guestState == GuestState.Response)
            {
                
            }
            else if (guestState == GuestState.End)
            {
                
            }
        }

        //Test, instead of gesture
        if (Input.GetKeyDown(KeyCode.A))
        {
            gestureState = GestureEnum.ThumbsUp;
            GestureFinished = true;
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            gestureState = GestureEnum.ThumbsDown;
            GestureFinished = true;
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            gestureState = GestureEnum.MiddleFinger;
            GestureFinished = true;
        }

        if (guestState == GuestState.WaitForGesture)
        {
            //Read gesture from GestureDectection
            GestureType currentGesture = gestureDectection.matchedGesture;
            if (currentGesture != GestureType.Undefined)
            {
                switch (currentGesture)
                {
                    case GestureType.ThumbsUp:
                        {
                            gestureState = GestureEnum.ThumbsUp;
                            GestureFinished = true;
                            MusicManager.Instance.PlayThumbsUpClip();
                            Debug.LogWarning("->ThumbsUp");
                            break;
                        }
                    case GestureType.ThumbsDown:
                        {
                            gestureState = GestureEnum.ThumbsDown;
                            GestureFinished = true;
                            MusicManager.Instance.PlayThumbsDownClip();
                            Debug.LogWarning("->ThumbsDown");
                            break;
                        }
                    case GestureType.MiddleFinger:
                        {
                            gestureState = GestureEnum.MiddleFinger;
                            GestureFinished = true;
                            MusicManager.Instance.PlayRaiseHandClip();
                            Debug.LogWarning("->MiddleFinger");
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

            }
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

        PortraitManager.Instance.SetPortrait(dp.Portraits);
        MusicManager.Instance.PlayClip(dp.Sound, 0.5f);

        TextTyper.SendMessage("ReceiveDialogEvent", message, SendMessageOptions.RequireReceiver);
    }

    //3 Wait for gesture

    //To do waitforgesture don't show ok
    public void WhenTypingCompletedEvent()
    {
        if (guestState == GuestState.Dialog)
        {
            HintGesture.Instance.ShowHintGestureUI(true);
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
                StartCoroutine("GuestLeaving");
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
        Sprite portrait = personData.DialogData[dialogIndex].Responses[(int)gestureState].Portrait;
        AudioClip aduio = personData.DialogData[dialogIndex].Responses[(int)gestureState].Sound;

        object[] message = new object[2];
        message[0] = stringArr;
        message[1] = personData.PeopleName;

        PortraitManager.Instance.SetPortrait(portrait);
        MusicManager.Instance.PlayClip(aduio, 0.5f);

        TextTyper.SendMessage("ReceiveDialogEvent", message, SendMessageOptions.RequireReceiver);

    }

    public void ChangeRepulation(GestureEnum g)
    {
        Reputation += data[personIndex].DialogData[dialogIndex].Responses[(int)g].Reputation;
    }
    IEnumerator TestCode()
    {
        yield return new WaitForSeconds(5);
        //GestureFinished = true;
    }

    IEnumerator GuestLeaving()
    {
        yield return new WaitForSeconds(5);
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
    IEnumerator DelayIntroUI()
    {
        yield return new WaitForSeconds(5);
        state = GameState.Intro;
        TitleUI.SetActive(false);
        IntroUI.SetActive(true);
    }
}
