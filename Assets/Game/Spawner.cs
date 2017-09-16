using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour, IDamageable
{


    public CardType cardType;
    public EnviromentTile TileOn;

    public EnviromentTile GetCurrentTile { get { return TileOn; } }
    public List<EnviromentTile> FindTilesAround(int distance) { return (terrainControl.FindMoveRange(TileOn, distance)); }
    public List<EnviromentTile> FindAttackRangeAround(EnviromentTile AttackTile, int AttackDistance) { return (terrainControl.FindAttackRange(AttackTile, AttackDistance)); }


    TerrainControl terrainControl;
    public List<EnviromentTile> TilesNear;
    LoseScreen loseScreen;
    WinScreen winScreen;

    [SerializeField]
    int maxHealthPoints = 100;
    public int currentHealthPoints;

    public float getCurrentHealth { get { return (float)currentHealthPoints; } }

    public void TakeDamage(int Damage, Transform attackerTransform)
    {
        BroadcastMessage("DamageDealt", Damage);
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - Damage, 0, maxHealthPoints);
        if (attackerTransform.GetComponent<CardObject>() != null) { attackerTransform.GetComponent<CardObject>().CombatOver(); }
        if (currentHealthPoints <= 0)
        {
            if (cardType == CardType.Player)
            {
                loseScreen.gameObject.SetActive(true);
            }
            else if (cardType == CardType.Enemy)
            {
                winScreen.gameObject.SetActive(true);
            }
        }
    }

    public void ResetTiles()
    {
        foreach (EnviromentTile tile in TilesNear)
        {
            tile.ChangeColor(tile.MatColorOriginal);
        }
    }

        // Use this for initialization
        void Start () {
        currentHealthPoints = maxHealthPoints;
        terrainControl = FindObjectOfType<TerrainControl>();
        EnviromentTile[] Tiles = FindObjectsOfType<EnviromentTile>();
        foreach (EnviromentTile tile in Tiles)
        {
            if (tile.ObjectHeld == this.gameObject) { TileOn = tile; }
        }
        if (cardType == CardType.Player)
        {
            loseScreen = FindObjectOfType<LoseScreen>();
            Debug.Log(loseScreen);
            loseScreen.gameObject.SetActive(false);
        }
        else if (cardType == CardType.Enemy)
        {
            winScreen = FindObjectOfType<WinScreen>();
            Debug.Log(winScreen);
            winScreen.gameObject.SetActive(false);
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
