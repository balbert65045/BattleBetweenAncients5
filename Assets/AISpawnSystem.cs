using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawnSystem : MonoBehaviour {

    
    public Spawns[] Spawns;
    public EnviromentTile TileOn;
    TerrainControl terrainControl;

    private void Start()
    {
        terrainControl = FindObjectOfType<TerrainControl>();
        int layerMask = 1 << (int)Layer.LevelTerrain;
        RaycastHit hit;
        bool hasHit = Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 3f, layerMask);
        if (hasHit)
        {
            TileOn = hit.transform.GetComponent<EnviromentTile>();
        }
        else
        {
            Debug.LogWarning("Spawner not over an enviroment tile");
        }
    }

    public List<EnviromentTile> CheckTilesAround()
    {
        return (terrainControl.FindTilesOpenAround(TileOn));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + Vector3.up, new Vector3(2, 2, 2));
        
    }
}
