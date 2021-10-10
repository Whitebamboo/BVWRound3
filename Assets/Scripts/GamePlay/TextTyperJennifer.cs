namespace RedBlueGames.Tools.TextTyper
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using RedBlueGames.Tools.TextTyper;
    using UnityEngine.UI;
    using TMPro;

    /// <summary>
    /// Class that tests TextTyper and shows how to interface with it.
    /// </summary>
    public class TextTyperJennifer : MonoBehaviour
    {

        [HideInInspector]public GameObject GM;
        public float NextScriptInterval = 7f;

        public GestureDectection gestureDectection;

#pragma warning disable 0649 // Ignore "Field is never assigned to" warning, as these are assigned in inspector
        [SerializeField]
        private AudioClip printSoundEffect;

        [Header("UI References")]

        [SerializeField]
        private Button printNextButton;

        [SerializeField]
        private Button printNoSkipButton;

        [SerializeField]
        private Toggle pauseGameToggle;

        private Queue<string> dialogueLines = new Queue<string>();
        private Queue<Sprite> sprites = new Queue<Sprite>();
        private Queue<AudioClip> audios = new Queue<AudioClip>();

        [SerializeField]
        [Tooltip("The text typer element to test typing with")]
        private TextTyper testTextTyper;

        [SerializeField]
        [Tooltip("Chatacter's name")]
        private GameObject NameText;

        //[SerializeField]
        //[Tooltip("Current Reputation")]
        //private GameObject ReputationText;

#pragma warning restore 0649
        public void Start()
        {
            gestureDectection = FindObjectOfType<GestureDectection>();
            GM = GameObject.Find("GameManager");
            this.testTextTyper.PrintCompleted.AddListener(this.HandlePrintCompleted);
            this.testTextTyper.CharacterPrinted.AddListener(this.HandleCharacterPrinted);

            this.printNextButton.onClick.AddListener(this.HandlePrintNextClicked);
            this.printNoSkipButton.onClick.AddListener(this.HandlePrintNoSkipClicked);

            StartText();
            
            //dialogueLines.Enqueue(Character1.DialogData[0].Dialogs[0]);
            //dialogueLines.Enqueue("Hello! My name is... <delay=0.5>NPC</delay>. Got it, <i>bub</i>?");
            //dialogueLines.Enqueue("You can <b>use</b> <i>uGUI</i> <size=40>text</size> <size=20>tag</size> and <color=#ff0000ff>color</color> tag <color=#00ff00ff>like this</color>.");
            //dialogueLines.Enqueue("bold <b>text</b> test <b>bold</b> text <b>test</b>");
            //dialogueLines.Enqueue("Sprites!<sprite index=0><sprite index=1><sprite index=2><sprite index=3>Isn't that neat?");
            //dialogueLines.Enqueue("You can <size=40>size 40</size> and <size=20>size 20</size>");
            //dialogueLines.Enqueue("You can <color=#ff0000ff>color</color> tag <color=#00ff00ff>like this</color>.");
            //dialogueLines.Enqueue("Sample Shake Animations: <anim=lightrot>Light Rotation</anim>, <anim=lightpos>Light Position</anim>, <anim=fullshake>Full Shake</anim>\nSample Curve Animations: <animation=slowsine>Slow Sine</animation>, <animation=bounce>Bounce Bounce</animation>, <animation=crazyflip>Crazy Flip</animation>");
            //ShowScript();
        }

        public void Update()
        {
            UnityEngine.Time.timeScale = this.pauseGameToggle.isOn ? 0.0f : 1.0f;

            if (Input.GetKeyDown(KeyCode.Space))
            {

                var tag = RichTextTag.ParseNext("blah<color=red>boo</color");
                LogTag(tag);
                tag = RichTextTag.ParseNext("<color=blue>blue</color");
                LogTag(tag);
                tag = RichTextTag.ParseNext("No tag in here");
                LogTag(tag);
                tag = RichTextTag.ParseNext("No <color=blueblue</color tag here either");
                LogTag(tag);
                tag = RichTextTag.ParseNext("This tag is a closing tag </bold>");
                LogTag(tag);
            }
            //int reputationText = GameManager.Reputation;
            //this.ReputationText.GetComponent<TextMeshProUGUI>().text = reputationText.ToString();
        }
        public void StartText()
        {
            //jennifer
            
        }
        public void ReceiveDialogEvent(object[] obj)//???string[] stringArr, string name
        {
            //name
            
            this.NameText.GetComponent<TextMeshProUGUI>().text = (string)obj[1];
            foreach (var d in (string[])obj[0])
            {
                dialogueLines.Enqueue(d);
            }
            ShowScript();
        }
        private void HandlePrintNextClicked()
        {
            if (this.testTextTyper.IsSkippable() && this.testTextTyper.IsTyping)
            {
                this.testTextTyper.Skip();
            }
            else
            {
                ShowScript();
            }
        }

        private void HandlePrintNoSkipClicked()
        {
            ShowScript();
        }

        private void ShowScript()
        {
            if (dialogueLines.Count <= 0)
            {
                GM.SendMessage("WhenTypingCompletedEvent");
                return;
            }

            this.testTextTyper.TypeText(dialogueLines.Dequeue());
        }

        private void LogTag(RichTextTag tag)
        {
            if (tag != null)
            {
                Debug.Log("Tag: " + tag.ToString());
            }
        }

        private void HandleCharacterPrinted(string printedCharacter)
        {
            // Do not play a sound for whitespace
            if (printedCharacter == " " || printedCharacter == "\n")
            {
                return;
            }

            var audioSource = this.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = this.gameObject.AddComponent<AudioSource>();
            }

            audioSource.clip = this.printSoundEffect;
            audioSource.Play();
        }

        //Wait for respond
        private void HandlePrintCompleted()
        {
            Debug.Log("TypeText Complete");
            //3secs later show next scripts
            StartCoroutine("WaitForGesture");


        }

        IEnumerator ShowNextScript()
        {
            yield return new WaitForSeconds(NextScriptInterval);
            HandlePrintNextClicked();
        }

        IEnumerator WaitForGesture()
        {
            while (true)
            {
                if (gestureDectection.matchedGesture == GestureType.MoveNext)
                {
                    break;
                }
                else
                {
                    yield return null;
                }
            }
            yield return new WaitForSeconds(NextScriptInterval);
            HandlePrintNextClicked();
        }
    }
}