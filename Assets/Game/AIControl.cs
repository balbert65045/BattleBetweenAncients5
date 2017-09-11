using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControl : MonoBehaviour {

     public int xGridLength = 10;
     public int zGridLength = 10;

    TurnSystem turnSystem;

    [SerializeField]
     GameObject[] enmyCardObjects;

    Spawner AISpawner;
    Spawner playerSpawner;
    public List<EnviromentTile> SpawnTiles;


    private EnviromentTile[] TilesTotal;
    private EnviromentTile[,] GridTiles;

    private CardObject SelectedCardObject;
    private CardObject[] cardsObjectsOut;
    private List<CardObject> enemyCardObjectsOut;
    private List<CardObject> playerCardObjectsOut;

    public List<EnviromentTile> Path;
    private List<EnviromentTile> AttackArea;
    private List<EnviromentTile> Range;

    

    bool enable = false;
    bool TurnOver = false;
    bool ObjectMoving = false;
    private int index;

    

    [SerializeField]
    float TimeBetweenNextObject = 1f;

    float TimeObjectStopedMoving;

    // Use this for initialization
    void Start () {

        Spawner[] spawners = FindObjectsOfType<Spawner>();
        foreach (Spawner spawner in spawners)
        {
            if (spawner.cardType == CardType.Enemy) { AISpawner = spawner; }
            else if (spawner.cardType == CardType.Player) { playerSpawner = spawner; }
        }

        TilesTotal = FindObjectsOfType<EnviromentTile>();
        turnSystem = FindObjectOfType<TurnSystem>();
        GridTiles = new EnviromentTile[xGridLength, zGridLength];
        foreach (EnviromentTile Tile in TilesTotal)
        {
            GridTiles[Tile.X, Tile.Z] = Tile;
        }
        

      //  GridTiles[5, 5].OnItemMake(enmyCardObjects[0]);

        playerCardObjectsOut = new List<CardObject>();
        enemyCardObjectsOut = new List<CardObject>();
    }

    public void SelectedObjectMoveStateChange(bool Moving)
    {

        if (Moving)
        {
            ObjectMoving = true;

        }
        else
        {
            ObjectMoving = false;
            SelectedCardObject.MoveChangeObservers -= SelectedObjectMoveStateChange;
        }

    }

    public void DeathChange(CardObject cardObject)
    {
        if (playerCardObjectsOut.Contains(cardObject)) { playerCardObjectsOut.Remove(cardObject); }
        if (enemyCardObjectsOut.Contains(cardObject)) { enemyCardObjectsOut.Remove(cardObject); }
        cardObject.DeathChangeObservers -= DeathChange;
    }


    // TODO NEED tell turnController turn is over when out of objects 
    public void Active()
    {
        enable = true;
        index = 0;

        // SPAWNING
        SpawnTiles = AISpawner.CheckTilesAround();
        int randomTile = Random.Range( 0, SpawnTiles.Count);
        int randomCard = Random.Range(0, enmyCardObjects.Length);
        Debug.Log(randomCard);
        SpawnTiles[randomTile].OnItemMake(enmyCardObjects[randomCard]);

        cardsObjectsOut = FindObjectsOfType<CardObject>();
        foreach (CardObject cardObject in cardsObjectsOut)
        {
            if (cardObject.cardType == CardType.Enemy) {
                if (!enemyCardObjectsOut.Contains(cardObject))
                {
                    enemyCardObjectsOut.Add(cardObject);
                    cardObject.DeathChangeObservers += DeathChange;
                }
            }


            else if (cardObject.cardType == CardType.Player) {
                if (!playerCardObjectsOut.Contains(cardObject)) {
                    playerCardObjectsOut.Add(cardObject);
                    cardObject.DeathChangeObservers += DeathChange;

                }
            }
        }

        foreach(CardObject cardObject in enemyCardObjectsOut)
        {
            cardObject.ResetAbilities();
        }

        if (enemyCardObjectsOut.Count > 0) { SelectObject(enemyCardObjectsOut[index]); }

    }
	
    void SelectObject(CardObject cardObject)
    {
        SelectedCardObject = cardObject;
        cardObject.MoveChangeObservers += SelectedObjectMoveStateChange;
        Range = cardObject.FindMoveRange();

        // check if there are any player objects out on the field
        if (playerCardObjectsOut.Count > 0)
        {
            // Find the closest player Object to attack
            CardObject ClosestPlayerCardObject = null;
            int minDistancePlayer = 100;

            foreach (CardObject cardObjectplayer in playerCardObjectsOut)
            {
                // Look for closest player available 
                if (cardObject.CheckIfPathAvailable(cardObjectplayer.GetCurrentTile))
                {
                    int Distance = cardObject.FindTileDistance(cardObjectplayer.GetCurrentTile);
                    if (Distance < minDistancePlayer)
                    {
                        minDistancePlayer = Distance;
                        ClosestPlayerCardObject = cardObjectplayer;
                    }
                }
            }

            // If there is no path to an available object to attack dont move 
            if (ClosestPlayerCardObject != null)
            {
                AttackArea = playerSpawner.FindTilesAround(cardObject.MaxAttackDistance);
                int minDistanceSpawner = 100;
                foreach (EnviromentTile tile in AttackArea)
                {
                    int Distance = cardObject.FindTileDistance(tile);
                    if (Distance < minDistanceSpawner)
                    {
                        minDistanceSpawner = Distance;
                    }
                }

                if (minDistancePlayer < minDistanceSpawner)
                {
                    MovetoClosestTileinAttackRange(cardObject, ClosestPlayerCardObject.GetCurrentTile);
                }
                else
                {
                    MovetoClosestTileinAttackRange(cardObject, playerSpawner.GetCurrentTile);
                }

              
            }

            // if no path available to the player objects go for the flag/spawner
            else
            {
                MovetoClosestTileinAttackRange(cardObject, playerSpawner.GetCurrentTile);
            }
        }
        // If no player is found then go for the spawner
        else
        {
            Debug.Log("Moving towards Spawner");
            MovetoClosestTileinAttackRange(cardObject, playerSpawner.GetCurrentTile);
        }

    }



    void MovetoClosestTileinAttackRange(CardObject cardObject, EnviromentTile TileofObject)
    {
        // Dont move if in attack range
        if (!cardObject.FindAttackRange().Contains(TileofObject))
        {
            if (TileofObject.ObjectHeld.GetComponent<CardObject>() == null)
            { AttackArea = TileofObject.ObjectHeld.GetComponent<Spawner>().FindTilesAround(cardObject.MaxAttackDistance); }
            else
            {
                AttackArea = TileofObject.ObjectHeld.GetComponent<CardObject>().FindTilesAround(cardObject.MaxAttackDistance);
            }
         
            EnviromentTile MoveTile = null;
            int minDistance = 100;
            foreach (EnviromentTile tile in AttackArea)
            {
                if (cardObject.CheckIfPathAvailable(tile))
                {
                    int Distance = cardObject.FindTileDistance(tile);
                    if (Distance < minDistance)
                    {
                        minDistance = Distance;
                        MoveTile = tile;
                    }
                }
            }
            Path = cardObject.MakePath(MoveTile);
            cardObject.enableMovement(Path);
        }
    } 



    void CheckforAttackAvailable()
    {
        Range = SelectedCardObject.FindAttackRange();
        if (Range.Count > 0)
        {
            EnviromentTile TileToAttack = null;
            foreach (EnviromentTile tile in Range)
            {
                if (tile.cardType == CardType.Player)
                {
                    TileToAttack = tile;
                }
            }
            if (TileToAttack != null) { SelectedCardObject.AttackObject(TileToAttack.ObjectHeld); }
        }
        TimeObjectStopedMoving = Time.time;
        TurnOver = true;
    }




	// Need to handle moving to next Card Object at apporpriate time 
	void Update () {
      if (enable)
        {
            if (!ObjectMoving && !TurnOver)
            {
                    CheckforAttackAvailable();
            }
            if (Time.time > TimeObjectStopedMoving + TimeBetweenNextObject && TurnOver)
            {
                TurnOver = false;
                index++;
                if (index <= enemyCardObjectsOut.Count - 1)
                {
                    SelectObject(enemyCardObjectsOut[index]);
                }
                else
                {
                    turnSystem.EndTurn(2);
                    enable = false;
                }
            }

        }

    }
}
