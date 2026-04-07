using System;
using _Scripts.New_Folder.SkillTree;
using _Scripts.Units.Player.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class SkillTree : MonoBehaviour
{
    [Inject] private GameManager gameManager;
    [SerializeField] private SkillAmount skillAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        skillAmount.UpdateSkillAmountText();
        gameManager.SetGameState(GameState.InMenu);
        
    }

    private void OnDisable()
    {
        gameManager.SetGameState(GameState.InGame);
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
