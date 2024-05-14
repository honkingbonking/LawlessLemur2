using UnityEngine;
using TMPro;

public class WordScramble : MonoBehaviour
{
    public string[] words; // Array of strings
    public TextMeshProUGUI mainText; // Main text to display the original string
    public TextMeshProUGUI[] wordSlots; // Array of TextMeshPro for displaying scrambled words
    public TextMeshPro resultText; // Text to display result

    private string originalString; // Original unscrambled string
    private string currentString = ""; // Current string formed from rearranged words

    int wordAmt = 0;

    private void Start()
    {
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
    }

    private void Update()
    {
        // Handle keyboard input
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddWord(wordSlots[0].text);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            AddWord(wordSlots[1].text);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            AddWord(wordSlots[2].text);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            AddWord(wordSlots[3].text);
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            AddWord(wordSlots[4].text);
        }

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
        if (wordAmt == 5)
        {
            if (currentString.Trim() == originalString)
            {
                Debug.Log("YOU WIN");
                resultText.text = "YOU WIN";
            }
            else
            {
                //When it is wrong
            }
        }
    }
}
