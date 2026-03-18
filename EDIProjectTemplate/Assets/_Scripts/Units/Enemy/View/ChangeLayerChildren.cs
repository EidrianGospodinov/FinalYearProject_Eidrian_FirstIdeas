using UnityEngine;



public class ChangeLayerChildren : MonoBehaviour
{
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeLayerOfChildren(LayerMask layerMask)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.layer = (int)Mathf.Log(layerMask.value, 2);
        }
    }
}
