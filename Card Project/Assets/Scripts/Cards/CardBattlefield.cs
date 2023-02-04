using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class CardBattlefield : MonoBehaviour , IPointerDownHandler, IPointerUpHandler , IDragHandler
{
    [Header("Parameters")]
    [SerializeField] int cardHealth;
    [SerializeField] int cardDamage;
    [SerializeField] bool attackedThisTurn = false;
    [SerializeField] bool isBot = false;

    [Header("References")]
    [SerializeField] CardsBattlefieldController battlefield;
    [SerializeField] CardScriptable cardScriptable;
    [SerializeField] HandManager cardOwner;
    [SerializeField] LayerMask interactLayer;

    [Header("Particles")]
    [SerializeField] GameObject hitParticle;
    [SerializeField] GameObject deathParticle;

    [Space]
    public CardBattlefieldElements BattlefieldElements;

    private Material cardMaterial;
    private string cardName;
    private int cardCost;
    private int cardAttack;
    private string cardDescription;

    #region Getters & Setters

    public int CardHealth { get { return cardHealth; } set { cardHealth = value; BattlefieldElements.CardDefenseText.text = CardHealth.ToString(); } }
    public int CardDamage { get { return cardDamage; } set { cardDamage = value; BattlefieldElements.CardAttackText.text = CardDamage.ToString(); } }
    public bool AttackedThisTurn { get { return attackedThisTurn; } set { attackedThisTurn = value; } }
    public CardsBattlefieldController Battlefield { get { return battlefield; } set { battlefield = value; } }
    public HandManager CardOwner { get { return cardOwner; } set { cardOwner = value; } }
    public CardScriptable CardScriptable { get { return cardScriptable; } set { cardScriptable = value; } }
    public bool IsBot { get { return isBot; } set { isBot = value; } }
    public Material CardMaterial
    {
        get { return cardMaterial; }
        set {
            cardMaterial = value;
            BattlefieldElements.CardMesh.material = cardMaterial;
        }
    }
    public string CardName {
        get { return cardName; }
        set {
            cardName = value;
            BattlefieldElements.CardNameText.text = cardName;
        }
    }
    public int CardCost
    {
        get { return cardCost; }
        set {
            cardCost = value;
            BattlefieldElements.CardCostText.text = cardCost.ToString();
        }
    }
    public string CardDescription {
        get { return cardDescription; }
        set {
            cardDescription = value;
            BattlefieldElements.CardDescriptionText.text = cardDescription.ToString();
        }
    }

    #endregion

    public void SetupCard()
    {
        CardMaterial = cardScriptable.CardMaterial;
        CardName = cardScriptable.CardName;
        CardCost = cardScriptable.CardCost;
        CardDescription = cardScriptable.CardDescription;

        CardHealth = cardScriptable.CardDefense;
        CardDamage = cardScriptable.CardDamage;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            TakeDamage(1);
    }

    public void TakeDamage(int damage)
    {
        CardHealth -= damage;
        if (CardHealth <= 0) {
            Battlefield.DestroyCard(gameObject);
            GameController.instance.RemoveCardFromBattlefield(gameObject);
            Instantiate(deathParticle, transform.position, Quaternion.identity);
        }
        else {
            Instantiate(hitParticle, transform.position, Quaternion.identity);
        }
    }

    public void SelectCard() {
        Battlefield.CardSelected = this;
    }
    public void UnselectCard() {
        Battlefield.CardSelected = null;
    }

    GameObject ReturnRaycast(out Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, interactLayer))
        {
            pos = new Vector3(hit.point.x, 2, hit.point.z);
            return hit.collider.gameObject;
        }
        pos = Vector3.zero;
        return null;
    }

    #region Events

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Battlefield.HandOwner.Bot || CardOwner != GameController.instance.HandTurn)
            return;

        SelectCard();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Battlefield.HandOwner.Bot || CardOwner != GameController.instance.HandTurn)
            return;

        Vector3 pos;
        var obj = ReturnRaycast(out pos);
        var attackingCard = obj.GetComponent<CardBattlefield>();
        var attackingBattlefield = obj.GetComponent<CardsBattlefieldController>();
        if (attackingCard != null) {
            Battlefield.CardAttacked = attackingCard;
            Battlefield.CardFight(Battlefield.CardSelected, Battlefield.CardAttacked);
        }
        else if (attackingBattlefield != null) {
            if (attackingBattlefield.EmptyBattlefield) {
                battlefield.DirectFight(attackingBattlefield, this);
            }
            else {
                Debug.Log("Defeat in battle monsters");
            }
        }
        Battlefield.HideLine();
        UnselectCard();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Battlefield.HandOwner.Bot || CardOwner != GameController.instance.HandTurn)
            return;

        Vector3 mousePos = new Vector3();
        var card = ReturnRaycast(out mousePos).GetComponent<CardBattlefield>();
        if (card != null) {
            Battlefield.UpdateLine(gameObject, mousePos, card.gameObject);
        }
        else {
            Battlefield.UpdateLine(gameObject, mousePos);
        }
    }

    #endregion
}
