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

    private CardObject[] cardsObjectsOut;
    public List<CardObject> enemyCardObjectsOut;
    private List<CardObject> playerCardObjectsOut;
    public CardObject SelectedCardObject;

    private List<EnviromentTile> Path;
    public List<EnviromentTile> AttackArea;
    private List<EnviromentTile> Range;

    public int index;
    bool spawn = false;
    //TODO BUG find why turn does not end (Happened when object hit by one move then killed by the next and in attack distance..)

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
        if (!spawn)
        {
            SpawnUnit();
            spawn = true;
        }

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
        SpawnTiles = AISpawner.CheckTilesAround();
        int randomTile = Random.Range(0, SpawnTiles.Count);
        int randomCard = Random.Range(0, enmyCardObjects.Length);
        SpawnTiles[randomTile].OnItemMake(enmyCardObjects[randomCard]);
    }

    // Control the card object to attack the closest object thats attackable
    void SelectObject(CardObject cardObject)
    {
        index++;
        SelectedCardObject = cardObject;
        cardObject.MoveChangeObservers += SelectedObjectMoveStateChange;
        cardObject.CombatChangeObservers += SelectedObjectCombatChange;

        Range = cardObject.FindMoveRange();

        // check if there are any player objects out on the field
        if (playerCardObjectsOut.Count > 0)
        {
            // Find the closest player Object to attack
            CardObject ClosestPlayerCardObject = null;
            int minDistancePlayer = 100;
            EnviromentTile ClosestPlayerTile = null;

            foreach (CardObject cardObjectplayer in playerCardObjectsOut)
            {
                AttackArea = cardObjectplayer.FindTilesAround(cardObject.MaxAttackDistance);
                Debug.Log("Looking at attack area");
                foreach (EnviromentTile tile in AttackArea)
                {
                        int Distance = cardObject.FindTileDistance(tile);
                        if (Distance < minDistancePlayer)
                        {
                            minDistancePlayer = Distance;
                            ClosestPlayerCardObject = cardObjectplayer;
                            ClosestPlayerTile = tile;
                        }
                }
            }
            Debug.Log(ClosestPlayerTile);
            Debug.Log(minDistancePlayer);

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
                    MoveToTile(cardObject, ClosestPlayerTile, ClosestPlayerCardObject);
                    return;
                }
            }
 
        }
        // if no path available to the player objects go for the flag/spawner
        // If no player is found then go for the spawner

     MovetoClosestTileinAttackRange(cardObject, playerSpawner.GetCurrentTile);

    }



    void MoveToTile(CardObject cardObject, EnviromentTile tileToMove, CardObject ClosestPlayer)
    {
        Debug.Log("Looking for movement");
        if (!cardObject.FindAttackRange().Contains(ClosestPlayer.GetCurrentTile))
        {
            Debug.Log("Moving");
            Path = cardObject.MakePath(tileToMove);
            cardObject.enableMovement(Path);
            return;
        }
        Debug.Log("Attacking");
        CheckforAttackAvailable(cardObject);
    }

    // TODO Find movement to Spawner Bug
    void MovetoClosestTileinAttackRange(CardObject cardObject, EnviromentTile TileofObject)
    {
        // Dont move if in attack range, just attack 
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
            Debug.Log("Moving");
            Path = cardObject.MakePath(MoveTile);
            cardObject.enableMovement(Path);
        }
        // Look for something to attack 
        else {
            CheckforAttackAvailable(cardObject);
        }
    } 


    // Look for Something to attack 
    void CheckforAttackAvailable(CardObject SelectedCardObject)
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
            if (TileToAttack != null)
            {
                Debug.Log("InCombat");
                SelectedCardObject.EngageCombat(CombatType.Attack, TileToAttack.ObjectHeld);
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
