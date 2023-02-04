using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class HandManager : MonoBehaviour
{
    [Header("Parameters")]
    public DeckType DeckType;
    [SerializeField] bool bot;

    [SerializeField] int manaPoints = 1;
    [SerializeField] int healthPoints = 100;
    [SerializeField] [Range(2, 9)] int maxHandCards = 6;
    [SerializeField] bool playedThisTurn = false;

    public List<GameObject> HandCards;

    [Header("References")]
    public Transform CardFather;
    public CardHolderScriptable CardsHolder;
    public CardsBattlefieldController CardBattlefield;
    [SerializeField] ParticleSystem hitParticle;
    [SerializeField] TMP_Text mpText;
    [SerializeField] TMP_Text hpText;

    private List<CardScriptable> deck;
    private int manaRound = 2;
    private int maxMana = 6;

    #region Getters & Setters

    public bool Bot { get { return bot; } }
    public int MaxHandCards { get { return maxHandCards; } set { maxHandCards = value; } }
    public int HealthPoints { get { return healthPoints; } set { healthPoints = value; hpText.text = "HP: " + healthPoints.ToString(); } }
    public int ManaPoints { get { return manaPoints; } set { manaPoints = value; mpText.text = "MP: " + manaPoints.ToString(); } }
    public int ManaRound { get { return manaRound; } set { manaRound += 1; if (manaRound > maxMana) manaRound = maxMana; ManaPoints = ManaRound; } }
    public int MaxMana { set { maxMana = value; } }
    public bool PlayedThisTurn { get { return playedThisTurn; } set { playedThisTurn = value; } }
    public List<CardScriptable> Deck { get { return deck; } set { deck = value; } }

    #endregion

    public virtual void Setup(int handStarter) {
        Deck = CardsHolder.DeckReturn(DeckType);
        HealthPoints = healthPoints;
        ManaPoints = manaPoints;
        DealCard(handStarter);
    }

    public void StartTurn()
    {
        ManaRound = ManaRound;
        if(HandCards.Count < MaxHandCards)
            DealCard(1);
    }

    #region Hand Functions

    public virtual void DealCard(int cardToDeal)
    {
        for (int i = 0; i < cardToDeal; i++) {
            CardScriptable card = Deck[Random.Range(0, Deck.Count)];
            GameObject cardObj = Instantiate(CardsHolder.CardPrefab, CardFather);
            var cardScript = cardObj.GetComponent<CardUIHand>();
            var hand = this as BotController;
            cardScript.IsBot = hand != null ? true : false;
            cardScript.CardScriptable = card;
            cardScript.CardOwner = this as HandManager;
            cardScript.SetupCard();
            HandCards.Add(cardObj);
        }
    }

    public void RemoveCard(GameObject card)
    {
        HandCards.Remove(card);
        Destroy(card.gameObject); 
    }

    public void TakeDamage(int damageCount) {
        HealthPoints -= damageCount;
        hitParticle.Play();
    }
    public void MaximizeMana(int manaMaximize) {
        ManaPoints += manaMaximize;
    }

    public void EndTurn() {
        if (GameController.instance.HandTurn) {
            PlayedThisTurn = true;
            GameController.instance.EndTurnButton = false;
            GameController.instance.NextPlayerTurn(this);
            var bot = this as BotController;
            bool isBot = bot != null ? true : false;
            if (GameController.instance.ActualMode == ActualMode.Edit && !isBot)
                GameController.instance.SwitchMode();
        }
    }

    #endregion
}
