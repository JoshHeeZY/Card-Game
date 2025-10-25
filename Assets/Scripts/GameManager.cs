using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject collectionPanel;
    public GameObject battlePanel;
    public GameObject packOpeningPanel;
    
    [Header("UI References")]
    public Button playButton;
    public Button collectionButton;
    public Button packOpeningButton;
    public Button backToMenuButton;
    
    [Header("Managers")]
    public CardManager cardManager;
    public CollectionManager collectionManager;
    public BattleSystem battleSystem;
    
    [Header("Pack Opening")]
    public Button openPackButton;
    public TextMeshProUGUI packCountText;
    public CardDisplay packCardDisplay;
    
    private int packCount = 5; // Starting packs
    
    void Start()
    {
        InitializeGame();
        SetupUI();
    }
    
    void InitializeGame()
    {
        // Ensure CardManager exists
        if (cardManager == null)
        {
            cardManager = FindObjectOfType<CardManager>();
        }
        
        // Show main menu by default
        ShowMainMenu();
    }
    
    void SetupUI()
    {
        // Main menu buttons
        if (playButton != null)
            playButton.onClick.AddListener(StartBattle);
        
        if (collectionButton != null)
            collectionButton.onClick.AddListener(ShowCollection);
        
        if (packOpeningButton != null)
            packOpeningButton.onClick.AddListener(ShowPackOpening);
        
        // Back button
        if (backToMenuButton != null)
            backToMenuButton.onClick.AddListener(ShowMainMenu);
        
        // Pack opening
        if (openPackButton != null)
            openPackButton.onClick.AddListener(OpenPack);
        
        UpdatePackCount();
    }
    
    public void ShowMainMenu()
    {
        SetActivePanel(mainMenuPanel);
    }
    
    public void ShowCollection()
    {
        SetActivePanel(collectionPanel);
        
        // Refresh collection if needed
        if (collectionManager != null)
        {
            collectionManager.RefreshCollection();
        }
    }
    
    public void ShowPackOpening()
    {
        SetActivePanel(packOpeningPanel);
        UpdatePackCount();
    }
    
    public void StartBattle()
    {
        SetActivePanel(battlePanel);
        
        // Start new battle
        if (battleSystem != null)
        {
            battleSystem.StartNewBattle();
        }
    }
    
    void SetActivePanel(GameObject activePanel)
    {
        // Hide all panels
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
        
        if (collectionPanel != null)
            collectionPanel.SetActive(false);
        
        if (battlePanel != null)
            battlePanel.SetActive(false);
        
        if (packOpeningPanel != null)
            packOpeningPanel.SetActive(false);
        
        // Show active panel
        if (activePanel != null)
            activePanel.SetActive(true);
    }
    
    public void OpenPack()
    {
        if (packCount > 0)
        {
            packCount--;
            
            if (CardManager.Instance != null)
            {
                PlayerCard newCard = CardManager.Instance.OpenPack();
                
                if (newCard != null && packCardDisplay != null)
                {
                    packCardDisplay.SetCardData(newCard);
                    Debug.Log($"Opened pack! Got {newCard.playerName} ({newCard.rarity})");
                }
            }
            
            UpdatePackCount();
        }
        else
        {
            Debug.Log("No packs remaining!");
        }
    }
    
    void UpdatePackCount()
    {
        if (packCountText != null)
            packCountText.text = $"Packs: {packCount}";
        
        if (openPackButton != null)
            openPackButton.interactable = packCount > 0;
    }
    
    public void AddPacks(int amount)
    {
        packCount += amount;
        UpdatePackCount();
    }
    
    // For testing - add packs
    public void GiveTestPacks()
    {
        AddPacks(10);
    }
}

