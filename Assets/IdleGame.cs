using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PolyLabs;
using UnityEngine.EventSystems;

public class IdleGame : MonoBehaviour
{
    public double coins;
    public double totalCoins;
    public double coinsToPrestige;
    public double profitMultiplier;
    public double speedval;
    public double fameMultiplier;
    public double fame;
    public Text coinsText;
    public Text buyMultiplierText;
    public Text offlineTimeText;
    public Text offlineProfitText;


    public int[] numShops = new int[10];
    public int[] shopLvls = new int[10];
    public double[] shopRewards = new double[10];
    public bool[] shopAutomation = new bool[10];
    public float[] shopRunTime = new float[10];
    public int[] speedMultipliers = new int[]{10, 25, 50, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000};
	public int[] shopUpgradeIndex = new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    public int speedIndex = 0;
    public ArrayList oldUpgrades = new ArrayList();
    public int buyMultiplier;


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
    public GameObject prestigeBackground;
    public GameObject upgradeMenu;
    public PrestigeManager prestigeManager;
    public UpgradeManager upgradeManager;
    GameObject upgradeList;
    public GameObject offlineScreen;
    public bool saved;
    public bool forsteGang;
    // Start is called before the first frame update
    void Start()
    {
        prestigeBackground = GameObject.Find("PrestigeBackground");
        upgradeList = GameObject.Find("Vertical Layout Group");
        upgradeMenu = GameObject.Find("upgradeMenu");
        buyMultiplier = 1;
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
        prestigeManager = GameObject.Find("PrestigeManager").GetComponent<PrestigeManager>();
        upgradeManager = GameObject.Find("UpgradeManager").GetComponent<UpgradeManager>();
        if(SaveSystem.LoadPlayer() == null){
                offlineScreen.SetActive(false);
                coins = 0;
                totalCoins = 0;
                coinsToPrestige = 1000000;
                profitMultiplier = 1;
                speedval = 1;
                fameMultiplier = 0.02;
                saved = false;
             }else{
                 saved = true;
                 LoadPlayer();
        }

		StartCoroutine(CountDown());
    }
		
	IEnumerator CountDown()
    	{
			yield return new WaitForSeconds(0.1f);
			UpdateAllText();	
		}

	void pikk(){
		CountDown();
	}
    // Update is called once per frame
    void Update()
    {
        coinsText.text = "$" + ShortScale.ParseDouble(coins, 2);

        SavePlayer();

    }

    public void prestige(){
        speedIndex = 0;
        coins = 0;
        coinsToPrestige = totalCoins + coinsToPrestige;
        totalCoins = 0;
        profitMultiplier = (fameMultiplier*fame) + 1;
        speedval = 1;
        for(int i = 0; i < 10; i++){
            numShops[i] = 0;
            shopLvls[i] = 0;
            shopAutomation[i] = false;
			shopUpgradeIndex[i] = 0;
        }
		UpdateAllText();
    }

    public void SavePlayer(){
        SaveSystem.SaveGame(this);

    }

    public void LoadPlayer(){
        SaveData data = SaveSystem.LoadPlayer();
        if(data == null){
            return;
        }else{
            coins = data.coins;
            totalCoins = data.totalCoins;
            coinsToPrestige = data.coinsToPrestige;
            profitMultiplier = data.profitMultiplier;
            speedval = data.speedval;
            fameMultiplier = data.fameMultiplier;
            fame = data.fame;
            numShops = data.numShops;
            speedIndex = data.speedIndex;
            shopLvls = data.shopLvls;
            shopRunTime = data.shopRunTime;
            oldUpgrades.AddRange(data.oldUpgrades);
            shopRewards = data.shopRewards;
            shopAutomation = data.shopAutomation;
            TimeSpan duration = DateTime.Now - DateTime.Parse(data.offlineTime);
            string offlineTime = string.Format("{0:00}:{1:00}:{2:00}", duration.Hours, duration.Minutes, duration.Seconds);
            offlineTimeText.text = "You were offline for \n" + offlineTime;
            double totalRewards = 0;
            double totalTime = 0;
            for (int i = 0; i < 10; i++)
            {
                if (numShops[i] > 0 && shopAutomation[i] == true)
                {   
                    totalRewards += numShops[i] * shopRewards[i];
                    totalTime += shopRunTime[i];    
                }
            }

            double profitPerSec = 0;
            if (totalTime > 0)
            {
                profitPerSec = totalRewards / totalTime;
                coins += profitPerSec * duration.TotalSeconds;
            }

            
            offlineProfitText.text = "You earned \n $" + ShortScale.ParseDouble((profitPerSec*duration.TotalSeconds), 2);
            
        }
    }

    public void offlineButton()
    {
        offlineScreen.SetActive(false);
    }

    public void buyMultiplierButton()
    {
        if (buyMultiplier == 1)
        {
            buyMultiplier = 10;
            buyMultiplierText.text = "10x";
        }
        
        else if (buyMultiplier == 10)
        {
            buyMultiplier = 100;
            buyMultiplierText.text = "100x";
        }
        
        else if (buyMultiplier == 100)
        {
            buyMultiplier = 2;
            buyMultiplierText.text = "NEXT";
        }
        
        else if (buyMultiplier == 2)
        {
            buyMultiplier = 3;
            buyMultiplierText.text = "MAX";
        }
        
        else if (buyMultiplier == 3)
        {
            buyMultiplier = 1;
            buyMultiplierText.text = "1x";
        }
		UpdateAllText();
    }

	public void UpdateText(int index){
		if(index == 0){
        	cm.updateText();
		}
		else if(index == 1){
		    mm.updateText();
		}
		else if(index == 2){
			sm.updateText();
		}
		else if(index == 3){
			vm.updateText();
		}
		else if(index == 4){
			som.updateText();
		}
		else if(index == 5){
			mkm.updateText();
		}
		else if(index == 6){
			pm.updateText();
		}
		else if(index == 7){
			jm.updateText();
		}
		else if(index == 8){
			dm.updateText();
		}
		else if(index == 9){
			gm.updateText();
		}

	}

	public void UpdateAllText(){
        cm.updateText();
        mm.updateText();
        sm.updateText();
        vm.updateText();
        som.updateText();
        mkm.updateText();
        pm.updateText();
        jm.updateText();
        dm.updateText();
        gm.updateText();
	}

	public void startShop(int index){
		if(index == 0){
        	cm.startShop();
		}
		else if(index == 1){
		    mm.startShop();
		}
		else if(index == 2){
			sm.startShop();
		}
		else if(index == 3){
			vm.startShop();
		}
		else if(index == 4){
			som.startShop();
		}
		else if(index == 5){
			mkm.startShop();
		}
		else if(index == 6){
			pm.startShop();
		}
		else if(index == 7){
			jm.startShop();
		}
		else if(index == 8){
			dm.startShop();
		}
		else if(index == 9){
			gm.startShop();
		}

	}
}
