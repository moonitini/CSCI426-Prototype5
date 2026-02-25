using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private Tile tile_prefab;
    [SerializeField] private Transform cam;
    //private Tile targetScript = null; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateGrid(); 
    }

    void GenerateGrid()
    {
        for(int x=0; x<width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var spawnedTile = Instantiate(tile_prefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                //var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                //spawnedTile.Init(isOffset); 
            }
        }

        cam.position = new Vector3(((float)width / 2) - 0.5f, ((float)height / 2) - 0.5f, -10); 
    }
}
