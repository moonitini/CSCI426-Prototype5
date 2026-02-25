using UnityEngine;

public class FlareGrowth : MonoBehaviour
{
    bool growing = false;
    [SerializeField] float maxRadius = 2;
    [SerializeField] float growthFactor = 1.1f;
    [SerializeField] float growSpeed = 1.0f; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //maxRadius = 2; 
    }

    public void turnGrowOn()
    {
        growing = true; 
    }

    public void turnGrowOff()
    {
        growing = false; 
    }

    // Update is called once per frame
    void Update()
    {
        if(growing)
        {
            float currScale = transform.localScale.x; 
            if(currScale <= maxRadius)
            {
                float newScale = currScale + growSpeed * Time.deltaTime;
                newScale = Mathf.Min(maxRadius, newScale); 
                transform.localScale = new Vector3(newScale, newScale, 1);

                
            }
        }
    }
}
