using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.New_Folder.Checkpoint;
using TMPro;
using UnityEngine;
using Zenject;

public class MapTeleporter : MonoBehaviour
{
    [Inject] private TeleporterService teleporterService;

    [SerializeField] private GameObject TeleporterButtonPrefab;

    [SerializeField] private Transform ButtonContainer;

    private List<GameObject> buttons= new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        SetUpButtons();
    }

    private void OnDisable()
    {
        RemoveButtons();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }



    private void SetUpButtons()
    {
        foreach (var checkpointCamp in teleporterService.GetDiscoveredCamps())
        {
            var buttonGO = Instantiate(TeleporterButtonPrefab, ButtonContainer);
            var buttonText = buttonGO.GetComponentInChildren<TMP_Text>();
            buttons.Add(buttonGO);
            if (buttonText != null)
            {
                buttonText.text = checkpointCamp.GetName;
            }

            var buttonComponent = buttonGO.GetComponent<UnityEngine.UI.Button>();
            if (buttonComponent != null)
            {
                var targetCamp = checkpointCamp;
                buttonComponent.onClick.AddListener((() =>
                {
                    teleporterService.TeleportPlayerTo(targetCamp);
                    gameObject.SetActive(false);
                }));
            }
        }
    }
    private void RemoveButtons()
    {
        buttons.ForEach(Destroy);
        buttons.Clear();
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
