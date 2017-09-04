using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControl : MonoBehaviour {

    public static int xGridLength = 7;
    public static int zGridLength = 7;

    [SerializeField]
     GameObject[] enmyCardObjects;

    public EnviromentTile[] TilesTotal;
    public EnviromentTile[,] GridTiles;

    private CardObject[] cardsObjectsOut;
    public List<CardObject> enemyCardObjectsOut;
    public List<CardObject> playerCardObjectsOut;


    // Use this for initialization
    void Start () {
        TilesTotal = FindObjectsOfType<EnviromentTile>();
        GridTiles = new EnviromentTile[xGridLength, zGridLength];
        foreach (EnviromentTile Tile in TilesTotal)
        {
            GridTiles[Tile.X, Tile.Z] = Tile;
        }

        GridTiles[5, 5].OnItemMake(enmyCardObjects[0]);
    }


    public void Active()
    {
        cardsObjectsOut = FindObjectsOfType<CardObject>();
        enemyCardObjectsOut.Clear();
        foreach (CardObject cardObject in cardsObjectsOut)
        {
            if (cardObject.cardType == CardType.Enemy) { enemyCardObjectsOut.Add(cardObject); }
            else if (cardObject.cardType == CardType.Player) { playerCardObjectsOut.Add(cardObject); }
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (enemyCardObjectsOut.Count > 0)
        {
            foreach (CardObject cardObject in enemyCardObjectsOut)
            {
                ;
            }
        }

    }
}
