using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private bool inConvo;
    public TextAsset dialogueFile;
    private string[] linesRaw;
    private ArrayList lines = new ArrayList();
    private Line currentLine;

    // Start is called before the first frame update
    void Start()
    {
        inConvo = false;
        BeginDialogue();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void BeginDialogue()
    {
        linesRaw = dialogueFile.text.Split('\n');
        print(linesRaw[0]);
        foreach (string line in linesRaw)
        {
            string[] textSplit = line.Split(':');
            string text = textSplit[1];
            string[] parameterSplit = textSplit[0].Split(' ');
            if (parameterSplit[0].Equals("T"))
            {
                int[] nextLine = new int[] { int.Parse(parameterSplit[1]) };
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
                    nextLine[i] = int.Parse(nextLineStrings[i]);
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
    }

    void NextDialogue()
    {
        print(currentLine.getLine());
        if (currentLine.GetType() == typeof(LineChoice))
        {
            //line choice
            LineChoice lineChoice = (LineChoice)currentLine;

        }
        else
        {

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
            return speaker + ": " + text + "\n";
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
}
