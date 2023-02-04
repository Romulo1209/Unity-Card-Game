using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] DeckType playerDeckType;
    [SerializeField] DeckType enemyDeckType;

    [SerializeField] HandManager playerManager;
    [SerializeField] HandManager enemyManager;

    [SerializeField] TMP_Text playerDeckTypeText;
    [SerializeField] TMP_Text enemyDeckTypeText;

    [SerializeField] GameObject MenuWindow;
    [SerializeField] GameObject GameWindow;
    [SerializeField] GameObject EndWindow;

    #region Getters & Setters

    public DeckType PlayerDeckType { 
        get { return playerDeckType; } 
        set { 
            playerDeckType = value; 
            playerDeckTypeText.text = PlayerDeckType.ToString();
            playerManager.DeckType = PlayerDeckType;
        } 
    }
    public DeckType EnemyDeckType { 
        get { return enemyDeckType; } 
        set {
            enemyDeckType = value; 
            enemyDeckTypeText.text = EnemyDeckType.ToString();
            enemyManager.DeckType = EnemyDeckType;
        }
    }

    #endregion

    private void Start()
    {
        PlayerDeckType = PlayerDeckType;
        EnemyDeckType = EnemyDeckType;
    }

    public void SwitchPlayerDeck() {
        int actualDeckIndex = (int)PlayerDeckType;
        int lastDeckIndex = (int)Enum.GetValues(typeof(DeckType)).Cast<DeckType>().Last();

        if (actualDeckIndex != lastDeckIndex)
            PlayerDeckType = (DeckType)actualDeckIndex + 1;
        else
            PlayerDeckType = 0;
    }
    public void SwitchEnemyDeck() {
        int actualDeckIndex = (int)EnemyDeckType;
        int lastDeckIndex = (int)Enum.GetValues(typeof(DeckType)).Cast<DeckType>().Last();

        if (actualDeckIndex != lastDeckIndex)
            EnemyDeckType = (DeckType)actualDeckIndex + 1;
        else
            EnemyDeckType = 0;
    }
    public void StartGame()
    {
        GameController.instance.SetupGame();
        MenuWindow.SetActive(false);
        GameWindow.SetActive(true);
    }
    public void EndGame()
    {
        MenuWindow.SetActive(false);
        GameWindow.SetActive(false);
        EndWindow.SetActive(true);
        StartCoroutine(Restart());
    }
    IEnumerator Restart()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(0);
    }
}
