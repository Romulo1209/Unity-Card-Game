using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum CardType { Monster }
public enum Team { Player, Bot }
public enum DeckType { Human, Goblin }
public enum ActualMode { Attack, Edit}

[System.Serializable]
public struct CardUIElements
{
    public Image CardImage;
    public Image CardBase;
    public TMP_Text CardNameText;
    public TMP_Text CardCostText;
    public TMP_Text CardAttackText;
    public TMP_Text CardDefenseText;
    public TMP_Text CardDescriptionText;
}

[System.Serializable]
public struct CardBattlefieldElements
{
    public MeshRenderer CardMesh;
    public TMP_Text CardNameText;
    public TMP_Text CardCostText;
    public TMP_Text CardAttackText;
    public TMP_Text CardDefenseText;
    public TMP_Text CardDescriptionText;
}

public class CardUtils
{

}
