using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCard
{
    [Header("Basic Info")]
    public string playerName;
    public string teamName;
    public string position; // GK, CB, LB, RB, CDM, CM, CAM, LW, RW, ST
    public int overallRating;
    public CardRarity rarity;
    
    [Header("Stats")]
    public int attack;
    public int defense;
    public int speed;
    public int skill;
    public int stamina;
    
    [Header("Visual")]
    public Sprite playerImage;
    public Sprite teamLogo;
    public Color cardColor;
    
    [Header("Special")]
    public string specialAbility;
    public bool isCaptain;
    
    public PlayerCard(string name, string team, string pos, int overall, CardRarity cardRarity)
    {
        playerName = name;
        teamName = team;
        position = pos;
        overallRating = overall;
        rarity = cardRarity;
        
        // Generate stats based on overall rating
        GenerateStats();
        SetCardColor();
    }
    
    private void GenerateStats()
    {
        // Base stats around overall rating with some variation
        int baseStat = overallRating;
        int variation = Random.Range(-3, 4);
        
        attack = Mathf.Clamp(baseStat + variation, 1, 99);
        defense = Mathf.Clamp(baseStat + variation, 1, 99);
        speed = Mathf.Clamp(baseStat + variation, 1, 99);
        skill = Mathf.Clamp(baseStat + variation, 1, 99);
        stamina = Mathf.Clamp(baseStat + variation, 1, 99);
    }
    
    private void SetCardColor()
    {
        switch (rarity)
        {
            case CardRarity.Common:
                cardColor = new Color(0.7f, 0.7f, 0.7f, 1f); // Gray
                break;
            case CardRarity.Rare:
                cardColor = new Color(0.2f, 0.8f, 0.2f, 1f); // Green
                break;
            case CardRarity.Epic:
                cardColor = new Color(0.6f, 0.2f, 0.8f, 1f); // Purple
                break;
            case CardRarity.Legendary:
                cardColor = new Color(1f, 0.8f, 0.2f, 1f); // Gold
                break;
        }
    }
}

public enum CardRarity
{
    Common,
    Rare,
    Epic,
    Legendary
}
