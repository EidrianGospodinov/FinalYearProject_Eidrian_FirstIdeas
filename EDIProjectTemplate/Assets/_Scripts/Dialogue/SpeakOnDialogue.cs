using System;
using _Scripts.Dialogue;
using UnityEngine;

[RequireComponent(typeof(DialogueManager), typeof(Animator))]
public class SpeakOnDialogue : MonoBehaviour
{
    private Animator animator;

    private DialogueManager _dialogueManager;

    private Transform mainCamTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamTransform = Camera.main.transform;
        animator = GetComponent<Animator>();
        _dialogueManager = GetComponent<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_dialogueManager != null)
        {
            if (animator.GetBool("IsTalking") == _dialogueManager.IsTalking)
            {
                return;
            }
            animator.SetBool("IsTalking", _dialogueManager.IsTalking);
        }
    }

    /*private void LateUpdate()
    {
        Vector3 targetDir = mainCamTransform.position - transform.position;
        targetDir.y = 0;

        transform.rotation = Quaternion.LookRotation(targetDir);
        
    }*/
}
