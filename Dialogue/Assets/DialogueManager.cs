using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // type next speaker:text
    // choice next,next,next speaker:text
    // paramter name,value speaker:text
    // flag name speaker:text
    // opinion name,value speaker:text,text
    private bool inConvo;
    public TextAsset dialogueFile;
    private string[] linesRaw;
    private List<Line> lines = new List<Line>();
    private Line currentLine;

    public float textSpeed = 0.1f;

    //public AudioClip dialogueSound;
    public AudioSource dialogueSound;

    public Image textboxImage;
    public Text textboxText;

    // Start is called before the first frame update
    void Start()
    {
        dialogueSound = GetComponent<AudioSource>();
        inConvo = false;
        BeginDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            NextDialogue(0);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            NextDialogue(1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            NextDialogue(2);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            NextDialogue(3);
        }
    }

    void BeginDialogue()
    {
        linesRaw = dialogueFile.text.Split('\n');
        //print(linesRaw[0]);
        foreach (string line in linesRaw)
        {
            string[] textSplit = line.Split(':');
            string text = textSplit[1];
            string[] parameterSplit = textSplit[0].Split(' ');
            if (parameterSplit[0].Equals("T"))
            {
                int[] nextLine = new int[] { int.Parse(parameterSplit[1]) - 1};
                string speaker = parameterSplit[2];
                Line newLine = new Line(nextLine, speaker, text);
                lines.Add(newLine);

            }
            else if (parameterSplit[0].Equals("C"))
            {
                string[] nextLineStrings = parameterSplit[1].Split(',');
                int[] nextLine = new int[nextLineStrings.Length];
                for (int i = 0; i < nextLineStrings.Length; i++)
                {
                    nextLine[i] = int.Parse(nextLineStrings[i]) - 1;
                }
                string speaker = parameterSplit[2];
                LineChoice newLine = new LineChoice(nextLine, speaker, text);
                lines.Add(newLine);
            }
            else
            {
                print("invalid line type");
                return;
            }
        }
        currentLine = lines[0];
        StartCoroutine(DisplayText(currentLine.getLine()));
    }

    void NextDialogue(int choice)
    {
        if (currentLine.GetType() == typeof(LineChoice))
        {
            //line choice
            //print("line choice");
            LineChoice lineChoice = (LineChoice)currentLine;
            currentLine = lines[lineChoice.getNextLine(choice)];
            StartCoroutine(DisplayText(currentLine.getLine()));

        }
        else
        {
            if(currentLine.getNextLine() == -2)
            {
                EndDialogue();
            }
            else
            {
                currentLine = lines[currentLine.getNextLine()];
                StartCoroutine(DisplayText(currentLine.getLine()));
            }
        }
    }

    void EndDialogue()
    {

    }

    public class Line
    {
        protected int[] nextLine;
        protected string speaker;
        protected string text;

        public Line(int[] nextLine, string speaker, string text)
        {
            this.nextLine = nextLine;
            this.speaker = speaker;
            this.text = text;
        }

        public int getNextLine()
        {
            return nextLine[0];
        }

        public string getLine()
        {
            return speaker + "\n" + text + "\n";
        }
    }

    public class LineChoice : Line
    {

        public LineChoice(int[] nextLine, string speaker, string text) : base(nextLine, speaker, text) { }

        public int getNextLine(int choice)
        {
            try
            {
                return nextLine[choice - 1];
            }
            catch (System.ArgumentException)
            {
                print("Invalid choice index");
                return -1;
            }
        }
    }
    IEnumerator DisplayText(string text)
    {
        int charIndex = 0;
        while (charIndex < text.Length)
        {
            textboxText.text = text.Substring(0, charIndex);
            if (!text[charIndex].Equals(' '))
            {
                dialogueSound.Play(0);
            }
            charIndex++;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}
