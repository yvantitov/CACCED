using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreBattleManager : MonoBehaviour
{

    public static List<string> provinces = new List<string>
    {
        "Sicilia",
        "Sardinia and Corsica",
        "Hispania Citerior",
        "Hispania Ulterior",
        "Macedonia",
        "Africa",
        "Asia",
        "Gallia Narbonensis",
        "Crete and Cyrenaica",
        "Bithynia et Pontus",
        "Syria",
        "Cilicia",
        "Cyprus",
        "Africa Nova",
        "Cisalpine Gaul",
        "Italia",
        "Aegyptus",
        "Achaia",
        "Lusitania",
        "Illyricum",
        "Aquitania",
        "Gallia Lugdunensis",
        "Galatia",
        "Africa Proconsularis",
        "Gallia Belgica",
        "Raetia",
        "Moesia",
        "Judaea",
        "Cappadocia"
    };

    public enum InfoPanelType
    {
        Talk,
        Recon,
        Recruit,
        Fortify
    };

    public Text playerStatsText;
    public Text turnsRemText;
    public Text fortLevelText;
    public GameObject infoPopup;
    public GameObject infoPanelImage;
    public Text infoPopupText;
    public Sprite recruitSprite;
    public Sprite fortifySprite;
    public Sprite talkSprite;
    public Sprite reconSprite;

    private GameManager gm = GameManager.Instance;

    private int turnsRem;

    void Start()
    {
        turnsRem = gm.player.hasInitiative ? 6 : 5;
    }

    void Update()
    {
        // Update Player Stats
        playerStatsText.text = gm.player.manpower + " legions march at your command";
        turnsRemText.text = "Turns remaining: " + turnsRem;
        fortLevelText.text = "FL " + gm.player.fortBonus;
    }

    void MakePanel(string text, InfoPanelType panelType)
    {
        infoPopup.SetActive(true);
        infoPopupText.text = text;
        Sprite panelSprite = null;
        switch(panelType)
        {
            case InfoPanelType.Talk:
                panelSprite = talkSprite;
                break;
            case InfoPanelType.Recon:
                panelSprite = reconSprite;
                break;
            case InfoPanelType.Recruit:
                panelSprite = recruitSprite;
                break;
            case InfoPanelType.Fortify:
                panelSprite = fortifySprite;
                break;
        }
        infoPanelImage.GetComponent<SpriteRenderer>().sprite = panelSprite;
    }

    void Talk()
    {

    }

    void Recon()
    {

    }

    public void Recruit()
    {
        if (turnsRem > 0)
        {
            string homeProvince = provinces[Random.Range(0, provinces.Count)];
            Debug.Log("Player recruited troops from " + homeProvince);
            MakePanel(
                "With the help of fleet-footed messengers and a few friends in the Imperial court, you "
                + "have managed to procure reinforcements.\nA fresh legion has arrived from the province of " + homeProvince + "!",
                InfoPanelType.Recruit);
            gm.player.manpower++;
            turnsRem--;
        }
    }

    public void Fortify()
    {
        if (turnsRem > 0)
        {
            Debug.Log("Player fortified their position");
            string message = "";
            switch(gm.player.fortBonus)
            {
                case 0: 
                    message = "Sweating profusely in the midsummer sun, your men work tirelessly to surround your exposed"
                              + " encampment with wooden guard towers and palisades.\nThe work is grueling, but you know your men"
                              + " will sleep easier within the safety of Roman walls.";
                    break;
                case 1: 
                    message = "Centurions circle your camp like birds of prey, barking orders at those who would slack off"
                               + " under their watchful gaze.\nToday, the walls rise taller, and a new system of ditches, menacing with wooden stakes"
                               + " capable of felling a horse, surrounds your camp.";
                    break;
                default: 
                    message = "Under your direction, further nonstandard improvements have been made to the castra...";
                    break;
            }
            gm.player.fortBonus++;
            turnsRem--;
            MakePanel(message, InfoPanelType.Fortify);
        }
    }
}
