using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    public GameObject cardPrefab;
    public RectTransform parentCanvas;
    public int numberOfCards = 3;
    public float spacing = 350f;

    void Start()
    {
        SpawnCards(); // ✅ Make sure this is here!
    }

    void SpawnCards()
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            GameObject card = Instantiate(cardPrefab, parentCanvas);
            RectTransform rect = card.GetComponent<RectTransform>();

            float xPos = (i - (numberOfCards - 1) / 2f) * spacing;
            rect.anchoredPosition = new Vector2(xPos, 0);

            // Create dummy PlayerCard
            PlayerCard tempCard = new PlayerCard(
                name: $"Player {i + 1}",
                team: "Test FC",
                pos: "ST",
                overall: Random.Range(70, 90),
                cardRarity: CardRarity.Rare
            );

            // Assign to CardDisplay
            CardDisplay display = card.GetComponent<CardDisplay>();
            if (display != null)
            {
                display.SetCardData(tempCard);
            }
        }
    }
}
