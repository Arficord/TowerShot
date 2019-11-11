using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static string name = "PLAYER";

    private static int _money=0;
    public static int money
    {
        get
        {
            return _money;
        }
        set
        {
            _money = value;
            if (onMoneyChange != null)
            {
                onMoneyChange();
            }
        }
    }
    
    public static event Action onMoneyChange;
    public static ShellStats shellBaseStats;
    public static Upgrades upgrades;
    public static Options options;

    private static float _maxHp;
    public static float maxHp
    {
        get
        {
            return _maxHp;
        }
        set
        {
            _maxHp = value;
            if (onHpChange != null)
            {
                onHpChange();
            }
        }
    }
    private static float _curHp;
    public static float curHp 
    {
        get
        {
            return _curHp;
        }
        set
        {
            _curHp = value;
            if(onHpChange!=null)
            {
                onHpChange();
            }
            if (_curHp < 0)
            {
                EventController.eventController.onDefeate();
            }
        }
    }
    public static event Action onHpChange;


    static GameData(){
        reset();
    }
    public static void reset()
    {
        options = new Options();
        initializeUpgrades();
        initializeShellStats();
        initializeTrainStats();

        onHpChange = null;
        onMoneyChange = null;
    }
    private static void initializeUpgrades()
    {
        upgrades = new Upgrades();
        upgrades.penetration_lvl = 0;
        upgrades.bounce_lvl = 0;
        upgrades.fullHpDamage_lvl = 0;
        upgrades.burn_lvl = 0;
        upgrades.deathExplosive_lvl = 0;
        upgrades.slow_lvl = 0;
    }
    private static void initializeShellStats(){

        shellBaseStats = new ShellStats();
        shellBaseStats.damage = 15;
        shellBaseStats.speed = 30;
        shellBaseStats.penetrations = upgrades.penetration_lvl;
        shellBaseStats.bounces = upgrades.bounce_lvl;
    }
    private static void initializeTrainStats()
    {
        maxHp = 1000;
        curHp = maxHp;
    }
}
