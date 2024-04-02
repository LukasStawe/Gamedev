using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DIalogueUI : MonoBehaviour
{
    //The UI Object Container, set active when starting dialogue
    [SerializeField] private GameObject container;

    //The gameObject reference of the container
    GameObject dialogueWindow;

    //true if the container is visible/dialogue started, false otherwise
    public bool isShown;

    //The NPC stored locally
    private FriendlyNPC talkedNPC;

    //The gameobject with the Grid Layout object, parent of the Buttons
    [SerializeField] private GameObject dialogueParent;

    //Prefab of the Choice Buttons
    [SerializeField] private GameObject answerButton;

    //Array with all the instantiated Choice Buttons
    private List<GameObject> buttons = new List<GameObject>();

    //The Text Object where the Dialoguelines gonna be written
    [SerializeField] private TextMeshProUGUI dialogueText;

    //Speed with which the dialogue is gonna be written
    [SerializeField] private float typingSpeed = 0.5f;

    //The coroutine that types each Dialogue portion
    private Coroutine typeDialogueCoroutine;

    //true if the coroutine is typing a line, false otherwise
    private bool isTyping = false;

    //The current line thats being typed
    private string currentLine;

    //Name of the NPC talking to
    [SerializeField] private GameObject npcName;

    //The Dialogue tree that includes the whole Dialogue, gotten from the NPC
    DialogueTreeSO dialogueTree;

    //Queue that gets filled with all the lines of the current Dialogue Section
    private Queue<string> lines = new Queue<string>();

    //The current section of the Dialogue
    private DialogueTreeSO.DialogueSection currentSection;

    //true if currently at a branching point, false otherwise
    private bool isOnBranchingPoint = false;


    // Start is called before the first frame update
    void Start()
    {
        dialogueWindow = container.gameObject;
    }

    /// <summary>
    /// Called every frame.
    ///  Waits for inputs to continue Dialogue.
    /// </summary>
    private void Update()
    {
        if (isShown && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping) 
            {
                FinishLine();
            } 
            else if (!isOnBranchingPoint && !isTyping)
            {
                DisplayLines(lines);
            }
        }

        if (isOnBranchingPoint)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && buttons.Count > 0)
            {
                FinishLine();
                buttons[0].GetComponent<Button>().onClick.Invoke();
            }
             else if (Input.GetKeyDown(KeyCode.Alpha2) && buttons.Count > 1)
            {
                FinishLine();
                buttons[1].GetComponent<Button>().onClick.Invoke();
            }
            else if ( Input.GetKeyDown(KeyCode.Alpha3) && buttons.Count > 2)
            {
                FinishLine();
                buttons[2].GetComponent<Button>().onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) && buttons.Count > 3)
            {
                FinishLine();
                buttons[3].GetComponent<Button>().onClick.Invoke();
            }

        }
    }

    /// <summary>
    /// Shows the Dialogue window
    /// </summary>
    public void Show()
    {
        dialogueWindow.SetActive(true);
        isShown = true;
    }

    /// <summary>
    /// Empties all saved variables and hides the Dialogue window
    /// </summary>
    public void Hide()
    {
        dialogueTree = null;
        lines.Clear();
        currentLine = null;
        container.SetActive(false);
        isShown = false;
        talkedNPC = null;
    }

    /// <summary>
    /// This Method is called when the Dialogue is started.
    /// Shows the window, sets the NPC Name and starts the Dialogue-Process.
    /// </summary>
    /// 
    /// <param name="npc"> The NPC Object talked to </param>
    public void createUI(FriendlyNPC npc)
    {
        talkedNPC = npc;
        npcName.GetComponent<TextMeshProUGUI>().text = npc.name;
        Show();
        StartDialogue(npc.dialogue);
    }

    /// <summary>
    /// Starts the Dialogue by saving the tree locally and starting the first section.
    /// </summary>
    /// 
    /// <param name="newDialogueTree"> The Dialogue Tree Object </param>
    public void StartDialogue(DialogueTreeSO newDialogueTree)
    {
        dialogueTree = newDialogueTree;
        if (!talkedNPC.hasBeenTalkedTo)
        {
            NextSection(dialogueTree.sections[0]);
            talkedNPC.hasBeenTalkedTo = true;
        } else
        {
            NextBranchPoint(dialogueTree.sections[0].branchPoint);
        }
    }

    /// <summary>
    /// Starts the next Section by destroying all Choice Buttons and
    /// putting every Dialogue Line of this Section into the lines Queue.
    /// </summary>
    /// 
    /// <param name="section"> The current section of the Dialogue Tree </param>
    private void NextSection(DialogueTreeSO.DialogueSection section)
    {
        foreach(Transform child in dialogueParent.transform)
        {
            Destroy(child.gameObject);
        }
        buttons.Clear();

        currentSection = section;

        for ( int i = 0; i < section.dialogue.Length; i++ )
        {
            lines.Enqueue(section.dialogue[i]);
        }

        DisplayLines(lines);
    }

    /// <summary>
    /// Sets the Branch Point bool to true, starts the Coroutine with the Branch Question and
    /// Instantiates the Choice-Buttons. Then it adds a onClick Listener to every Button with the Method
    /// that should be called on click.
    /// </summary>
    /// 
    /// <param name="branch"> The current Branchpoint of the Dialogue </param>
    private void NextBranchPoint(DialogueTreeSO.BranchPoint branch)
    {
        isOnBranchingPoint = true;

        typeDialogueCoroutine = StartCoroutine(WriteLine(branch.question));

        for (int i = 0; i < branch.answers.Length; i++)
        {
            GameObject button = Instantiate(answerButton, dialogueParent.transform);
            TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
            text.text = i + 1 + " " + branch.answers[i].answerLabel;
            DialogueTreeSO.Answer currentAnswer = branch.answers[i];
            if (branch.answers[i].hasBeenClicked)
            {
                text.color = Color.grey;
            }             

            button.GetComponent<Button>().onClick
                .AddListener(() => DecisionButtonPressed(currentAnswer));

            buttons.Add(button);
        }
        Debug.Log(isOnBranchingPoint);
    }

    /// <summary>
    /// The OnClick Method for every Choice-Button. Sets the hasBeenClicked for the specific Answer-Object to true, sets the isOnBranchingPoint to false
    /// and starts the next Dialogue Section which is the section the Answer leads to.
    /// </summary>
    /// 
    /// <param name="answer">The specific Answer-Object asigned to every specific Button</param>
    private void DecisionButtonPressed(DialogueTreeSO.Answer answer)
    {
        answer.hasBeenClicked = true;
        isOnBranchingPoint = false;
        NextSection(dialogueTree.sections[answer.nextElement]);
    }

    
    /// <summary>
    /// Checks if there are still lines saved for the current dialogue Section.If yes, display it and remove it from the Queue.
    /// If not, end the Dialogue if this section was a Dialogue-Ending Section, go to the Branch-Point of the Section if it has one
    /// or go back to the Branch-Point of the first Section if the current Section has no Branch.
    /// </summary>
    /// 
    /// <param name="lines"> The Queue object with all the lines of the current Dialogue section. </param>
    public void DisplayLines(Queue<string> lines)
    {
        if (lines.Count == 0)
        {
            if (currentSection.endAfterDialogue)
            {
                Hide();
                return;
            }
            else if (currentSection.hasBranchPoint)
            {
                NextBranchPoint(currentSection.branchPoint);
                return;
            }
            else if (!currentSection.hasBranchPoint)
            {
                NextBranchPoint(dialogueTree.sections[0].branchPoint);
                return;
            }
        }

        currentLine = lines.Dequeue();
        
        typeDialogueCoroutine = StartCoroutine(WriteLine(currentLine));
    }

    
    /// <summary>
    /// Coroutine that writes the current line letter for letter in a set typing speed.
    /// </summary>
    /// 
    /// <param name="line"> The current line of the Dialogue that should be written. </param>
    /// 
    /// <returns> Returns a timer to wait for typing Speed </returns>
    private IEnumerator WriteLine(string line)
    {
        isTyping = true;
        currentLine = line;
        dialogueText.text = "";

        foreach(char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    /// <summary>
    /// Stops the Writing Coroutine and immediatly finished the current line.
    /// </summary>
    private void FinishLine()
    {
        StopCoroutine(typeDialogueCoroutine);
        isTyping = false;
        Debug.Log("Coroutine stopped. " + currentLine + isTyping);
        dialogueText.text = "";
        dialogueText.text = currentLine;
    }
}
