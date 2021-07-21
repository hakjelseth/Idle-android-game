using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PolyLabs;


public class MediaManager : MonoBehaviour
{
    public Text numberOfShopsText;
    public Text buyButtonText;
    public Text upgradeButtonText;
    public Text moneyText;
    public Text buyAmount;
    public Button buyButton;
    public Button openButton;
    public Image buyButtonImage;
    public float timer;
    public float runTime;
    public int shopLvl;
    public int numberOfShops;
    public int upgradeIndex = 0;
    public int ShopIndex = 1;
	public double buyingPrice;
	public double max;
	public double basePrice = 65;
	public double increment = 1.15;
    public double shopPrice;
    public bool autoShop = false;
    public bool shopRuns = false;
    public double shopReward;
    public int[] UpgradeArray = {25, 50, 100, 200, 300, 400, 500};
    double[] shopUpgradePrice = new double[]{15000, 500000, (50 * Math.Pow(10, 12)), (5 * Math.Pow(10, 18)),(50 * Math.Pow(10, 21)), (5 * Math.Pow(10, 42)),(250 * Math.Pow(10, 45)), (10 * Math.Pow(10, 60)), (1 * Math.Pow(10, 90)),(2 * Math.Pow(10, 96)),(2 * Math.Pow(10, 117)),(20 * Math.Pow(10, 117)),(150 * Math.Pow(10, 117)),(700 * Math.Pow(10, 117)),(3 * Math.Pow(10, 123)),(250 * Math.Pow(10, 129)),(5 * Math.Pow(10, 132)),(136 * Math.Pow(10, 138)),(3 * Math.Pow(10, 144)),(30 * Math.Pow(10, 147)), (888 * Math.Pow(10, 153)), (10 * Math.Pow(10, 159)),  (500 * Math.Pow(10, 162)),   (14 * Math.Pow(10, 201)),(421 * Math.Pow(10, 204)), (10 * Math.Pow(10, 213)),  (166 * Math.Pow(10, 213)),   (800 * Math.Pow(10, 216)), (789 * Math.Pow(10, 219)), (1 * Math.Pow(10, 228)), (8 * Math.Pow(10, 231)),(199 * Math.Pow(10, 234)), (5 * Math.Pow(10, 240)),(22 * Math.Pow(10, 243)),(10 * Math.Pow(10, 252)),(75 * Math.Pow(10, 252)),(12.2 * Math.Pow(10, 258)),(1 * Math.Pow(10, 264)),(2 * Math.Pow(10, 273)),(1 * Math.Pow(10, 285))};

    Coroutine routine;
    IdleGame ig;
	BuyManager BM;

    public Slider progressBar;

    void Start()
      {
        ig = GameObject.Find("GameManager").GetComponent<IdleGame>();
        BM = GameObject.Find("BuyManager").GetComponent<BuyManager>();

        numberOfShopsText = GameObject.Find("MediaShops").GetComponent<Text>();
        buyButtonText = GameObject.Find("buyMediaText").GetComponent<Text>();
        moneyText = GameObject.Find("mediaMoneyText").GetComponent<Text>();
        buyAmount = GameObject.Find("MediaBuyAmount").GetComponent<Text>();
        buyButton = GameObject.Find("buyMediaShop").GetComponent<Button>();
        buyButtonImage = GameObject.Find("buyMediaShop").GetComponent<Image>();
        openButton = GameObject.Find("startMedia").GetComponent<Button>();
        progressBar = GameObject.Find("Media Progress Bar").GetComponent<Slider>();
        buyButton.onClick.AddListener(buyShop);
        openButton.onClick.AddListener(startShop);
        if(ig.saved == true){
            numberOfShops = ig.numShops[1];
            shopLvl = ig.shopLvls[1];
            shopPrice = 65*Math.Pow(1.15, numberOfShops);
            shopReward = ig.shopRewards[1];
            autoShop = ig.shopAutomation[1];
            runTime = ig.shopRunTime[1];
            while(ig.numShops[ShopIndex] >= UpgradeArray[ig.shopUpgradeIndex[ShopIndex]]){
                ig.shopUpgradeIndex[ShopIndex]++;
            }
        }else{
            numberOfShops = 0;
            shopPrice = 65;
            shopReward = 60;
            ig.shopRewards[1] = 60;
            autoShop = false;
            runTime = 6;
            ig.shopRunTime[1] = 6;
        }
        progressBar.value = 0;
        timer = 0;

        if(autoShop == true && ig.numShops[1] > 0){
          routine = StartCoroutine(CountDown());
        }
      }



    IEnumerator CountDown()
    {
		string time;
       shopRuns = true;
       timer = 0;
       progressBar.value = 0;
       while(timer  < Math.Ceiling((double)(ig.shopRunTime[ShopIndex]/ig.speedval))){
            timer++;
			time = string.Format("{0:00}:{1:00}:{2:00}",(Math.Floor(((Math.Ceiling((double)ig.shopRunTime[ShopIndex]/ig.speedval))-timer)/3600)),((Math.Floor(((Math.Ceiling((double)ig.shopRunTime[ShopIndex]/ig.speedval))-timer)/60))%60),(((Math.Ceiling((double)ig.shopRunTime[ShopIndex]/ig.speedval))-timer)%60));
       		moneyText.text = "$" + ShortScale.ParseDouble(ig.numShops[ShopIndex]*ig.shopRewards[ShopIndex]*ig.profitMultiplier, 2) + " " + time;
            progressBar.value = (float)(timer/(ig.shopRunTime[ShopIndex]/ig.speedval));

            if(ig.shopRunTime[ShopIndex] < 1){
                yield return new WaitForSeconds(ig.shopRunTime[ShopIndex]);
            }else{
                yield return new WaitForSeconds(1f);
            }
       }
       timer = 0;
       time = string.Format("{0:00}:{1:00}:{2:00}",(Math.Floor(((Math.Ceiling((double)ig.shopRunTime[ShopIndex]/ig.speedval))-timer)/3600)),((Math.Floor(((Math.Ceiling((double)ig.shopRunTime[ShopIndex]/ig.speedval))-timer)/60))%60),(((Math.Ceiling((double)ig.shopRunTime[ShopIndex]/ig.speedval))-timer)%60));
       moneyText.text = "$" + ShortScale.ParseDouble(ig.numShops[ShopIndex]*ig.shopRewards[ShopIndex]*ig.profitMultiplier, 2) + " " + time;
       ig.coins += (double)((ig.shopRewards[ShopIndex]*ig.numShops[ShopIndex])*ig.profitMultiplier);
       ig.totalCoins +=(double)((ig.shopRewards[ShopIndex]*ig.numShops[ShopIndex])*ig.profitMultiplier);
       ig.UpdateAllText();
       progressBar.value = 0;
       if(autoShop == true && ig.numShops[ShopIndex] > 0){
            routine = StartCoroutine(CountDown());
        }else{
            shopRuns = false;
        }
    }

    public void updateText(){
		double buyAmountInt = ig.buyMultiplier;

		(buyingPrice, max, buyAmountInt) = BM.UpdateBuyingPrice(basePrice, increment, 1);

        
        if(ig.coins < buyingPrice){
            buyButtonImage.color = new Color32(7,87,12,255);
            buyButtonText.color = Color.white;
        }else{
            buyButtonImage.color = new Color32(9,250,0,255);
            buyButtonText.color = new Color32(50,50,50,255);
        }
        numberOfShopsText.text = ig.numShops[ShopIndex] + "/" + UpgradeArray[ig.shopUpgradeIndex[ShopIndex]];
        buyButtonText.text = "$" + ShortScale.ParseDouble(buyingPrice, 1);
		buyAmount.text = "x" + buyAmountInt;
        string time = string.Format("{0:00}:{1:00}:{2:00}",(Math.Floor(((Math.Ceiling((double)ig.shopRunTime[1]/ig.speedval))-timer)/3600)),((Math.Floor(((Math.Ceiling((double)ig.shopRunTime[1]/ig.speedval))-timer)/60))%60),(((Math.Ceiling((double)ig.shopRunTime[1]/ig.speedval))-timer)%60));
        moneyText.text = "$" + ShortScale.ParseDouble(ig.numShops[1]*ig.shopRewards[1]*ig.profitMultiplier, 2) + " " + time;
    }

    public void startShop()
       {
        if(ig.numShops[1] > 0 && shopRuns == false){

             routine = StartCoroutine(CountDown());
           }
      }

    public void buyShop()
    {
    
        BM.buyShop(1, buyingPrice, max);

    }

    public bool upgradeShop(int multiplier, int index)
      {
         if(ig.coins >= shopUpgradePrice[index]){
              if(multiplier == 0){
                   autoShop = true;
                   ig.shopAutomation[1] = true;
                   if(shopRuns == false && ig.numShops[ShopIndex] > 0){
                        routine = StartCoroutine(CountDown());
                   }
              }else{
                  ig.shopRewards[1] = ig.shopRewards[1]*multiplier;
              }
              ig.coins -= (double)shopUpgradePrice[index];
              shopLvl++;
              ig.shopLvls[1] = shopLvl;
              return true;
         }
         return false;
      }


    public void prestige(){
        progressBar.value = 0;
        shopPrice = 65;
        runTime = 6;
        ig.shopRunTime[1] = 6;
        shopReward = 60;
        ig.shopRewards[1] = 60;
        ig.numShops[ShopIndex] = 0;
        shopLvl = 0;
        timer = 0;
        upgradeIndex = 0;
        autoShop = false;
        if(shopRuns == true){
            StopCoroutine(routine);
        }
        shopRuns = false;

    }
}