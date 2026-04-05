using System;
using _Scripts.New_Folder.SkillTree;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillTree : MonoBehaviour
{
    [SerializeField] private SkillAmount skillAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        skillAmount.UpdateSkillAmountText();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        
    }

    private void OnDisable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }
}
