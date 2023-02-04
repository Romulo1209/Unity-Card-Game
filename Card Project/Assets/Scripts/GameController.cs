using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;



public class GameController : MonoBehaviour
{
    [Header("Start Game Parameters")]
    [SerializeField] [Range(2, 9)] int startHandCount = 3;
    [SerializeField] [Range(5, 9)] int maxMana = 6;
    [SerializeField] [Range(1, 6)] int maxBattlefieldPlayersCards = 6;

    [Header("Battlefield Parameters")]
    [SerializeField] ActualMode actualMode;
    [SerializeField] int turn = 1;
    [SerializeField] HandManager handTurn;

    [Header("Battlefield")]
    [SerializeField] List<GameObject> entireBattlefield;
    [SerializeField] List<GameObject> playerBattlefield;
    [SerializeField] List<GameObject> enemyBattlefield;
    [Space]
    [SerializeField] CardsBattlefieldController playerBattlefieldController;
    [SerializeField] CardsBattlefieldController enemyBattlefieldController;

    [Header("References")]
    [SerializeField] MenuController menuController;
    [SerializeField] GameObject CardPrefab;
    [SerializeField] Transform PlayerCardFather;
    [SerializeField] CardUIBase cardInspector;
    [Space]
    [SerializeField] Animator PlayerHandUI;
    [SerializeField] GameObject BotHandUI;
    [SerializeField] Button endTurnButton;

    [Header("Data Holder")]
    public CardHolderScriptable cardsHolder;

    public static GameController instance;

    //privates
    private HandManager[] players;

    #region Getters & Setters

    public int StartHandCount { get { return startHandCount; } set { startHandCount = value; } }
    public int MaxMana { get { return maxMana; } set { maxMana = value; } }
    public int MaxBattlefieldPlayersCards { get { return maxBattlefieldPlayersCards; } set { maxBattlefieldPlayersCards = value; } }

    public ActualMode ActualMode { get { return actualMode; } set { actualMode = value; } }
    public int Turn { get { return turn; } set { turn += value; } }
    public HandManager HandTurn { get { return handTurn; } set { handTurn = value; } }
    public List<GameObject> EntireBattlefield { get { return entireBattlefield; } set { entireBattlefield = value; } }
    public List<GameObject> PlayerBattlefield { get { return playerBattlefield; } set { playerBattlefield = value; } }
    public List<GameObject> EnemyBattlefield { get { return enemyBattlefield; } set { enemyBattlefield = value; } }
    public CardsBattlefieldController PlayerBattlefieldController { get { return playerBattlefieldController; } }
    public CardsBattlefieldController EnemyBattlefieldController { get { return enemyBattlefieldController; } }
    public bool EndTurnButton { get { return endTurnButton.interactable; } set { endTurnButton.interactable = value; } }

    #endregion

    private void Awake() {
        instance = this;
        players = GameObject.FindObjectsOfType<HandManager>();
        
    }

    public void SetupGame()
    {
        foreach (HandManager player in players) {
            player.MaxMana = MaxMana;
            player.Setup(StartHandCount);
        }
    }

    public void CheckPlayersLife()
    {
        foreach(HandManager player in players)
        {
            if(player.HealthPoints <= 0)
            {
                menuController.EndGame();
                Debug.Log("Game End");
            }
        }
    }

    #region Card Inspect

    public void CardInspectBegin(CardScriptable card)
    {
        cardInspector.gameObject.SetActive(true);
        cardInspector.CardScriptable = card;
        cardInspector.SetupCard();
    }
    public void CardInspectEnd()
    {
        cardInspector.gameObject.SetActive(false);
    }
    public void SwitchMode()
    {
        ActualMode = ActualMode == ActualMode.Edit ? ActualMode.Attack : ActualMode.Edit;
        PlayerHandUI.SetTrigger("HandSwitch");
    }

    #endregion

    #region Battlefield Functions

    public List<GameObject> ReturnBattlefield(HandManager hand)
    {
        List<GameObject> battlefield = new List<GameObject>();
        var botHand = hand as BotController;
        if (botHand != null)
            battlefield = EnemyBattlefield;
        else
            battlefield = PlayerBattlefield;

        return battlefield;
    }

    public void AddBattlefieldCard(GameObject card, Team team, HandManager cardOwner)
    {
        GameObject cardObj = null;
        cardOwner.CardBattlefield.AddCardToBattlefield(card, cardOwner, out cardObj);
        switch (team) {
            case Team.Player:
                PlayerBattlefield.Add(cardObj);
                break;
            case Team.Bot:
                EnemyBattlefield.Add(cardObj);
                break;
        }
        entireBattlefield.Add(cardObj);
    }

    public void RemoveCardFromBattlefield(GameObject card)
    {
        int indexEntire = EntireBattlefield.IndexOf(card);
        int indexPlayer = PlayerBattlefield.IndexOf(card);
        int indexEnemy = EnemyBattlefield.IndexOf(card);

        if(indexEntire != -1)
            EntireBattlefield.RemoveAt(indexEntire);
        if (indexPlayer != -1)
            PlayerBattlefield.RemoveAt(indexPlayer);
        if (indexEnemy != -1)
            EnemyBattlefield.RemoveAt(indexEnemy);
    }

    #region Turn

    public void NextPlayerTurn(HandManager actualPlayer)
    {
        foreach(HandManager player in players) {
            if (player.gameObject == actualPlayer.gameObject)
                continue;
            else {
                HandTurn = player;
                break;
            }
        }
        var handTurnBot = HandTurn as BotController;
        if (handTurnBot != null)
            StartCoroutine(handTurnBot.PutCardInBattlefield());
        else {
            if (actualMode == ActualMode.Attack)
                SwitchMode();
        }
        ActualTurn();
    }
    public void ActualTurn()
    {
        foreach(HandManager player in players) {
            if (!player.PlayedThisTurn)
                return;
        }

        Turn = 1;
        foreach (HandManager player in players) {
            player.CardBattlefield.ResetAttacks();
            player.PlayedThisTurn = false;
            player.StartTurn();
        }
        EndTurnButton = true;
    }

    #endregion

    #endregion
}
