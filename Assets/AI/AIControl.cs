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
    AISpawnSystem[] aiSpawnSystem;
    //Spawner playerSpawner;
    Mage mageSpawner;
    public List<EnviromentTile> SpawnTiles;


    private EnviromentTile[] TilesTotal;
    private EnviromentTile[,] GridTiles;

    private CardObject[] cardsObjectsOut;
    public List<CardObject> enemyCardObjectsOut;
    private List<CardObject> playerCardObjectsOut;
    public CardObject SelectedCardObject;

    private List<EnviromentTile> Path;
    public List<EnviromentTile> AttackArea;
    public List<EnviromentTile> AttackAreaAroundPlayer;
    public List<EnviromentTile> Range;

    public int index;
    bool spawn = false;
    //TODO BUG find why turn does not end (Happened when object hit by one move then killed by the next and in attack distance..)

    // Use this for initialization
    void Start () {

        Spawner[] spawners = FindObjectsOfType<Spawner>();
       
        foreach (Spawner spawner in spawners)
        {
            if (spawner.cardType == CardType.Enemy) { AISpawner = spawner; }
          //  else if (spawner.cardType == CardType.Player) { playerSpawner = spawner; }
        }

        mageSpawner = FindObjectOfType<Mage>();

        aiSpawnSystem = FindObjectsOfType<AISpawnSystem>();

        TilesTotal = FindObjectsOfType<EnviromentTile>();
        turnSystem = FindObjectOfType<TurnSystem>();
        GridTiles = new EnviromentTile[xGridLength, zGridLength];
        foreach (EnviromentTile Tile in TilesTotal)
        {
            GridTiles[Tile.X, Tile.Z] = Tile;
        }
        

        playerCardObjectsOut = new List<CardObject>();
        enemyCardObjectsOut = new List<CardObject>();
    }

    public void SelectedObjectMoveStateChange(bool Moving, CardObject cardObject)
    {
        if (!Moving)
        {
            if (SelectedCardObject == cardObject)
            {
                // Look for something to attack 
                CheckforAttackAvailable(cardObject);
            }
        }
    }

    public void DeathChange(CardObject cardObject)
    {
        if (playerCardObjectsOut.Contains(cardObject)) { playerCardObjectsOut.Remove(cardObject); }
        if (enemyCardObjectsOut.Contains(cardObject)) { enemyCardObjectsOut.Remove(cardObject); }
        cardObject.DeathChangeObservers -= DeathChange;
        if (cardObject == SelectedCardObject) {
            // Decrease the index since the size has now changed
            // and Select a new object if this is AI turn 
            if (turnSystem.AITurn)
            {
                index--;
                SelectNextObject();
            }
        }
    }

    public void SelectedObjectCombatChange(bool inCombat, CardObject cardObject)
    {

        if (!inCombat)
        {
            if (SelectedCardObject == cardObject)
            {
                // Find Next Object after attacking
                SelectNextObject();
            }
        }
    }


   // Once AI Control has recognized it is its turn
    public void Active()
    {
        index = 0;
        SpawnUnit();
        //if (!spawn)
        //{
        //    SpawnUnit();
        //    spawn = true;
        //}

        // Add Spawn Units to characters to control list
        cardsObjectsOut = FindObjectsOfType<CardObject>();
        foreach (CardObject cardObject in cardsObjectsOut)
        {
            if (cardObject.cardType == CardType.Enemy)
            {
                if (!enemyCardObjectsOut.Contains(cardObject))
                {
                    enemyCardObjectsOut.Add(cardObject);
                    cardObject.DeathChangeObservers += DeathChange;
                }
            }

            // Add Player Units to characters to control list
            else if (cardObject.cardType == CardType.Player)
            {
                if (!playerCardObjectsOut.Contains(cardObject))
                {
                    playerCardObjectsOut.Add(cardObject);
                    cardObject.DeathChangeObservers += DeathChange;
                }
            }
        }

        // Reset Every Card Movement and Attack 
        foreach (CardObject cardObject in enemyCardObjectsOut)
        {
            cardObject.ResetAbilities();
        }

        //Begin to Control all enemy Objects 
        SelectNextObject();


    }

    private void SpawnUnit()
    {
        // SPAWN Units

        // Check for Enemy Base Flag
        if (AISpawner != null)
        {
            SpawnTiles = AISpawner.CheckTilesAround();
            int randomTile = Random.Range(0, SpawnTiles.Count);
            int randomCard = Random.Range(0, enmyCardObjects.Length);
            SpawnTiles[randomTile].OnItemMake(enmyCardObjects[randomCard]);
        }

        // Check for Spawn Points
        foreach (AISpawnSystem SpawnPoint in aiSpawnSystem)
        {
            for (int i = 0; i < SpawnPoint.Spawns.Length; i++)
            {
                if (SpawnPoint.Spawns[i].Time == turnSystem.TurnCount)
                {
                    foreach (GameObject SpawnObject in SpawnPoint.Spawns[i].Objects)
                    {
                        SpawnTiles = SpawnPoint.CheckTilesAround();
                        int randomTile = Random.Range(0, SpawnTiles.Count);
                        int randomCard = Random.Range(0, enmyCardObjects.Length);
                        SpawnTiles[randomTile].OnItemMake(SpawnObject);
                    }
                }
            }
        }
    }

    // Control the card object to attack the closest object thats attackable
    void SelectObject(CardObject cardObject)
    {
        index++;
        SelectedCardObject = cardObject;
        Debug.Log("Selected " + SelectedCardObject.name + " " + index);
        cardObject.MoveChangeObservers += SelectedObjectMoveStateChange;
        cardObject.CombatChangeObservers += SelectedObjectCombatChange;

        Range = cardObject.FindMoveRange();

        EnviromentTile ClosestPlayerTile = null;
        EnviromentTile ClosestSpawnerTile = null;

        // if in attack range of the spawner attack it
        AttackArea = mageSpawner.FindAttackRangeAround(mageSpawner.GetCurrentTile, cardObject.MaxAttackDistance);
        if (AttackArea.Contains(cardObject.GetCurrentTile))
        {
            CheckforAttackAvailable(cardObject);
            return;
        }


        // check if there are any player objects out on the field
        if (playerCardObjectsOut.Count > 0)
        {
            // Find the closest player Object to attack
            int minDistancePlayer = 100;

            foreach (CardObject cardObjectplayer in playerCardObjectsOut)
            {
                AttackArea = cardObjectplayer.FindAttackRangeAround(cardObjectplayer.GetCurrentTile, cardObject.MaxAttackDistance);
                AttackAreaAroundPlayer = AttackArea;

                // CheckforPlayerinAttackArea
                if (AttackArea.Contains(cardObject.GetCurrentTile))
                {
                    CheckforAttackAvailable(cardObject);
                    return;
                }

                int Distance = 100;
                EnviromentTile tile = FindClosestTileforCard(cardObject, cardObjectplayer);
                Debug.Log(tile);
                //Make sure a tile is available to make a path 
                if (tile != null) { Distance = cardObject.FindTileDistance(tile); }
                if (Distance < minDistancePlayer)
                {
                    //Set new minDistance and new closest tile 
                    minDistancePlayer = Distance;
                    ClosestPlayerTile = tile;
                }
            }
          //  Debug.Log(ClosestPlayerTile);
         //   Debug.Log(minDistancePlayer);
            // If there is no path to an available object to attack dont move 

            if (ClosestPlayerTile != null)
            {
                int minDistanceSpawner = 100;
                ClosestSpawnerTile = FindClosestTileforSpawner(cardObject, mageSpawner);
                //Make sure a tile is available to make a path 
                if (ClosestSpawnerTile != null) { minDistanceSpawner = cardObject.FindTileDistance(ClosestSpawnerTile); }
                if (minDistancePlayer < minDistanceSpawner)
                {
                    MoveToTile(cardObject, ClosestPlayerTile);
                    return;
                }
                else
                {
                    MoveToTile(cardObject, ClosestSpawnerTile);
                    return;
                }
            }
 
        }
        // if no path available to the player objects go for the flag/spawner
        // If no player is found then go for the spawner
        ClosestSpawnerTile = FindClosestTileforSpawner(cardObject, mageSpawner);
        if (ClosestSpawnerTile != null) { MoveToTile(cardObject, ClosestSpawnerTile); }
        else { SelectNextObject(); }

    }

    EnviromentTile FindClosestTileforSpawner(CardObject cardObject, Mage mageSpawner)
    {
        AttackArea = mageSpawner.FindAttackRangeAround(mageSpawner.GetCurrentTile, cardObject.MaxAttackDistance);
        int minDistancePlayer = 100;
        EnviromentTile ClosestSpawnerrTile = null;
        foreach (EnviromentTile tile in AttackArea)
        {
            if (tile.cardType == CardType.Open)
            {
                int Distance = cardObject.FindTileDistance(tile);
                // If Distance is 0 then a path is not available
                if (Distance < minDistancePlayer && Distance > 0)
                {
                    minDistancePlayer = Distance;
                    ClosestSpawnerrTile = tile;
                }
            }
        }
        return (ClosestSpawnerrTile);
    }



    EnviromentTile FindClosestTileforCard(CardObject cardObject, CardObject cardObjectAgainst)
    {
        AttackArea = cardObjectAgainst.FindAttackRangeAround(cardObjectAgainst.GetCurrentTile, cardObject.MaxAttackDistance);
        int minDistancePlayer = 100;
        EnviromentTile ClosestPlayerTile = null;
        foreach (EnviromentTile tile in AttackArea)
        {
            if (tile.cardType == CardType.Open)
            {
                int Distance = cardObject.FindTileDistance(tile);
                // If Distance is 0 then a path is not available
                if (Distance < minDistancePlayer && Distance > 0)
                {
                    minDistancePlayer = Distance;
                    ClosestPlayerTile = tile;
                }
            }
        }
        return (ClosestPlayerTile);
    } 


    void MoveToTile(CardObject cardObject, EnviromentTile tileToMove)
    {
         //   Debug.Log("Moving");
      //  Debug.Log(tileToMove);
            Path = cardObject.MakePath(tileToMove);
            cardObject.enableMovement(Path);
            return;
    }


    // Look for Something to attack 
    void CheckforAttackAvailable(CardObject cardObject)
    {
        Range = cardObject.FindAttackRange();
        Debug.Log(Range.Count);
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
            Debug.Log(TileToAttack);
            if (TileToAttack != null)
            {
                Debug.Log("InCombat");
                cardObject.EngageCombat(CombatType.Attack, TileToAttack.ObjectHeld);
                return;
            }
        }
        SelectNextObject();
    }


    void SelectNextObject()
    {
      
        if (SelectedCardObject != null)
        {
            SelectedCardObject.CombatChangeObservers -= SelectedObjectCombatChange;
            SelectedCardObject.MoveChangeObservers -= SelectedObjectMoveStateChange;
        }
       
        // Select next object available 
        if (index + 1 <= enemyCardObjectsOut.Count)
        {
            SelectObject(enemyCardObjectsOut[index]);
        }
        // If no next object available end turn 
        else
        {
            Debug.Log("TurnOver");
            turnSystem.EndTurn(2);
        }
    }

}
