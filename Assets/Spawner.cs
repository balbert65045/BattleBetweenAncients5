using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour, IDamageable
{


    public CardType cardType;
    public EnviromentTile TileOn;
    TerrainControl terrainControl;
    public List<EnviromentTile> TilesNear;

    [SerializeField]
    int maxHealthPoints = 100;
    public int currentHealthPoints;

    public float getCurrentHealth { get { return (float)currentHealthPoints; } }

    public void TakeDamage(int Damage, Transform attackerTransform)
    {
        BroadcastMessage("DamageDealt", Damage);
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - Damage, 0, maxHealthPoints);
        if (currentHealthPoints <= 0)
        {
            Debug.Log("GAME OVER");
        }
    }

        // Use this for initialization
        void Start () {
        currentHealthPoints = maxHealthPoints;
        terrainControl = FindObjectOfType<TerrainControl>();
        EnviromentTile[] Tiles = FindObjectsOfType<EnviromentTile>();
        foreach(EnviromentTile tile in Tiles)
        {
            if (tile.ObjectHeld == this.gameObject) { TileOn = tile; }
        }

    }

    public List<EnviromentTile> CheckTilesAround()
    {
        return (terrainControl.FindTilesOpenAround(TileOn));
    }
	
	// Update is called once per frame
	void Update () {
       // TilesNear = terrainControl.FindTilesOpenAround(TileOn);
    }
}
