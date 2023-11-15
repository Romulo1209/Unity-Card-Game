using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : HandManager
{
    public IEnumerator PutCardInBattlefield() {
        yield return new WaitForSeconds(Random.Range(1f, 2f));
        int i = 0;
        bool found = false;
        foreach(GameObject card in HandCards) {
            if(ManaPoints >= card.GetComponent<CardUIHand>().CardScriptable.CardCost) {
                found = true;
                break;
            }
            i++;
        }
        if (found) {
            if (HandCards[i].GetComponent<CardUIHand>().PutCardOnBattlefield()) {
                StartCoroutine(PutCardInBattlefield());
                yield break;
            }
        }
        StartCoroutine(AttackMode());
    }

    public IEnumerator AttackMode()
    {
        Debug.Log("AttackMode");
        for(int i = 0; i < CardBattlefield.CardObjects.Count; i++) //(GameObject cardBattlefield in CardBattlefield.CardObjects) {
        { 
            if(GameController.instance.PlayerBattlefieldController.CardObjects.Count != 0) {
                AttackCard(CardBattlefield.CardObjects[i]);
            }
            else {
                GameController.instance.PlayerBattlefieldController.DirectFight(GameController.instance.PlayerBattlefieldController, CardBattlefield.CardObjects[i].GetComponent<CardBattlefield>());
            }
            yield return new WaitForSeconds(1);
        }
        EndTurn();
    }
    void AttackCard(GameObject cardBattlefield)
    {
        CardBattlefield attackerCard = cardBattlefield.GetComponent<CardBattlefield>();
        CardBattlefield defenderCard = GameController.instance.PlayerBattlefieldController.GetWeakestCard().GetComponent<CardBattlefield>();
        GameController.instance.EnemyBattlefieldController.CardFight(attackerCard, defenderCard);
    }
}
