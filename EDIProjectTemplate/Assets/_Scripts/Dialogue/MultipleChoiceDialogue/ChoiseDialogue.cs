using TMPro;
using UnityEngine;

public class ChoiseDialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI answerText;
    [SerializeField] private GameObject highlight;

    public void UpdateText(string text)
    {
        answerText.text = text;
    }

    public void EnableHighlight()
    {
        highlight.gameObject.SetActive(true);
        
    }

    public void DisableHighlight()
    {
        highlight.gameObject.SetActive(false);
        
    }


    // Update is called once per frame
    void Update()
    {

    }
}
