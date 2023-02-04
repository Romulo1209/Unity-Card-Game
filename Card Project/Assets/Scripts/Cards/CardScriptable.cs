using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu]
public class CardScriptable : ScriptableObject
{
    [Header("Card Parameters")]
    public string CardName;
    public CardType CardType;
    [TextArea] public string CardDescription;
    [Space]
    public int CardDamage;
    public int CardDefense;
    public int CardCost;

    [Header("Card References")]
    public Sprite CardBack;
    public Sprite CardBase;
    public GameObject CardPrefab;

    [Header("Card Images")]
    public Sprite CardIcon;
    public Material CardMaterial;
}
