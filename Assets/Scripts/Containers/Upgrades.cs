using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public struct Upgrades
{
    public enum UpgradesEnum
    {
        PENETRATION,
        BOUNCE,
        FULL_HP_DAMAGE,
        BURN,
        DEATH_EXPLOSIVE,
        SLOW,
    }

    #region penetration
    public int penetration_lvl; // + 1 пробитый противник (урон уменьшаеться так что-бы последнему пробитому наносилось 30% урона)
    public const float penetration_damageToLast = 0.3f;
    public const int penetration_BasePrice = 80;
    #endregion
    #region bounce
    public int bounce_lvl;      // + 1 отражение от стенки
    public const int bounce_BasePrice = 95;
    #endregion
    #region fullHpDamage
    public int fullHpDamage_lvl; // +60% урона против противников с >90% HP
    public const float fullHpDamage_damageMultiplier = 1.6f;
    public const float fullHpDamage_startHpMin = 1;
    public const float fullHpDamage_HpMinDecimal = 0.05f;
    public const int fullHpDamage_BasePrice = 120;
    #endregion
    #region burn
    public int burn_lvl; //+10% поджечь противника нанося 10% урона 5 секж
    public const float burn_tickDelay = 1f;
    public const float burn_startChance=10;
    public const float burn_plusChance = 2;
    public const float burn_startDamagePerTickMultiplier = 0.6f;
    public const float burn_plusDamagePerTickMultiplier = 0.6f;
    public const float burn_duration = 5;
    public const int burn_BasePrice = 20;
    #endregion
    #region deathExplosive
    public int deathExplosive_lvl;
    public const float deathExplosive_baseRadius = 1.9f;
    public const float deathExplosive__radiusPerLvl = 0.1f;
    public const float deathExplosive_baseDamage = 0.7f;
    public const float deathExplosive__damagePerLvl = 0.1f;
    public const int deathExplosive__BasePrice = 85;
    #endregion
    #region slow
    public int slow_lvl;
    public const float slow_basePercente = 1;
    public const float slow_DescrisePerLvl = 0.96f;
    public const float slow_baseSlowChance = 10;
    public const float slow_slowChancePerLvl = 1;
    public const int slow_basePrice = 130;

    public static float slow_getSlowAmount(int lvl)
    {
        return slow_basePercente * Mathf.Pow(slow_DescrisePerLvl, lvl);
    }
    public float slow_getSlowAmount()
    {
        return slow_getSlowAmount(slow_lvl);
    }
    public static float slow_getSlowChance(int lvl)
    {
        return slow_basePercente + slow_slowChancePerLvl * lvl;

    }
    public float slow_getSlowChance()
    {
        return slow_getSlowChance(slow_lvl);
    }
    //TODO
    #endregion
    #region healthRegeneration_DONT_INCLUDED_IF_WHAT_JUST_DELETE
    public int healthRegeneration_lvl;
    public const float healthRegeneration_timeDelay = 5;
    public const float healthRegeneration_baseAmount = 0.1f;
    public const float healthRegeneration_amountPerLvl = 0.1f;

    public static float healthRegeneration_getRegen(int lvl)
    {
        return healthRegeneration_baseAmount + healthRegeneration_amountPerLvl * lvl;
    }
    public float healthRegeneration_getRegen()
    {
        return healthRegeneration_getRegen(healthRegeneration_lvl);
    }
    #endregion
    public void upgradeItem( UpgradesEnum item)
    {
        switch(item)
        {
            case UpgradesEnum.PENETRATION:
                penetration_lvl++;
                GameData.shellBaseStats.penetrations = GameData.upgrades.penetration_lvl;
                break;
            case UpgradesEnum.BOUNCE:
                bounce_lvl++;
                GameData.shellBaseStats.bounces = GameData.upgrades.bounce_lvl;
                break;
            case UpgradesEnum.FULL_HP_DAMAGE:
                fullHpDamage_lvl++;
                break;
            case UpgradesEnum.BURN:
                burn_lvl++;
                break;
            case UpgradesEnum.DEATH_EXPLOSIVE:
                deathExplosive_lvl++;
                break;
            case UpgradesEnum.SLOW:
                slow_lvl++;
                break;
        }
    }
    public string getItemDescription(UpgradesEnum item, int level)
    {
        switch (item)
        {
            case UpgradesEnum.PENETRATION:
                return "Penetrate " +level+ " enemies, after penetration damage lower by " + Math.Round((1-Mathf.Pow(penetration_damageToLast, 1.0f / level))*100, 1) + "%";
            case UpgradesEnum.BOUNCE:
                return "Shell will bounce walls " + level + " times";
            case UpgradesEnum.FULL_HP_DAMAGE:
                return "Inflict " + (fullHpDamage_damageMultiplier * level)*100 + "% damage to enemies with higher then "+ ((fullHpDamage_startHpMin - level * fullHpDamage_HpMinDecimal)*100) + "% HP.";
            case UpgradesEnum.BURN:
                return "With " + (burn_startChance + burn_plusChance * level) + "% burn an enemy to inflict " + (burn_startDamagePerTickMultiplier + burn_plusDamagePerTickMultiplier * level)*100 + "% by tick in " + burn_duration + " secounds.";
            case UpgradesEnum.DEATH_EXPLOSIVE:
                return "On die enemy explode on " + (deathExplosive_baseRadius + deathExplosive__radiusPerLvl * level) + " m to inflict " + (deathExplosive_baseDamage + deathExplosive__damagePerLvl * level)*100 + "% damage.";
            case UpgradesEnum.SLOW:
                return "With " + slow_getSlowChance(level) + "%  slow enemy on hit by " + (1 - slow_getSlowAmount(level)) * 100 + "%";
            default:
                return "Desctription unfound!";
        }
    }
    public string getItemDescription(UpgradesEnum item)
    {
        return getItemDescription(item, getItemLevel(item));
    }
    public string getItemNextLevelDescription(UpgradesEnum item)
    {
        return getItemDescription(item, getItemLevel(item)+1);
    }
    public int getItemLevel(UpgradesEnum item)
    {
        switch (item)
        {
            case UpgradesEnum.PENETRATION:
                return penetration_lvl;
            case UpgradesEnum.BOUNCE:
                return bounce_lvl;
            case UpgradesEnum.FULL_HP_DAMAGE:
                return fullHpDamage_lvl;
            case UpgradesEnum.BURN:
                return burn_lvl; 
            case UpgradesEnum.DEATH_EXPLOSIVE:
                return deathExplosive_lvl;
            case UpgradesEnum.SLOW:
                return slow_lvl;
            default:
                return 0;
        }
    }
    public int getItemBasePrice(UpgradesEnum item)
    {
        switch (item)
        {
            case UpgradesEnum.PENETRATION:
                return penetration_BasePrice;
            case UpgradesEnum.BOUNCE:
                return bounce_BasePrice;
            case UpgradesEnum.FULL_HP_DAMAGE:
                return fullHpDamage_BasePrice;
            case UpgradesEnum.BURN:
                return burn_BasePrice;
            case UpgradesEnum.DEATH_EXPLOSIVE:
                return deathExplosive__BasePrice;
            case UpgradesEnum.SLOW:
                return slow_basePrice;
            default:
                return 0;
        }
    }
    public UpgradesEnum getRandomUpgrade()
    {
        Array values = Enum.GetValues(typeof(UpgradesEnum));
        return (UpgradesEnum)values.GetValue(Random.Range(0, values.Length));
    }
}
