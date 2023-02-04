using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsBattlefieldController : MonoBehaviour
{
    [SerializeField] List<Vector3> cardsPositions;
    [SerializeField] List<GameObject> cardsObjects;

    [Header("Battlefield References")]
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] HandManager handOwner;
    [SerializeField] CardBattlefield cardSelected;
    [SerializeField] CardBattlefield cardToAttack;

    public GameObject CardBattlefieldPrefab;

    #region Getters & Setters

    public List<GameObject> CardObjects { get { return cardsObjects; } }
    public bool EmptyBattlefield { get { if (cardsObjects.Count == 0) return true; else return false; } }
    public CardBattlefield CardSelected { get { return cardSelected; } set { cardSelected = value; } }
    public CardBattlefield CardAttacked { get { return cardToAttack; } set { cardToAttack = value; } }
    public HandManager HandOwner { get { return handOwner; } set { handOwner = value; } }

    #endregion

    public void AddCardToBattlefield(GameObject card, HandManager cardOwner, out GameObject cardObjOut)
    {
        GameObject cardObj = Instantiate(CardBattlefieldPrefab, transform.position, Quaternion.Euler(0, 180, 0));
        cardObj.transform.SetParent(transform);
        cardObj.name = card.GetComponent<CardUIHand>().CardName;
        var cardBattlefield = cardObj.GetComponent<CardBattlefield>();
        cardBattlefield.CardScriptable = card.GetComponent<CardUIHand>().CardScriptable;
        cardBattlefield.Battlefield = this;
        cardBattlefield.CardOwner = cardOwner;
        cardBattlefield.SetupCard();
        cardsObjects.Add(cardObj);
        int index = cardsObjects.IndexOf(cardObj);
        cardsObjects[index].transform.position = cardsPositions[index];
        cardObjOut = cardObj;
    }

    public void DestroyCard(GameObject card)
    {
        int index = cardsObjects.IndexOf(card);
        cardsObjects.RemoveAt(index);
        Destroy(card);
        StartCoroutine(RefreshBattlefield());
    }
    public IEnumerator RefreshBattlefield()
    {
        yield return new WaitForSeconds(.5f);
        int i = 0;
        foreach(GameObject card in cardsObjects) {
            card.transform.position = cardsPositions[i];
            i++;
        }
    }
    public void HideLine()
    {
        lineRenderer.enabled = false;
    }
    public void UpdateLine(GameObject cardAttacker, Vector3 freePosition, GameObject cardDefender = null)
    {
        lineRenderer.enabled = true;
        var cardAttackerPos = cardAttacker.transform.position;
        var cardDefenderPos = cardAttacker.transform.position;
        cardAttackerPos.y = 2;
        cardDefenderPos.y = 2;

        if (cardDefender == null || cardDefender.GetComponent<CardBattlefield>().Battlefield == cardAttacker.GetComponent<CardBattlefield>().Battlefield) {
            lineRenderer.SetPosition(0, cardAttacker.transform.position);
            lineRenderer.SetPosition(1, freePosition);
        }
        else {
            lineRenderer.SetPosition(0, cardAttacker.transform.position);
            lineRenderer.SetPosition(1, cardDefender.transform.position);
        }
    }

    public void CardFight(CardBattlefield attacker, CardBattlefield defender) {
        if (!attacker.AttackedThisTurn && attacker.Battlefield != defender.Battlefield) {
            attacker.TakeDamage(defender.CardDamage);
            defender.TakeDamage(attacker.CardDamage);
            attacker.AttackedThisTurn = true;
            return;
        }
        Debug.Log(attacker.CardName + " already attacked!");
    }
    public bool DirectFight(CardsBattlefieldController defenderHand, CardBattlefield attackerCard)
    {
        if (!attackerCard.AttackedThisTurn) {
            attackerCard.AttackedThisTurn = true;
            defenderHand.HandOwner.TakeDamage(attackerCard.CardDamage);
            GameController.instance.CheckPlayersLife();
            return true;
        }
        Debug.Log("Already Attacked");
        return false;
    }
    public void ResetAttacks() {
        foreach(GameObject card in cardsObjects) {
            card.GetComponent<CardBattlefield>().AttackedThisTurn = false;
        }
    }

    public GameObject GetWeakestCard()
    {
        if(CardObjects.Count != 0) {
            GameObject weakest = CardObjects[0];
            for(int i = 0; i < CardObjects.Count; i++) {
                if (CardObjects[i].GetComponent<CardBattlefield>().CardHealth < weakest.GetComponent<CardBattlefield>().CardHealth)
                    weakest = CardObjects[i];
            }
            return weakest;
        }
        return null;
    }
}
