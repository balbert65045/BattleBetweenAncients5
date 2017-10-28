using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour
{
    public EnviromentTile GetCurrentTile { get { return cardObject.GetCurrentTile; } }
    public List<EnviromentTile> FindTilesAround(int distance) { return (terrainControl.FindMoveRange(cardObject.GetCurrentTile, distance)); }
    public List<EnviromentTile> FindAttackRangeAround(EnviromentTile AttackTile, int AttackDistance) { return (terrainControl.FindAttackRange(AttackTile, AttackDistance)); }


    TerrainControl terrainControl;
    CardObject cardObject;
   
    LoseScreen loseScreen;
    WinScreen winScreen;

    
    public void Death()
    {
        loseScreen.gameObject.SetActive(true);
    }



    // Use this for initialization
    void Start()
    {
        //currentHealthPoints = maxHealthPoints;
        cardObject = GetComponent<CardObject>();
        terrainControl = FindObjectOfType<TerrainControl>();
        EnviromentTile[] Tiles = FindObjectsOfType<EnviromentTile>();


        loseScreen = FindObjectOfType<LoseScreen>();
        winScreen = FindObjectOfType<WinScreen>();
        if (loseScreen != null) { loseScreen.gameObject.SetActive(false); }
        if (winScreen != null) { winScreen.gameObject.SetActive(false); }
    }

    public List<EnviromentTile> CheckTilesAround()
    {
        EnviromentTile TileOn = cardObject.GetCurrentTile;
        return (terrainControl.FindTilesOpenAround(TileOn));
    }

}
