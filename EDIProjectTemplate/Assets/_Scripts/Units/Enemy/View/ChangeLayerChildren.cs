using UnityEngine;
using UnityEngine.Serialization;


public class ChangeLayerChildren : MonoBehaviour
{
    [SerializeField] private float normalOutlineSize = 0.02f;
    [SerializeField] private float cyclopsOutlineSize = 0.0001f;
    [SerializeField] private bool useCyclopsSize = false;
    
    private static readonly int SizeID = Shader.PropertyToID("_Size");

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
        int layer = (int)Mathf.Log(layerMask.value, 2);

        float outlineSize = useCyclopsSize ? cyclopsOutlineSize : normalOutlineSize;
        foreach (Transform child in transform)
        {
            child.gameObject.layer = layer;
            
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                MaterialPropertyBlock block = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(block);
                block.SetFloat(SizeID, outlineSize);
                renderer.SetPropertyBlock(block);
            }
        }
    }
}
