using UnityEngine;

public class MouseController : MonoBehaviour
{
    //for setting camera position to match the positioning set by GridManager 
    [SerializeField] float width, height;
    [SerializeField] Transform cam; 

    [SerializeField] private GameObject flare1_prefab;
    private GameObject flare1 = null; 
    private bool flare1_placed;
    private FlareGrowth targetScript = null;

    private Vector3 center1 = Vector3.zero;
    private float rad1 = 0; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flare1_placed = false; 
        cam.position = new Vector3((width / 2) - 0.5f, (height / 2) - 0.5f, -10);

    }

    public Vector3 getCenter1()
    {
        return center1; 
    }

    public float getRad1()
    {
        return rad1; 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !(flare1_placed))
        {
            Vector3 toSpawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            toSpawnPos.z = 0f; 
            Vector3 offset = new Vector3(0, 0, 10); 
            flare1 = Instantiate(flare1_prefab, toSpawnPos + offset, Quaternion.identity);
            targetScript = flare1.GetComponent<FlareGrowth>();
            if(targetScript == null)
            {
                Debug.Log("target script null"); 
            }
            else
            {
                targetScript.turnGrowOn();
            }

            Debug.Log(flare1.transform.position); 
            flare1_placed = true;
        }
        if(Input.GetMouseButtonUp(0))
        {
            if(flare1 != null && targetScript != null)
            {
                //Vector3 spriteHalfSize = flare1.GetComponent<SpriteRenderer>().sprite.bounds.extents;
                rad1 = flare1.GetComponent<SpriteRenderer>().bounds.size.x / 2f;

                center1 = flare1.transform.position;
                Debug.Log(rad1);
                Debug.Log(center1); 
                targetScript.turnGrowOff();
            }
        }
    }
}
