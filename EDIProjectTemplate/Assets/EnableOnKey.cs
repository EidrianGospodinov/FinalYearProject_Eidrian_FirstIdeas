using UnityEngine;

public class EnableOnKey : MonoBehaviour
{
    [SerializeField] GameObject skillTree;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            skillTree.SetActive(true);
        }   
    }
}
