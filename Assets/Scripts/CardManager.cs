using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [Header("Card Data")]
    public List<PlayerCard> allCards = new List<PlayerCard>();
    public List<PlayerCard> playerCollection = new List<PlayerCard>();
    
    [Header("Team Data")]
    public List<string> teamNames = new List<string>();
    public List<Sprite> teamLogos = new List<Sprite>();
    
    [Header("Card Generation")]
    public int totalCardsToGenerate = 50;
    
    public static CardManager Instance;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeTeams();
            GenerateCards();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void InitializeTeams()
    {
        teamNames.Add("Mike FC");
        teamNames.Add("Queeney United");
        teamNames.Add("Banter FC");
        teamNames.Add("Elmdale Forest");
        teamNames.Add("Finchlea SC");
        teamNames.Add("Temple Rovers");
        teamNames.Add("Holt Athletic");
        teamNames.Add("Derby Hounds");
        teamNames.Add("Newton City");
        teamNames.Add("Claptown FC");
    }
    
    void GenerateCards()
    {
        // Generate cards for each team
        foreach (string team in teamNames)
        {
            GenerateTeamCards(team);
        }
        
        // Give player some starter cards
        GiveStarterCards();
    }
    
    void GenerateTeamCards(string teamName)
    {
        string[] positions = { "GK", "CB", "CB", "LB", "RB", "CDM", "CM", "CAM", "LW", "RW", "ST" };
        string[] playerNames = GetPlayerNamesForTeam(teamName);
        
        for (int i = 0; i < positions.Length; i++)
        {
            string playerName = playerNames[i];
            string position = positions[i];
            int overallRating = Random.Range(65, 95);
            CardRarity rarity = DetermineRarity(overallRating);
            
            PlayerCard newCard = new PlayerCard(playerName, teamName, position, overallRating, rarity);
            allCards.Add(newCard);
        }
    }
    
    string[] GetPlayerNamesForTeam(string teamName)
    {
        switch (teamName)
        {
            case "Mike FC":
                return new string[] { "Mikenaldo", "Lord Bendtner", "Mikey Boi", "Messi Manvendra", "Magic Mike", "Sanvendra", "Toasty Elbow", "The Krillmaster", "Big Mo", "Lo-Fi Luca", "DJ Halfpass" };
            case "Queeney United":
                return new string[] { "Queeney", "Love Moodle", "Lara Frost", "McJoshX", "Fara Lrost", "Sweeney", "Vibe Checka", "Kween B", "Wraithwalka", "Capri Clutch", "Snappy Snaps" };
            case "Banter FC":
                return new string[] { "Clapperson", "Sir Slidealot", "Banterino", "Flickz", "Ping McPing", "Cruffy Taps", "Tactical Tony", "Meme Lordie", "Gibbo", "Spooksy", "Hoverboots" };
            case "Elmdale Forest":
                return new string[] { "John McGinn", "Piney Pete", "Lush Loxley", "Mossman", "Barkley Roots", "Oaksy", "Twigster", "Elmz", "Yawn Branch", "Breezy Ben", "Foxalot" };
            case "Finchlea SC":
                return new string[] { "Ashley Barnes", "Red Duke", "Stubbs", "Baron Kickalot", "Grimble", "Slicer Stan", "Vino Viño", "Brewmeister", "Flashy Finch", "Corky Slide", "El Clasico Steve" };
            case "Temple Rovers":
                return new string[] { "Troy Deeney", "Monk Mode", "Scrollios", "Holy Hattrick", "Canonball", "Whisper Wind", "Clean Sheet Carl", "Bellringer", "Shroud Monk", "Blessington", "Peace Feet" };
            case "Holt Athletic":
                return new string[] { "Callu Wilson", "Silent Bootz", "Warmer Walcott", "Cryptik Calvin", "Holdfast", "Glitchman", "Fade To Black", "Rollo", "Hype Train", "Husher", "Greg the Glove" };
            case "Derby Hounds":
                return new string[] { "Danny Iing", "The Tracker", "Bark Knight", "Muzzle Max", "Whippet Walt", "Growlie", "Slinker", "Roverino", "Clawford", "Chew Bacca", "Doggy Drax" };
            case "Newton City":
                return new string[] { "Marc Albrighton", "Blueton Barry", "Newton Jet", "Pivot Pete", "Spiral", "Orbitz", "Turbo Torque", "Nudge Core", "Gizmo Greg", "Flyer Finn", "Volt Vinnie" };
            case "Claptown FC":
                return new string[] { "Big Clappa", "Thudrick", "Squintz", "Cruncho", "Yapper", "Lefty Bootman", "Rudy Skudz", "Flip Johnson", "Sidekick Sid", "Oof Boi", "Keeper Kazaam" };
            default:
                return new string[] { "Player1", "Player2", "Player3", "Player4", "Player5", "Player6", "Player7", "Player8", "Player9", "Player10", "Player11" };
        }
    }
    
    CardRarity DetermineRarity(int overallRating)
    {
        if (overallRating >= 90) return CardRarity.Legendary;
        if (overallRating >= 80) return CardRarity.Epic;
        if (overallRating >= 70) return CardRarity.Rare;
        return CardRarity.Common;
    }
    
    void GiveStarterCards()
    {
        // Give player 5 random cards to start
        for (int i = 0; i < 5; i++)
        {
            if (allCards.Count > 0)
            {
                PlayerCard randomCard = allCards[Random.Range(0, allCards.Count)];
                playerCollection.Add(randomCard);
            }
        }
    }
    
    public PlayerCard OpenPack()
    {
        // Simple pack opening - returns one random card
        if (allCards.Count > 0)
        {
            PlayerCard newCard = allCards[Random.Range(0, allCards.Count)];
            playerCollection.Add(newCard);
            return newCard;
        }
        return null;
    }
    
    public List<PlayerCard> GetPlayerCollection()
    {
        return playerCollection;
    }
    
    public List<PlayerCard> GetAllCards()
    {
        return allCards;
    }
}
