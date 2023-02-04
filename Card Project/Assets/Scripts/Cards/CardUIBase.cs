using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]

public class CardUIBase : MonoBehaviour
{
    [SerializeField] CardScriptable cardScriptable;
    [SerializeField] HandManager cardOwner;
    [SerializeField] bool isBot = false;

    private Sprite cardBase;
    private Sprite cardSprite;
    private string cardName;
    private int cardCost;
    private int cardAttack;
    private int cardDefense;
    private string cardDescription;

    public CardUIElements UIElements;

    #region Getters & Setters

    public bool IsBot { get { return isBot; } set { isBot = value; } }
    public CardScriptable CardScriptable { get { return cardScriptable; } set { cardScriptable = value; } }
    public HandManager CardOwner { get { return cardOwner; } set { cardOwner = value; } }
    public Sprite CardBase {
        get { return cardBase; }
        set {
            cardBase = value;
            UIElements.CardBase.sprite = cardBase;
        }
    }
    public Sprite CardSprite {
        get { return cardSprite; }
        set {
            if(value != null) {
                cardSprite = value;
                UIElements.CardImage.sprite = cardSprite;
            }
            else
                UIElements.CardImage.enabled = false;
        }
    }
    public string CardName {
        get { return cardName; }
        set {
            if(value != null) {
                cardName = value;
                UIElements.CardNameText.text = cardName;
            }
            else
                UIElements.CardNameText.enabled = false;
        }
    }
    public int CardCost {
        get { return cardCost; }
        set {
            if (value != 0) {
                cardCost = value;
                UIElements.CardCostText.text = cardCost.ToString();
            }
            else
                UIElements.CardCostText.enabled = false;
        }
    }
    public int CardAttack {
        get { return cardAttack; }
        set {
            if (value != 0) {
                cardAttack = value;
                UIElements.CardAttackText.text = cardAttack.ToString();
            }
            else
                UIElements.CardAttackText.enabled = false;
        }
    }
    public int CardDefense {
        get { return cardDefense; }
        set {
            if (value != 0) {
                cardDefense = value;
                UIElements.CardDefenseText.text = cardDefense.ToString();
            }
            else
                UIElements.CardDefenseText.enabled = false;
        }
    }
    public string CardDescription {
        get { return cardDescription; }
        set {
            if (value != null) {
                cardDescription = value;
                UIElements.CardDescriptionText.text = cardDescription.ToString();
            }
            else
                UIElements.CardDescriptionText.enabled = false;
        }
    }

    #endregion

    private void OnValidate()
    {
        if(UIElements.CardImage == null)
        {
            UIElements.CardImage = transform.GetChild(1).GetComponent<Image>();
            UIElements.CardNameText = transform.GetChild(4).GetComponent<TMP_Text>();
            UIElements.CardCostText = transform.GetChild(3).GetComponent<TMP_Text>();
            UIElements.CardAttackText = transform.GetChild(5).GetComponent<TMP_Text>();
            UIElements.CardDefenseText = transform.GetChild(6).GetComponent<TMP_Text>();
            UIElements.CardDescriptionText = transform.GetChild(7).GetComponent<TMP_Text>();
        }
    }

    public void SetupCard()
    {
        if (!IsBot) {
            CardBase = cardScriptable.CardBase;
            CardSprite = cardScriptable.CardIcon;
            CardName = cardScriptable.CardName;
            CardCost = cardScriptable.CardCost;
            CardAttack = cardScriptable.CardDamage;
            CardDefense = cardScriptable.CardDefense;
            CardDescription = cardScriptable.CardDescription;
        }
        else
        {
            CardBase = cardScriptable.CardBack;
            CardSprite = null;
            CardName = null;
            CardCost = 0;
            CardAttack = 0;
            CardDefense = 0;
            CardDescription = null;
        }   
    }

    public bool PutCardOnBattlefield() {
        if (GameController.instance.ReturnBattlefield(CardOwner).Count < GameController.instance.MaxBattlefieldPlayersCards) {
            GameController.instance.AddBattlefieldCard(gameObject, TeamReturn(), CardOwner);
            GameController.instance.CardInspectEnd();
            CardOwner.ManaPoints -= CardScriptable.CardCost;
            CardOwner.RemoveCard(gameObject);
            return true;
        }
        return false;
    }

    public Team TeamReturn()
    {
        if (IsBot)
            return Team.Bot;
        else
            return Team.Player;
    }
}
