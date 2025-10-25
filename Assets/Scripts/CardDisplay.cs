using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    [Header("Card References")]
    public PlayerCard cardData;
    
    [Header("UI Elements")]
    public Image cardBackground;
    public Image teamLogo;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI positionText;
    public TextMeshProUGUI overallRatingText;
    public TextMeshProUGUI teamNameText;
    
    [Header("Stat Displays")]
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defenseText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI skillText;
    public TextMeshProUGUI staminaText;
    
    [Header("Visual Elements")]
    public Image rarityGlow;
    public Image hexagonPattern;
    public Image playerImage;
    
    [Header("Animation")]
    public float flipSpeed = 1f;
    public AnimationCurve flipCurve;
    
    private bool isFlipped = false;
    
    void Start()
    {
        if (cardData != null)
        {
            UpdateCardDisplay();
        }
    }
    
    public void SetCardData(PlayerCard newCardData)
    {
        cardData = newCardData;
        UpdateCardDisplay();
    }
    
    void UpdateCardDisplay()
    {
        if (cardData == null) return;
        
        // Basic info
        if (playerNameText != null)
            playerNameText.text = cardData.playerName;
        
        if (positionText != null)
            positionText.text = cardData.position;
        
        if (overallRatingText != null)
            overallRatingText.text = cardData.overallRating.ToString();
        
        if (teamNameText != null)
            teamNameText.text = cardData.teamName;
        
        // Stats
        if (attackText != null)
            attackText.text = cardData.attack.ToString();
        
        if (defenseText != null)
            defenseText.text = cardData.defense.ToString();
        
        if (speedText != null)
            speedText.text = cardData.speed.ToString();
        
        if (skillText != null)
            skillText.text = cardData.skill.ToString();
        
        if (staminaText != null)
            staminaText.text = cardData.stamina.ToString();
        
        // Visual styling
        UpdateCardVisuals();
    }
    
    void UpdateCardVisuals()
    {
        // Card background color based on rarity
        if (cardBackground != null)
        {
            cardBackground.color = cardData.cardColor;
        }
        
        // Rarity glow effect
        if (rarityGlow != null)
        {
            rarityGlow.color = new Color(cardData.cardColor.r, cardData.cardColor.g, cardData.cardColor.b, 0.3f);
        }
        
        // Team logo
        if (teamLogo != null && cardData.teamLogo != null)
        {
            teamLogo.sprite = cardData.teamLogo;
        }
        
        // Player image
        if (playerImage != null && cardData.playerImage != null)
        {
            playerImage.sprite = cardData.playerImage;
        }
    }
    
    public void FlipCard()
    {
        if (!isFlipped)
        {
            StartCoroutine(FlipAnimation());
        }
    }
    
    IEnumerator FlipAnimation()
    {
        isFlipped = true;
        float time = 0;
        
        // Scale down to 0 on X axis
        while (time < flipSpeed / 2)
        {
            time += Time.deltaTime;
            float progress = time / (flipSpeed / 2);
            float scaleX = flipCurve.Evaluate(progress);
            transform.localScale = new Vector3(scaleX, 1, 1);
            yield return null;
        }
        
        // Change card content here if needed
        
        // Scale back up
        time = 0;
        while (time < flipSpeed / 2)
        {
            time += Time.deltaTime;
            float progress = time / (flipSpeed / 2);
            float scaleX = flipCurve.Evaluate(1 - progress);
            transform.localScale = new Vector3(scaleX, 1, 1);
            yield return null;
        }
        
        transform.localScale = Vector3.one;
        isFlipped = false;
    }
    
    public void OnCardClicked()
    {
        // Handle card click - could show details, add to deck, etc.
        Debug.Log($"Clicked on {cardData.playerName} from {cardData.teamName}");
        
        // Trigger flip animation
        FlipCard();
    }
    
    // Method to highlight card (for selection)
    public void HighlightCard(bool highlight)
    {
        if (highlight)
        {
            transform.localScale = Vector3.one * 1.1f;
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }
}
