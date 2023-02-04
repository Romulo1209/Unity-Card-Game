using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CardUIHand : CardUIBase , IPointerEnterHandler , IPointerExitHandler , IPointerDownHandler , IPointerUpHandler 
{
    [SerializeField] RectTransform rectCard;
    [SerializeField] Animator animator;

    #region Events

    public void OnPointerEnter(PointerEventData eventData)
    {   
        if (!IsBot) {
            //Mouse em cima da carta
            GameController.instance.CardInspectBegin(CardScriptable);
            animator.SetBool("Hover", true);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsBot) {
            //Mouse sai de cima da carta
            GameController.instance.CardInspectEnd();
            animator.SetBool("Hover", false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameController.instance.HandTurn == CardOwner) {
            //Botão do mouse pressionado
            if (CardOwner.ManaPoints >= CardScriptable.CardCost) {
                PutCardOnBattlefield();
            }
        }
        else
            Debug.Log("Not Your Turn!");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Mouse Button Up
    }
 
    #endregion
}