using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CardHolderScriptable : ScriptableObject
{
    public List<CardScriptable> AllCards;
    public List<CardScriptable> HumanCards;
    public List<CardScriptable> GoblinCards;
    [Space]
    public GameObject CardPrefab;

    public List<CardScriptable> DeckReturn(DeckType deckType)
    {
        switch (deckType)
        {
            case DeckType.Human:
                return HumanCards;
            case DeckType.Goblin:
                return GoblinCards;
            default:
                return null;
        }
    }
}
