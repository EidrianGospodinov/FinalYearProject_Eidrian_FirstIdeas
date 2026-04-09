using System;
using UnityEngine;

namespace _Scripts.Units.Player.Core
{
    public class GameManager
    {
        private GameState currentGameState;
        public GameState GetCurrentGameState => currentGameState;
        public void SetGameState(GameState gameState)
        {
            currentGameState = gameState;
            switch (gameState)
            {
                case GameState.InGame:
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    Time.timeScale = 1;
                    break;
                case GameState.InMenu:
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                    Time.timeScale = 0;
                    break;
                case GameState.InDialogue:
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Confined;
                    Time.timeScale = 1;
                    break;
            }
        }
    }
    public enum GameState
    {
        InGame,
        InMenu,
        InDialogue
    }
}
