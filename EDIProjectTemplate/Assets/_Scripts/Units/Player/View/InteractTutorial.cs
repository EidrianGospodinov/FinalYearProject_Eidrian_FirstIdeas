using System;
using TMPro;
using UnityEngine;

namespace _Scripts.Units.Player.View
{
    public class InteractTutorial : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI tutorialText;
        private bool destroyingSelf = false;
        private void Start()
        {
            GetComponent<Animator>().Play("InputTutorialFadeInAnim");
        }

        public void UpdateTutorialText(string text)
        {
            tutorialText.text = text;
        }

        public void DestroySelf()
        {
            if (destroyingSelf)
            {
                return;
            }
            destroyingSelf = true;
            GetComponent<Animator>().Play("InputTutorialFadeOutAnim");
            Destroy(gameObject, 1);

        }
        
    }
}