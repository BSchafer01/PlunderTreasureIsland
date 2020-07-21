using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
    public TMPro.TMP_Text nameText;
    public TMPro.TMP_Text dialogueText;
    public GameObject dialogueBox;
    public Animator animator;
    public Animator stumpy;
    public Animator input;
    public GameObject inputForm;
    public Animator pirateShip;
    public TMPro.TMP_InputField manualInput;
    public Rigidbody2D rb;

    private Queue<string> sentences = new Queue<string>();

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();

    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        stumpy.SetBool("IsOnScreen", true);
        Debug.Log("Starting conversation with " + dialogue.name);
        nameText.text = dialogue.name;
        //dialogueBox.SetActive(true);

        sentences.Clear();

        foreach (string s in dialogue.sentences)
        {
            sentences.Enqueue(s);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return 0.1;
        }
    }

    void EndDialogue()
    {
        inputForm.SetActive(true);
        stumpy.SetBool("IsOnScreen", false);
        animator.SetBool("IsOpen", false);
        input.SetBool("IsOpen", true);
        

        Debug.Log("End of conversation.");
    }

    public void PirateShipMove(int position)
    {
        pirateShip.SetInteger("Position", position);

        //pirateShip.SetInteger("Position", 0);
    }


}
