using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;
    // native array
    public Vector3[] enemyPositions = new Vector3[5];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        StartCoroutine(highlightEnemies()); 
    }
    IEnumerator highlightEnemies()
    {
        for(int i=0; i<5; i++)
        {
            yield return new WaitForSeconds(1);
            highlightRandom();
            yield return new WaitForSeconds(3);
            clearEnemies();
        }
        
        
    }
    void highlightRandom()
    {
        for(int i=0; i<5; i++)
        {
            int x = Random.Range(0, 15);
            int y = Random.Range(0, 8);
            var currTile = GameObject.Find($"Tile {x} {y}");
            enemyPositions[i] = currTile.transform.position; 
            var toRender = currTile.GetComponent<SpriteRenderer>();
            toRender.color = Color.red;
        }
    }

    void clearEnemies()
    {
        for(int x=0; x<width; x++)
        {
            for(int y=0; y<height; y++)
            {
                var currTile = GameObject.Find($"Tile {x} {y}"); 
                var toRender = currTile.GetComponent<SpriteRenderer>();
                toRender.color = Color.cyan;  
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
