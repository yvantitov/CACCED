using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    private GameManager gm = GameManager.Instance;

    // Flanks
    public GameObject playerLeftFlank;
    public Text playerLeftFlankText;

    public GameObject playerCenterFlank;
    public Text playerCenterFlankText;

    public GameObject playerRightFlank;
    public Text playerRightFlankText;

    public GameObject enemyLeftFlank;
    public Text enemyLeftFlankText;

    public GameObject enemyCenterFlank;
    public Text enemyCenterFlankText;

    public GameObject enemyRightFlank;
    public Text enemyRightFlankText;

    public GameObject selectionHighlighter;
    public GameObject targetingPointer;

    // Buttons
    public GameObject attackButton;
    public GameObject scoutButton;
    public GameObject tacticButton;
    public GameObject retreatButton;

    // Rout marker
    public GameObject routedFlag;

    // Top-left turn counter
    public Text turnCounterText;

    // Combat info panel
    public GameObject combatInfoPanel;
    public Text combatInfoPanelTitleText;
    public Text combatInfoPanelFlavorText;
    public Text combatInfoPanelPowerText;
    public Text combatInfoPanelButtonText;

    // Retreat info panel
    public GameObject retreatInfoPanel;
    public Text retreatInfoPanelFlavorText;
    public Text retreatInfoPanelSubtitleText;
    public Text retreatInfoPanelButtonText;

    // Battle outcome panel
    public GameObject battleOutcomePanel;
    public Text battleOutcomePanelTitleText;
    public Text battleOutcomePanelFlavorText;
    public GameObject battleOutcomePanelVictoryImage;
    public GameObject battleOutcomePanelDefeatImage;

    // Music and ambience
    public GameObject ambience;
    public GameObject defeatMusic;
    public GameObject victoryMusic;

    // Text at bottom of screen indicating current state
    public Text currentStateText;

    public enum BattleState
    {
        Default,
        SelectAttackSourceFlank,
        SelectAttackTargetFlank,
        SelectScoutTargetFlank,
        Over,
        Retreat
    }
    public BattleState state = BattleState.Default;

    public enum EnemyFlank
    {
        Left,
        Center,
        Right
    }

    public enum PlayerFlank
    {
        None,
        Left,
        Center,
        Right
    }
    private PlayerFlank currentSelectedPlayerFlank = PlayerFlank.None;

    public enum RetreatType
    {
        TotalSuccess,
        PartialSuccess,
        Failure
    }

    private Battle battle;

    void Start()
    {
        battle = gm.currentBattle;
        ambience.SetActive(true);
        victoryMusic.SetActive(false);
        defeatMusic.SetActive(false);
    }

    void Update()
    {
        // We don't update while any popups are open
        if (combatInfoPanel.activeInHierarchy || retreatInfoPanel.activeInHierarchy)
        {
            return;
        }
        
        // Check to see if we have retreated
        if (state == BattleState.Retreat)
        {
            EndBattle();
            state = BattleState.Over;
        }

        // Update current state
        string newStateText = "";
        switch (state)
        {
            case BattleState.Default:
                newStateText = "Your men await your orders, general.";
                break;
            case BattleState.SelectAttackSourceFlank:
                newStateText = "Select a flank to attack with, general.";
                break;
            case BattleState.SelectAttackTargetFlank:
                newStateText = "Where shall we attack, general?";
                break;
            case BattleState.SelectScoutTargetFlank:
                newStateText = "Where shall we send scouts, general?";
                break;
        }
        currentStateText.text = newStateText;

        // Update turn counter
        turnCounterText.text = "Turn " + battle.turn;

        // Check for player loss
        int playerTotalLost = 0;
        playerTotalLost += battle.pLeftRouted ? 1 : 0;
        playerTotalLost += battle.pCenterRouted ? 1 : 0;
        playerTotalLost += battle.pRightRouted ? 1 : 0;
        if (playerTotalLost >= 2)
        {
            Debug.Log("The battle is lost, general. Today is a black day for Rome!");
            state = BattleState.Over;
            EndBattle(false);
        }

        // Check for enemy loss
        int enemyTotalLost = 0;
        enemyTotalLost += battle.eLeftRouted ? 1 : 0;
        enemyTotalLost += battle.eCenterRouted ? 1 : 0;
        enemyTotalLost += battle.eRightRouted ? 1 : 0;
        if (enemyTotalLost >= 2)
        {
            Debug.Log("Our legions stand triumphant! Glory to Rome!");
            state = BattleState.Over;
            EndBattle(true);
        }

        if (state == BattleState.Over)
        {
            // This just makes the program stop, TODO: Make Battles actually connect to the map screen
            return;
        }

        // Update player force values
        playerLeftFlankText.text = battle.pLeft.ToString();
        playerCenterFlankText.text = battle.pCenter.ToString();
        playerRightFlankText.text = battle.pRight.ToString();

        // Update enemy force values
        enemyLeftFlankText.text = battle.eLeftIntel ? battle.eLeft.ToString() : "?";
        enemyCenterFlankText.text = battle.eCenterIntel ? battle.eCenter.ToString() : "?";
        enemyRightFlankText.text = battle.eRightIntel ? battle.eRight.ToString() : "?";

        // Update selection highlighter
        if (state == BattleState.Default)
        {
            currentSelectedPlayerFlank = PlayerFlank.None;
        }
        switch (currentSelectedPlayerFlank)
        {
            case PlayerFlank.None:
                selectionHighlighter.SetActive(false);
                break;
            case PlayerFlank.Left:
                selectionHighlighter.SetActive(true);
                selectionHighlighter.transform.SetPositionAndRotation(playerLeftFlank.transform.position, Quaternion.identity);
                break;
            case PlayerFlank.Center:
                selectionHighlighter.SetActive(true);
                selectionHighlighter.transform.SetPositionAndRotation(playerCenterFlank.transform.position, Quaternion.identity);
                break;
            case PlayerFlank.Right:
                selectionHighlighter.SetActive(true);
                selectionHighlighter.transform.SetPositionAndRotation(playerRightFlank.transform.position, Quaternion.identity);
                break;
        }
    }

    void EndBattle(bool playerWon)
    {
        ambience.SetActive(false);
        if (playerWon)
        {
            victoryMusic.SetActive(true);
            battleOutcomePanelTitleText.text = "VICTORY";
            battleOutcomePanelFlavorText.text = "The sons of Rome stand triumphant!\nA glorious day for Rome!";
            battleOutcomePanelDefeatImage.SetActive(false);
            battleOutcomePanelVictoryImage.SetActive(true);
        }
        else
        {
            defeatMusic.SetActive(true);
            battleOutcomePanelTitleText.text = "DEFEAT";
            battleOutcomePanelFlavorText.text = "Our forces have been routed, general!\nA black day for Rome!";
            battleOutcomePanelDefeatImage.SetActive(true);
            battleOutcomePanelVictoryImage.SetActive(false);
        }
        battleOutcomePanel.SetActive(true);
    }

    // For retreats
    void EndBattle()
    {
        ambience.SetActive(false);
        battleOutcomePanelTitleText.text = "Retreat";
        battleOutcomePanelFlavorText.text = "Your force has quit the field to fight another day.";
        battleOutcomePanelVictoryImage.SetActive(false);
        battleOutcomePanelDefeatImage.SetActive(false);
        battleOutcomePanel.SetActive(true);
    }

    void RunEnemyTurn()
    {
        battle.turn++;
        Debug.Log("Enemy turn!");
        state = BattleState.Default;
    }
    
    void Scout(EnemyFlank target)
    {
        switch (target)
        {
            case EnemyFlank.Left:
                Debug.Log("Player scouted enemy left flank!");
                battle.eLeftIntel = true;
                break;
            case EnemyFlank.Center:
                Debug.Log("Player scouted enemy center flank!");
                battle.eCenterIntel = true;
                break;
            case EnemyFlank.Right:
                Debug.Log("Player scouted enemy right flank!");
                battle.eRightIntel = true;
                break;
        }
        RunEnemyTurn();
    }

    void Attack(PlayerFlank playerFlank, EnemyFlank enemyFlank, bool playerIsAttacking)
    {
        float powPlayer = GetFlankPower(playerFlank) + (float) gm.player.fortBonus;
        float powEnemy = GetFlankPower(enemyFlank);
        bool playerVictory = true;
        
        if (playerIsAttacking)
        {
            Debug.Log("Player " + playerFlank.ToString() + " attacked Enemy " + enemyFlank.ToString()
                + "\n" + powPlayer.ToString("0.000") + " : " + powEnemy.ToString("0.000"));
        }
        else
        {
            Debug.Log("Enemy " + enemyFlank.ToString() + " attacked Player " + playerFlank.ToString()
                + "\n" + powEnemy.ToString("0.000") + " : " + powPlayer.ToString("0.000"));
        }

        if (powPlayer == powEnemy)
        {
            if (playerIsAttacking)
            {
                Rout(playerFlank);
                playerVictory = false;
            }
            else
            {
                Rout(enemyFlank);
                playerVictory = true;
            }
        }
        else if (powPlayer > powEnemy)
        {
            Rout(enemyFlank);
            playerVictory = true;
        }
        else
        {
            Rout(playerFlank);
            playerVictory = false;
        }
        MakeCombatInfoPanel(powPlayer, powEnemy, playerIsAttacking, playerVictory);
        RunEnemyTurn();
    }

    void MakeCombatInfoPanel(float powPlayer, float powEnemy, bool playerAttacking, bool playerVictory)
    {
        combatInfoPanelTitleText.text = playerVictory ? "VICTORIA" : "CLADES";
        combatInfoPanelPowerText.text = powPlayer.ToString("0.0") + " : " + powEnemy.ToString("0.0");
        float powerBalance = powPlayer / powEnemy;
        string flavorText = "";
        if (powerBalance > 3)
        {
            if (playerAttacking)
            {
                flavorText = "The onslaught of your soldiers is great and terrible. The vastly outnumbered enemy flees the field before your men even make contact.";
            }
            else
            {
                flavorText = "A few pila is all it takes to break the enemy's suicidal charge. They shatter and quit the field before they reach your lines.";
            }
        }
        else if (powerBalance > 2)
        {
            if (playerAttacking)
            {
                flavorText = "A hail of pila rains down upon the enemy, rendering shields unusable and skewering a few unlucky souls all the way through."
                    + "\nWithout giving them a moment's rest, your men furiously crash into the enemy, cutting them to bits with gladius in hand. The enemy melts away before your eyes.";
            }
            else
            {
                flavorText = "Your lines barely quiver as the enemy makes contact. They fight half-heartedly for a few moments before a rout breaks out.";
            }
        }
        else if (powerBalance > 1)
        {
            if (playerAttacking)
            {
                flavorText = "The enemy meets the brutal charge of your legionaries head-on. But after a few vicious minutes of fighting, it becomes clear that your legions have triumphed.";
            }
            else
            {
                flavorText = "A sizable enemy force crashes upon your shield wall, ranting incomprehensible threats of violence in their barbaric tongue."
                    + "\nYour men beat them back after some fierce fighting, but not without some difficulty.";
            }
        }
        else if (powerBalance == 1)
        {
            if (playerVictory)
            {
                flavorText = "Fierce and violent fighting lasts for what seems like an eternity. Finally, the battered and bruised enemy quits the field, and not a moment too late."
                    + "\nYour men are exhausted, beaten and bloody.";
            }
            else
            {
                flavorText = "Your men do their very best to break through the enemy line. The fighting is brutal, but eventually even the steadfast cries of your centurions cannot"
                    + " convince your men to throw themselves into the enemy yet again. The formation quits the field in disarray.";
            }
        }
        else if (powerBalance < 1)
        {
            if (playerAttacking)
            {
                flavorText = "Your men charge bravely into batle against great odds. They put up a valiant fight, but are eventually routed.";
            }
            else
            {
                flavorText = "Your men resist the enemy's charge for as long as possible, but eventually your bloodied and exhausted legionaries quit the field in disarray.";
            }
        }
        combatInfoPanelFlavorText.text = flavorText;
        combatInfoPanelButtonText.text = playerVictory ? "Optume!" : "Terribilis!";
        combatInfoPanel.SetActive(true);
    }

    PlayerFlank GetRandomUnroutedPlayerFlank()
    {
        List<PlayerFlank> unroutedFlanks = new List<PlayerFlank>();
        if (!battle.pLeftRouted)
        {
            unroutedFlanks.Add(PlayerFlank.Left);
        }
        if (!battle.pCenterRouted)
        {
            unroutedFlanks.Add(PlayerFlank.Center);
        }
        if (!battle.pRightRouted)
        {
            unroutedFlanks.Add(PlayerFlank.Right);
        }
        return unroutedFlanks[Random.Range(0, unroutedFlanks.Count)];
    }

    void DamageRandomFlank()
    {
        switch(GetRandomUnroutedPlayerFlank())
        {
            case PlayerFlank.Left:
                battle.pLeft = (int)(battle.pLeft * 0.8);
                break;
            case PlayerFlank.Center:    
                battle.pCenter = (int)(battle.pCenter * 0.8);
                break;
            case PlayerFlank.Right:
                battle.pRight = (int)(battle.pRight * 0.8);
                break;
        }
    }

    void RoutRandomFlank()
    {   
        Rout(GetRandomUnroutedPlayerFlank());
    }

    void MakeRetreatPanel(RetreatType type)
    {
        string subtitleText = "";
        string buttonText = "";
        string flavorText = "";
        switch (type)
        {
            case RetreatType.TotalSuccess:
                subtitleText = "Complete success";
                buttonText = "Optume!";
                flavorText = "Your men withdraw in good order, and your skilled rearguard prevents all but the most negligible casualties.";
                break;
            case RetreatType.PartialSuccess:
                subtitleText = "Partial success";
                buttonText = "Bene...";
                flavorText = "Your men withdraw in good order, but your rearguard gets bogged down in heavy fighting, sustaining heavy losses.";

                break;
            case RetreatType.Failure:
                subtitleText = "Failure";
                buttonText = "Terribilis!";
                flavorText = "The order to retreat is not properly communicated to your force, and confusion breaks out in the ranks. One of your flanks breaks into a rout."
                    + "\nWhile your centurions desperately try to restore order, the enemy siezes the initiative.";
                break;
        }
        retreatInfoPanelSubtitleText.text = subtitleText;
        retreatInfoPanelButtonText.text = buttonText;
        retreatInfoPanelFlavorText.text = flavorText;
        retreatInfoPanel.SetActive(true);
    }

    float GetFlankPower(PlayerFlank flank)
    {
        switch(flank)
        {
            case PlayerFlank.None:
                Debug.LogError("Attempted to calculate flank power of an empty flank");
                break;
            case PlayerFlank.Left:
                return (float) battle.pLeft + (battle.pCenterRouted ? 0f : (battle.pCenter / 3f));
            case PlayerFlank.Center:
                return (float) battle.pCenter
                    + (battle.pLeftRouted ? 0f : (battle.pLeft / 3f))
                    + (battle.pRightRouted ? 0f : (battle.pRight / 3f));
            case PlayerFlank.Right:
                return (float) battle.pRight + (battle.pCenterRouted ? 0f : (battle.pCenter / 3f));
        }
        return -1f;
    }

    float GetFlankPower(EnemyFlank flank)
    {
        switch (flank)
        {
            case EnemyFlank.Left:
                return (float) battle.eLeft + (battle.eCenter / 3f);
            case EnemyFlank.Center:
                return (float) battle.eCenter + (battle.eLeft / 3f) + (battle.eRight / 3f);
            case EnemyFlank.Right:
                return (float) battle.eRight + (battle.eCenter / 3f);
        }
        return -1f;
    }

    void MakeRoutMarker(GameObject parent)
    {
        GameObject newFlag = Instantiate(routedFlag, parent.transform);
        newFlag.transform.localScale = new Vector3(0.08f, 0.2f);
    }

    void Rout(PlayerFlank flank)
    {
        switch (flank)
        {
            case PlayerFlank.None:
                Debug.LogError("Attempted to rout an empty flank. Nice going guys");
                break;
            case PlayerFlank.Left:
                battle.pLeftRouted = true;
                MakeRoutMarker(playerLeftFlank);
                break;
            case PlayerFlank.Center:
                battle.pCenterRouted = true;
                MakeRoutMarker(playerCenterFlank);
                break;
            case PlayerFlank.Right:
                battle.pRightRouted = true;
                MakeRoutMarker(playerRightFlank);
                break;
        }
    }

    void Rout(EnemyFlank flank)
    {
        switch (flank)
        {
            case EnemyFlank.Left:
                battle.eLeftRouted = true;
                MakeRoutMarker(enemyLeftFlank);
                break;
            case EnemyFlank.Center:
                MakeRoutMarker(enemyCenterFlank);
                battle.eCenterRouted = true;
                break;
            case EnemyFlank.Right:
                MakeRoutMarker(enemyRightFlank);
                battle.eRightRouted = true;
                break;
        }
    }

    public void AttackButton()
    {
        if (state != BattleState.SelectAttackSourceFlank && state != BattleState.SelectAttackTargetFlank)
        {
            Debug.Log("Select attacking flank");
            state = BattleState.SelectAttackSourceFlank;
        }
        else
        {
            state = BattleState.Default;
            currentSelectedPlayerFlank = PlayerFlank.None;
            Debug.Log("Attack cancelled");
        }
    }

    public void ScoutButton()
    {
        if (state != BattleState.SelectScoutTargetFlank)
        {
            Debug.Log("Select enemy flank to scout");
            state = BattleState.SelectScoutTargetFlank;
        }
        else
        {
            state = BattleState.Default;
            Debug.Log("Scouting cancelled");
        }
    }

    public void TacticButton()
    {
        // To be implemented...
    }

    public void RetreatButton()
    {
        float successModifier = (Random.value * 5f) + (float) gm.player.fortBonus;
        Debug.Log("Player attempted to retreat with success modifier " + successModifier.ToString("0.00"));
        if (successModifier > 3)
        {
            state = BattleState.Retreat;
            MakeRetreatPanel(RetreatType.TotalSuccess);
        }
        else if (successModifier > 2)
        {
            // Some losses
            DamageRandomFlank();
            state = BattleState.Retreat;
            MakeRetreatPanel(RetreatType.PartialSuccess);
        }
        else
        {
            // Unable to retreat
            RoutRandomFlank();
            MakeRetreatPanel(RetreatType.Failure);
            RunEnemyTurn();
        }
    }

    public void EnemyLeftFlankClicked()
    {
        if (state == BattleState.SelectScoutTargetFlank)
        {
            Scout(EnemyFlank.Left);
        }
        else if (state == BattleState.SelectAttackTargetFlank && !battle.eLeftRouted)
        {
            Attack(currentSelectedPlayerFlank, EnemyFlank.Left, true);
        }
    }

    public void EnemyCenterFlankClicked()
    {
        if (state == BattleState.SelectScoutTargetFlank)
        {
            Scout(EnemyFlank.Center);
        }
        else if (state == BattleState.SelectAttackTargetFlank && !battle.eCenterRouted)
        {
            Attack(currentSelectedPlayerFlank, EnemyFlank.Center, true);
        }
    }

    public void EnemyRightFlankClicked()
    {
        if (state == BattleState.SelectScoutTargetFlank)
        {
            Scout(EnemyFlank.Right);
        }
        else if (state == BattleState.SelectAttackTargetFlank && !battle.eRightRouted)
        {
            Attack(currentSelectedPlayerFlank, EnemyFlank.Right, true);
        }
    }

    public void PlayerLeftFlankClicked()
    {
        if (state == BattleState.SelectAttackSourceFlank && !battle.pLeftRouted)
        {
            currentSelectedPlayerFlank = PlayerFlank.Left;
            state = BattleState.SelectAttackTargetFlank;
        }
    }

    public void PlayerCenterFlankClicked()
    {
        if (state == BattleState.SelectAttackSourceFlank && !battle.pCenterRouted)
        {
            currentSelectedPlayerFlank = PlayerFlank.Center;
            state = BattleState.SelectAttackTargetFlank;
        }
    }

    public void PlayerRightFlankClicked()
    {
        if (state == BattleState.SelectAttackSourceFlank && !battle.pRightRouted)
        {
            currentSelectedPlayerFlank = PlayerFlank.Right;
            state = BattleState.SelectAttackTargetFlank;
        }
    }
}
