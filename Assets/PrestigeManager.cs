using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyLabs;
using System;


public class PrestigeManager : MonoBehaviour
{
    public Text CurrentFame;
    public Text PrestigeFame;
    public Text FameMultiplier;
    public Text PrestigeMultiplier;

    ComicManager cm;
    MediaManager mm;
    SushiManager sm;
    VincyManager vm;
    ShoeManager som;
    MajorkeyManager mkm;
    PineappleManager pm;
    JewelManager jm;
    DonManager dm;
    GalacticManager gm;
    GameObject prestigeMenu;
    IdleGame ig;
    UpgradeManager upgradeManager;


    Button prestigeMenuButton;
    Button prestigeButton;


    public bool prestigeMenuShowing;

    // Start is called before the first frame update
    void Start()
    {
        CurrentFame = GameObject.Find("CurrentFame").GetComponent<Text>();
        PrestigeFame = GameObject.Find("PrestigeFame").GetComponent<Text>();
        FameMultiplier = GameObject.Find("FameMultiplier").GetComponent<Text>();
        PrestigeMultiplier = GameObject.Find("PrestigeMultiplier").GetComponent<Text>();


        ig = GameObject.Find("GameManager").GetComponent<IdleGame>();
        upgradeManager = GameObject.Find("UpgradeManager").GetComponent<UpgradeManager>();

        cm = GameObject.Find("ComicManager").GetComponent<ComicManager>();
        mm = GameObject.Find("MediaManager").GetComponent<MediaManager>();
        sm = GameObject.Find("SushiManager").GetComponent<SushiManager>();
        vm = GameObject.Find("VincyManager").GetComponent<VincyManager>();
        som = GameObject.Find("ShoeManager").GetComponent<ShoeManager>();
        mkm = GameObject.Find("MajorkeyManager").GetComponent<MajorkeyManager>();
        pm = GameObject.Find("PineappleManager").GetComponent<PineappleManager>();
        jm = GameObject.Find("JewelManager").GetComponent<JewelManager>();
        dm = GameObject.Find("DonManager").GetComponent<DonManager>();
        gm = GameObject.Find("GalacticManager").GetComponent<GalacticManager>();

        prestigeMenu = GameObject.Find("PrestigeBackground");
        prestigeMenuButton = GameObject.Find("prestigeMenuButton").GetComponent<Button>();
        prestigeButton = GameObject.Find("PrestigeButton").GetComponent<Button>();
        prestigeMenuShowing = false;
        prestigeMenu.SetActive(false);
        prestigeMenuButton.onClick.AddListener(openPrestige);
        prestigeButton.onClick.AddListener(prestige);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void openPrestige()
    {
        if (prestigeMenuShowing == false){
            prestigeMenu.SetActive(true);
            ig.upgradeMenu.SetActive(false);
            ig.upgradeManager.upgMenuShowing = false;
            prestigeMenuShowing = true;
            double fameToGet = Math.Floor(150 * System.Math.Sqrt(ig.totalCoins / 1e8));
            CurrentFame.text = "Current Demons: " + ShortScale.ParseDouble(ig.fame, 2);
            PrestigeFame.text = "You'll recieve: : " + ShortScale.ParseDouble(fameToGet, 0);
            FameMultiplier.text = "Bonus Per Demon: " + (ig.fameMultiplier*100) + "%";
            double multiplier = ((fameToGet + ig.fame +1)/(ig.fame+1));

            PrestigeMultiplier.text = "Profit speed: 1x ->" + multiplier + "x";


        }else{
            prestigeMenu.SetActive(false);
            prestigeMenuShowing = false;
        }
    }


    public void prestige(){
        //if(ig.totalCoins >= ig.coinsToPrestige){
              ig.fame = ig.fame + Math.Floor(150 * System.Math.Sqrt(ig.totalCoins / 1e8));
              ig.prestige();
              cm.prestige();
              mm.prestige();
              sm.prestige();
              vm.prestige();
              som.prestige();
              mkm.prestige();
              pm.prestige();
              jm.prestige();
              dm.prestige();
              gm.prestige();
              upgradeManager.prestige();
              double fameToGet = Math.Floor(150 * System.Math.Sqrt(ig.totalCoins / 1e8));
              CurrentFame.text = "Current Demons: " + ShortScale.ParseDouble(ig.fame, 2);
              PrestigeFame.text = "You'll recieve: : " + ShortScale.ParseDouble(fameToGet, 0);
              FameMultiplier.text = "Bonus Per Demon: " + (ig.fameMultiplier*100) + "%";
              double multiplier = ((fameToGet + ig.fame +1)/(ig.fame+1));

              PrestigeMultiplier.text = "Profit speed: 1x ->" + multiplier + "x";
              prestigeMenu.SetActive(false);
              prestigeMenuShowing = false;
        //}
    }
}
