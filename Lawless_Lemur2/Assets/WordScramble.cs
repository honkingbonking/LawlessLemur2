using UnityEngine;
using TMPro;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using WiimoteApi;

public class WordScramble : MonoBehaviour
{
    public string[] words; // Array of strings
    public TextMeshProUGUI mainText; // Main text to display the original string
    public TextMeshProUGUI[] wordSlots; // Array of TextMeshPro for displaying scrambled words
    public TextMeshPro resultText; // Text to display result
    [SerializeField] GameObject speechBubble;

    private string originalString; // Original unscrambled string
    private string currentString = ""; // Current string formed from rearranged words

    private ManageFlow flow;

    [SerializeField] Move Lemur;

    private int wordAmt = 0;

    private Wiimote mote;
    private GuitarData gd;

    // Variables to store the previous state of each button
    private bool prevGreenState = false;
    private bool prevRedState = false;
    private bool prevYellowState = false;
    private bool prevBlueState = false;
    private bool prevOrangeState = false;

    private void Start()
    {
        flow = GetComponent<ManageFlow>();
        WiimoteManager.FindWiimotes();
        if (WiimoteManager.Wiimotes.Count > 0)
        {
            mote = WiimoteManager.Wiimotes[0];
            if (mote != null)
            {
                Debug.Log("Connected!");
                mote.SendPlayerLED(true, false, false, false);
                Debug.Log("Current Extension: " + mote.current_ext.ToString());
            }
        }
    }

    public void StartShuffling()
    {
        speechBubble.SetActive(true);
        // Select a random string from the array
        originalString = words[Random.Range(0, words.Length)];
        mainText.text = originalString;
        // Separate the words from the original string
        string[] wordsArray = originalString.Split(' ');
        // Scramble the words and assign them to TextMeshPro objects
        ShuffleArray(wordsArray);
        for (int i = 0; i < wordSlots.Length; i++)
        {
            wordSlots[i].text = wordsArray[i];
        }
        flow.ToggleObj();
    }

    private void Update()
    {
        // Handle keyboard input
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddWord(wordSlots[0].text);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AddWord(wordSlots[1].text);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AddWord(wordSlots[2].text);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            AddWord(wordSlots[3].text);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            AddWord(wordSlots[4].text);
        }

        // Handle Wiimote input
        if (mote != null)
        {
            mote.ReadWiimoteData();
            if (mote.current_ext == ExtensionController.GUITAR)
            {
                gd = mote.Guitar;
                if (gd != null)
                {
                    CheckButtonState(gd.green, ref prevGreenState, wordSlots[0].text);
                    CheckButtonState(gd.red, ref prevRedState, wordSlots[1].text);
                    CheckButtonState(gd.yellow, ref prevYellowState, wordSlots[2].text);
                    CheckButtonState(gd.blue, ref prevBlueState, wordSlots[3].text);
                    CheckButtonState(gd.orange, ref prevOrangeState, wordSlots[4].text);
                }
            }
        }
    }

    private void CheckButtonState(bool buttonState, ref bool prevState, string word)
    {
        if (buttonState && !prevState)
        {
            AddWord(word);
        }
        // Update the previous state for the next check
        prevState = buttonState;
    }

    // Shuffle an array of strings
    private void ShuffleArray(string[] array)
    {
        wordAmt = 0;
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            string temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    // Add clicked word to the current string
    public void AddWord(string word)
    {
        currentString += word + " ";
        resultText.text = currentString;
        wordAmt++;
        flow.GuitarStrum();
       if (wordAmt == 5)

        {
            flow.ToggleObj();
            if (currentString.Trim() == originalString)
            {
                Debug.Log("YOU WIN");
                flow.KillThem();
                Lemur.LemurMove();
                CleanUp();
            }
            else
            {
                resultText.text = "";
                flow.Death();
                Debug.Log("Failed!");
                CleanUp();
            }
        }
    }

    private void CleanUp()
    {
        currentString = "";
        resultText.text = "";
    }
}
