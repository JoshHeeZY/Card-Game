using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectionManager : MonoBehaviour
{
    [Header("UI References")]
    public Transform cardGridParent;
    public GameObject cardPrefab;
    public ScrollRect scrollRect;
    
    [Header("Filtering")]
    public TMP_Dropdown teamFilter;
    public TMP_Dropdown rarityFilter;
    public TMP_Dropdown positionFilter;
    public TMP_InputField searchField;
    
    [Header("Collection Display")]
    public TextMeshProUGUI collectionCountText;
    public TextMeshProUGUI totalCardsText;
    
    private List<PlayerCard> allPlayerCards = new List<PlayerCard>();
    private List<PlayerCard> filteredCards = new List<PlayerCard>();
    private List<GameObject> cardUIObjects = new List<GameObject>();
    
    void Start()
    {
        InitializeCollection();
        SetupFilters();
        RefreshCollection();
    }
    
    void InitializeCollection()
    {
        if (CardManager.Instance != null)
        {
            allPlayerCards = CardManager.Instance.GetPlayerCollection();
        }
        
        UpdateCollectionStats();
    }
    
    void SetupFilters()
    {
        // Setup team filter
        if (teamFilter != null)
        {
            teamFilter.options.Clear();
            teamFilter.options.Add(new TMP_Dropdown.OptionData("All Teams"));
            
            foreach (string team in CardManager.Instance.teamNames)
            {
                teamFilter.options.Add(new TMP_Dropdown.OptionData(team));
            }
            
            teamFilter.onValueChanged.AddListener(OnFilterChanged);
        }
        
        // Setup rarity filter
        if (rarityFilter != null)
        {
            rarityFilter.options.Clear();
            rarityFilter.options.Add(new TMP_Dropdown.OptionData("All Rarities"));
            rarityFilter.options.Add(new TMP_Dropdown.OptionData("Common"));
            rarityFilter.options.Add(new TMP_Dropdown.OptionData("Rare"));
            rarityFilter.options.Add(new TMP_Dropdown.OptionData("Epic"));
            rarityFilter.options.Add(new TMP_Dropdown.OptionData("Legendary"));
            
            rarityFilter.onValueChanged.AddListener(OnFilterChanged);
        }
        
        // Setup position filter
        if (positionFilter != null)
        {
            positionFilter.options.Clear();
            positionFilter.options.Add(new TMP_Dropdown.OptionData("All Positions"));
            positionFilter.options.Add(new TMP_Dropdown.OptionData("GK"));
            positionFilter.options.Add(new TMP_Dropdown.OptionData("CB"));
            positionFilter.options.Add(new TMP_Dropdown.OptionData("LB"));
            positionFilter.options.Add(new TMP_Dropdown.OptionData("RB"));
            positionFilter.options.Add(new TMP_Dropdown.OptionData("CDM"));
            positionFilter.options.Add(new TMP_Dropdown.OptionData("CM"));
            positionFilter.options.Add(new TMP_Dropdown.OptionData("CAM"));
            positionFilter.options.Add(new TMP_Dropdown.OptionData("LW"));
            positionFilter.options.Add(new TMP_Dropdown.OptionData("RW"));
            positionFilter.options.Add(new TMP_Dropdown.OptionData("ST"));
            
            positionFilter.onValueChanged.AddListener(OnFilterChanged);
        }
        
        // Setup search field
        if (searchField != null)
        {
            searchField.onValueChanged.AddListener(OnFilterChanged);
        }
    }
    
    void OnFilterChanged(string value)
    {
        RefreshCollection();
    }
    
    void OnFilterChanged(int value)
    {
        RefreshCollection();
    }
    
    public void RefreshCollection()
    {
        // Clear existing UI objects
        foreach (GameObject cardUI in cardUIObjects)
        {
            if (cardUI != null)
                DestroyImmediate(cardUI);
        }
        cardUIObjects.Clear();
        
        // Apply filters
        filteredCards = ApplyFilters(allPlayerCards);
        
        // Create card UI objects
        CreateCardUIObjects();
        
        UpdateCollectionStats();
    }
    
    List<PlayerCard> ApplyFilters(List<PlayerCard> cards)
    {
        List<PlayerCard> filtered = new List<PlayerCard>(cards);
        
        // Team filter
        if (teamFilter != null && teamFilter.value > 0)
        {
            string selectedTeam = teamFilter.options[teamFilter.value].text;
            filtered.RemoveAll(card => card.teamName != selectedTeam);
        }
        
        // Rarity filter
        if (rarityFilter != null && rarityFilter.value > 0)
        {
            string selectedRarity = rarityFilter.options[rarityFilter.value].text;
            CardRarity rarity = (CardRarity)System.Enum.Parse(typeof(CardRarity), selectedRarity);
            filtered.RemoveAll(card => card.rarity != rarity);
        }
        
        // Position filter
        if (positionFilter != null && positionFilter.value > 0)
        {
            string selectedPosition = positionFilter.options[positionFilter.value].text;
            filtered.RemoveAll(card => card.position != selectedPosition);
        }
        
        // Search filter
        if (searchField != null && !string.IsNullOrEmpty(searchField.text))
        {
            string searchTerm = searchField.text.ToLower();
            filtered.RemoveAll(card => !card.playerName.ToLower().Contains(searchTerm));
        }
        
        return filtered;
    }
    
    void CreateCardUIObjects()
    {
        if (cardPrefab == null || cardGridParent == null)
        {
            Debug.LogError("Card prefab or grid parent not assigned!");
            return;
        }
        
        foreach (PlayerCard card in filteredCards)
        {
            GameObject cardUI = Instantiate(cardPrefab, cardGridParent);
            CardDisplay cardDisplay = cardUI.GetComponent<CardDisplay>();
            
            if (cardDisplay != null)
            {
                cardDisplay.SetCardData(card);
            }
            
            // Add click listener
            Button cardButton = cardUI.GetComponent<Button>();
            if (cardButton != null)
            {
                cardButton.onClick.AddListener(() => OnCardClicked(card));
            }
            
            cardUIObjects.Add(cardUI);
        }
    }
    
    void OnCardClicked(PlayerCard card)
    {
        Debug.Log($"Clicked on {card.playerName} from {card.teamName}");
        // Here you could show card details, add to deck, etc.
    }
    
    void UpdateCollectionStats()
    {
        if (collectionCountText != null)
            collectionCountText.text = $"Collection: {allPlayerCards.Count} cards";
        
        if (totalCardsText != null)
            totalCardsText.text = $"Showing: {filteredCards.Count} cards";
    }
    
    public void OpenPack()
    {
        if (CardManager.Instance != null)
        {
            PlayerCard newCard = CardManager.Instance.OpenPack();
            if (newCard != null)
            {
                allPlayerCards.Add(newCard);
                RefreshCollection();
                Debug.Log($"Opened pack! Got {newCard.playerName} from {newCard.teamName}");
            }
        }
    }
    
    public void SortByOverall()
    {
        allPlayerCards.Sort((a, b) => b.overallRating.CompareTo(a.overallRating));
        RefreshCollection();
    }
    
    public void SortByRarity()
    {
        allPlayerCards.Sort((a, b) => b.rarity.CompareTo(a.rarity));
        RefreshCollection();
    }
    
    public void SortByTeam()
    {
        allPlayerCards.Sort((a, b) => a.teamName.CompareTo(b.teamName));
        RefreshCollection();
    }
}

