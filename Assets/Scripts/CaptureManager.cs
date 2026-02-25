using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UIElements;

public class CaptureManager : MonoBehaviour
{
    [SerializeField] MouseController flareInfo;
    [SerializeField] EnemyManager enemyInfo; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        getCaptures(); 
    }


    IEnumerator getCaptures()
    {
        yield return new WaitForSeconds(60);
        float rad = flareInfo.getRad1();
        Vector3 center = flareInfo.getCenter1();

        for(int i=0; i<5; i++)
        {
            Vector3 currPos = enemyInfo.enemyPositions[i];
            if ( Mathf.Pow(currPos.x - center.x, 2) + Mathf.Pow(currPos.y - center.y, 2) < Mathf.Pow(rad, 2) )
            {
                Debug.Log($"Captured enemy at: {currPos.x}, {currPos.y}"); 
            }
        }
    }

    void calculateCaptures()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
