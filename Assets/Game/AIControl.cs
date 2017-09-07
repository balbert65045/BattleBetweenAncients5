using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControl : MonoBehaviour {

     static int xGridLength = 7;
     static int zGridLength = 7;

    TurnSystem turnSystem;

    [SerializeField]
     GameObject[] enmyCardObjects;

    private EnviromentTile[] TilesTotal;
    private EnviromentTile[,] GridTiles;

    private CardObject SelectedCardObject;
    private CardObject[] cardsObjectsOut;
    private List<CardObject> enemyCardObjectsOut;
    private List<CardObject> playerCardObjectsOut;

    private List<EnviromentTile> Path;
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
        TilesTotal = FindObjectsOfType<EnviromentTile>();
        turnSystem = FindObjectOfType<TurnSystem>();
        GridTiles = new EnviromentTile[xGridLength, zGridLength];
        foreach (EnviromentTile Tile in TilesTotal)
        {
            GridTiles[Tile.X, Tile.Z] = Tile;
        }

        GridTiles[5, 5].OnItemMake(enmyCardObjects[0]);

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
            CardObject ClosestPlayerCardObject = playerCardObjectsOut[0];
            foreach (CardObject cardObjectplayer in playerCardObjectsOut)
            {
                float MinDistance = (ClosestPlayerCardObject.transform.position - cardObject.transform.position).magnitude;
                float Distance = (cardObjectplayer.transform.position - cardObject.transform.position).magnitude;
                if (Distance < MinDistance) { ClosestPlayerCardObject = cardObjectplayer; }
            }

            //Dont move if already in attacking distance
            if (!cardObject.FindAttackRange().Contains(ClosestPlayerCardObject.GetCurrentTile))
            {
                Debug.Log("Finding tiles around " + ClosestPlayerCardObject);
                AttackArea = ClosestPlayerCardObject.FindTilesAround(cardObject.MaxAttackDistance);
                if (AttackArea.Count > 0)
                {
                    {
                        EnviromentTile MoveTile = AttackArea[0];
                        int minDistance = cardObject.FindTileDistance(MoveTile);
                        foreach (EnviromentTile tile in AttackArea)
                        {
                            int Distance = cardObject.FindTileDistance(tile);
                            if (Distance < minDistance) { MoveTile = tile; }
                        }
                        Path = cardObject.MakePath(MoveTile);
                        cardObject.enableMovement(Path);
                    }
                }
            }
            else
            {
            //    Debug.Log("Waiting...");
                TimeObjectStopedMoving = Time.time;
            }
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
