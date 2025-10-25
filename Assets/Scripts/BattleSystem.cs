using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleSystem : MonoBehaviour
{
    [Header("Battle Data")]
    public List<PlayerCard> playerDeck = new List<PlayerCard>();
    public List<PlayerCard> opponentDeck = new List<PlayerCard>();
    
    [Header("Battle State")]
    public int currentRound = 0;
    public int playerWins = 0;
    public int opponentWins = 0;
    public int draws = 0;
    
    [Header("UI References")]
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI opponentScoreText;
    public TextMeshProUGUI battleResultText;
    
    [Header("Card Display")]
    public CardDisplay playerCardDisplay;
    public CardDisplay opponentCardDisplay;
    
    [Header("Battle Animation")]
    public float battleDelay = 2f;
    public AnimationCurve battleCurve;
    
    private bool battleInProgress = false;
    private PlayerCard currentPlayerCard;
    private PlayerCard currentOpponentCard;
    
    void Start()
    {
        InitializeBattle();
    }
    
    void InitializeBattle()
    {
        // Get player's deck from CardManager
        if (CardManager.Instance != null)
        {
            playerDeck = new List<PlayerCard>(CardManager.Instance.GetPlayerCollection());
            
            // Ensure we have 11 cards (fill with random if needed)
            while (playerDeck.Count < 11)
            {
                PlayerCard randomCard = CardManager.Instance.OpenPack();
                if (randomCard != null)
                    playerDeck.Add(randomCard);
            }
        }
        
        // Generate opponent deck
        GenerateOpponentDeck();
        
        // Start first round
        StartCoroutine(StartBattle());
    }
    
    void GenerateOpponentDeck()
    {
        opponentDeck.Clear();
        
        // Generate 11 random cards for opponent
        for (int i = 0; i < 11; i++)
        {
            if (CardManager.Instance != null && CardManager.Instance.GetAllCards().Count > 0)
            {
                PlayerCard randomCard = CardManager.Instance.GetAllCards()[Random.Range(0, CardManager.Instance.GetAllCards().Count)];
                opponentDeck.Add(randomCard);
            }
        }
    }
    
    IEnumerator StartBattle()
    {
        battleInProgress = true;
        currentRound = 0;
        playerWins = 0;
        opponentWins = 0;
        draws = 0;
        
        UpdateUI();
        
        // Play 11 rounds
        for (int round = 0; round < 11; round++)
        {
            currentRound = round + 1;
            yield return StartCoroutine(PlayRound(round));
            yield return new WaitForSeconds(battleDelay);
        }
        
        // Battle finished
        EndBattle();
    }
    
    IEnumerator PlayRound(int roundIndex)
    {
        if (roundIndex >= playerDeck.Count || roundIndex >= opponentDeck.Count)
        {
            Debug.LogError("Not enough cards for round " + roundIndex);
            yield break;
        }
        
        currentPlayerCard = playerDeck[roundIndex];
        currentOpponentCard = opponentDeck[roundIndex];
        
        // Update card displays
        if (playerCardDisplay != null)
            playerCardDisplay.SetCardData(currentPlayerCard);
        
        if (opponentCardDisplay != null)
            opponentCardDisplay.SetCardData(currentOpponentCard);
        
        UpdateUI();
        
        // Determine battle outcome
        BattleOutcome outcome = DetermineBattleOutcome(currentPlayerCard, currentOpponentCard);
        
        // Animate battle
        yield return StartCoroutine(AnimateBattle(outcome));
        
        // Update scores
        switch (outcome)
        {
            case BattleOutcome.PlayerWin:
                playerWins++;
                break;
            case BattleOutcome.OpponentWin:
                opponentWins++;
                break;
            case BattleOutcome.Draw:
                draws++;
                break;
        }
        
        UpdateUI();
    }
    
    BattleOutcome DetermineBattleOutcome(PlayerCard playerCard, PlayerCard opponentCard)
    {
        // Match Attax style: Player chooses Attack, Opponent defends
        // Compare Attack vs Defense
        int playerAttack = playerCard.attack;
        int opponentDefense = opponentCard.defense;
        
        if (playerAttack > opponentDefense)
        {
            return BattleOutcome.PlayerWin;
        }
        else if (playerAttack < opponentDefense)
        {
            return BattleOutcome.OpponentWin;
        }
        else
        {
            return BattleOutcome.Draw;
        }
    }
    
    IEnumerator AnimateBattle(BattleOutcome outcome)
    {
        // Simple battle animation
        float duration = 1f;
        float time = 0;
        
        Vector3 playerStartScale = playerCardDisplay.transform.localScale;
        Vector3 opponentStartScale = opponentCardDisplay.transform.localScale;
        
        while (time < duration)
        {
            time += Time.deltaTime;
            float progress = time / duration;
            float curveValue = battleCurve.Evaluate(progress);
            
            // Animate cards based on outcome
            if (outcome == BattleOutcome.PlayerWin)
            {
                playerCardDisplay.transform.localScale = playerStartScale * (1 + curveValue * 0.2f);
                opponentCardDisplay.transform.localScale = opponentStartScale * (1 - curveValue * 0.1f);
            }
            else if (outcome == BattleOutcome.OpponentWin)
            {
                playerCardDisplay.transform.localScale = playerStartScale * (1 - curveValue * 0.1f);
                opponentCardDisplay.transform.localScale = opponentStartScale * (1 + curveValue * 0.2f);
            }
            
            yield return null;
        }
        
        // Reset scales
        playerCardDisplay.transform.localScale = playerStartScale;
        opponentCardDisplay.transform.localScale = opponentStartScale;
    }
    
    void UpdateUI()
    {
        if (roundText != null)
            roundText.text = $"Round {currentRound}/11";
        
        if (playerScoreText != null)
            playerScoreText.text = $"Player: {playerWins}";
        
        if (opponentScoreText != null)
            opponentScoreText.text = $"Opponent: {opponentWins}";
        
        if (battleResultText != null && currentPlayerCard != null && currentOpponentCard != null)
        {
            BattleOutcome outcome = DetermineBattleOutcome(currentPlayerCard, currentOpponentCard);
            string resultText = "";
            
            switch (outcome)
            {
                case BattleOutcome.PlayerWin:
                    resultText = $"{currentPlayerCard.playerName} WINS! ({currentPlayerCard.attack} vs {currentOpponentCard.defense})";
                    break;
                case BattleOutcome.OpponentWin:
                    resultText = $"{currentOpponentCard.playerName} WINS! ({currentPlayerCard.attack} vs {currentOpponentCard.defense})";
                    break;
                case BattleOutcome.Draw:
                    resultText = $"DRAW! ({currentPlayerCard.attack} vs {currentOpponentCard.defense})";
                    break;
            }
            
            battleResultText.text = resultText;
        }
    }
    
    void EndBattle()
    {
        battleInProgress = false;
        
        string finalResult = "";
        if (playerWins > opponentWins)
        {
            finalResult = "VICTORY! You won the match!";
        }
        else if (opponentWins > playerWins)
        {
            finalResult = "DEFEAT! Opponent won the match!";
        }
        else
        {
            finalResult = "DRAW! The match ended in a tie!";
        }
        
        Debug.Log($"Battle Complete! {finalResult} Score: {playerWins}-{opponentWins}-{draws}");
        
        if (battleResultText != null)
            battleResultText.text = finalResult;
    }
    
    public void StartNewBattle()
    {
        if (!battleInProgress)
        {
            InitializeBattle();
        }
    }
    
    public void SkipToNextRound()
    {
        // For testing purposes
        if (battleInProgress)
        {
            StopAllCoroutines();
            StartCoroutine(StartBattle());
        }
    }
}

public enum BattleOutcome
{
    PlayerWin,
    OpponentWin,
    Draw
}
