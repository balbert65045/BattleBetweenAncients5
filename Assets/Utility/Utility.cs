﻿public enum Layer{
    LevelTerrain = 8,
    RaycastEndStop = -1
}

public enum CardType
{
    Open = 1,
    Player = 2,
    Enemy = 3,
    Terrain = 4,
}

public enum CardState
{
    Move = 1,
    Attack = 2,
}


public enum GameType
{
    Survival = 0,
    Defense = 1,
    Attack = 2, 
}

public enum CombatType
{
    Defend = 1,
    Attack = 2,
    OutOfCombat = 3,
}

public enum SpellType
{
    Buff = 1,
    DeBuff = 2,

}


//Make a better way of working with this
public enum CardSummonName
{
    Peasant = 0,
    CrossBow_Soldier = 1,
    Warrior = 2,
    Knight = 3,

}

public enum CardSpellName
{
    DamageBuff = 0,
    Heal = 1,
    MoveBuff = 2,
}


