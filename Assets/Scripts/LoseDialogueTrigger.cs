using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class LoseDialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool isTriggered = false;

    public void TriggerDialogue()
    {
        isTriggered = true;
        FindObjectOfType<LoseDialogueManager>().StartDialogue(dialogue);
    }
}
