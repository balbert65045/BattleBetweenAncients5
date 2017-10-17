using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CardUser : MonoBehaviour {

    CameraRaycaster cameraRaycaster;
    EnviromentTile Tile;
    CardHand cardHand;
    EnviromentTile OldTileOver;
    Spawner playerSpawner;
    public List<EnviromentTile> SpawnTiles;
    PowerCounter powerCounter;

    // Use this for initialization
    void Start () {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.layerChangeObservers += CardShowUse;
        cardHand = FindObjectOfType<CardHand>();
        powerCounter = FindObjectOfType<PowerCounter>();
        Spawner[] spawners = FindObjectsOfType<Spawner>();
        foreach (Spawner spawner in spawners)
        {
            if (spawner.cardType == CardType.Player) { playerSpawner = spawner; }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (cardHand.CardUsing)
        {
            if (CrossPlatformInputManager.GetButtonUp("pointer1"))
            {
                if (cardHand.CardUsing.GetComponent<CardSummon>() != null)
                {
                    OnItemCreate();
                    OldTileOver = null;
                }
                else if (cardHand.CardUsing.GetComponent<CardSpell>() != null)
                {
                    if (OldTileOver != null)
                    {
                        onSpellUse();
                    }
                    OldTileOver = null;
                }
            }
            if (CrossPlatformInputManager.GetButtonDown("Cancel"))
            {
                if (OldTileOver != null)
                {
                    OldTileOver.DestroyImage();
                    OldTileOver = null;
                }
                SpawnTiles = playerSpawner.CheckTilesAround();
                foreach (EnviromentTile tile in SpawnTiles)
                {
                    tile.ChangeColor(tile.MatColorOriginal);
                }

            }
        }
    }

    void CardShowUse(Transform newTransform)
    {
        if (cardHand.CardUsing != null)
        {
            if (cardHand.CardUsing.GetComponent<CardSummon>() != null)
            {
               // Debug.Log("CreatingImage");
                OnItemCreateImage(newTransform);
            }
            else if (cardHand.CardUsing.GetComponent<CardSpell>() != null)
            {
                OnSpellShowUse(newTransform);
            }
        }
    }

    void OnSpellShowUse(Transform newTranform)
    {
        Debug.Log("Showing Spell Use");
        CardSpell Spell = cardHand.CardUsing.GetComponent<CardSpell>();
        EnviromentTile NewTile = newTranform.GetComponent<EnviromentTile>();
        switch (Spell.spellType)
        {
            case SpellType.Buff:
                if (OldTileOver != null) { OldTileOver.ChangeColor(OldTileOver.MatColorOriginal); }
                if (NewTile.cardType == CardType.Player && NewTile.ObjectHeld.GetComponent<CardObject>() != null)
                {
                    NewTile.ChangeColor(Color.cyan);
                }
                    break;
            case SpellType.DeBuff:
                break;
            default:
                Debug.LogWarning("No Spell Type attached to Spell");
                return;
        }
        OldTileOver = NewTile;

    }

    void OnItemCreateImage(Transform newTransform)
    {

        SpawnTiles = playerSpawner.CheckTilesAround();
        foreach (EnviromentTile tile in SpawnTiles)
        {
            // Debug.Log("ColorTiles");
            tile.ChangeColor(Color.cyan);
        }
        Tile = newTransform.GetComponent<EnviromentTile>();
        GameObject newItem = cardHand.CardUsing.GetComponent<CardSummon>().SummonImageObject;

        if (OldTileOver != null)
        {
        //    Debug.Log("DestroyedObject");
            OldTileOver.DestroyImage();
        }
        if (SpawnTiles.Contains(Tile))
        {
       //     Debug.Log("Tile In Spawn Area");
            Tile.OnItemMake(newItem);
            OldTileOver = Tile;
        }
     
    }

    void onSpellUse()
    {
        CardSpell Spell = cardHand.CardUsing.GetComponent<CardSpell>();
       
        if (OldTileOver != null) { OldTileOver.ChangeColor(OldTileOver.MatColorOriginal); }
        switch (Spell.spellType)
        {
            case SpellType.Buff:
                if (OldTileOver.cardType == CardType.Player && OldTileOver.ObjectHeld.GetComponent<CardObject>() != null)
                {
                    Spell.GiveBuff(OldTileOver.ObjectHeld.GetComponent<CardObject>());
                    powerCounter.RemovePower(cardHand.CardUsing.GetPowerAmount);
                    cardHand.DestroyCardUsed();
                }
                break;
            case SpellType.DeBuff:
                break;
            default:
                Debug.LogWarning("No Spell Type attached to Spell");
                return;
        }
    }

    void OnItemCreate()
    {
        if (cardHand.CardUsing)
        {
            OldTileOver.DestroyImage();
            if (SpawnTiles.Contains(Tile))
            {

                powerCounter.RemovePower(cardHand.CardUsing.GetPowerAmount);
                GameObject newItem = cardHand.CardUsing.GetComponent<CardSummon>().SummonObject;
                OldTileOver.OnItemMake(newItem);
                cardHand.DestroyCardUsed();

            }
            foreach (EnviromentTile tile in SpawnTiles)
            {
                // Debug.Log("resetTiles");
                tile.ChangeColor(tile.MatColorOriginal);
            }
        }
    }

}
