using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColor, offsetColor;
    //[SerializeField] private SpriteRenderer renderer;
    private SpriteRenderer toRender; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        toRender = GetComponent<SpriteRenderer>();
    }
    public void Init(bool isOffset) 
    {
        toRender.color = isOffset ? offsetColor : baseColor; 
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
