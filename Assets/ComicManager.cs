using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PolyLabs;


public class ComicManager : MonoBehaviour
{
    public Text numberOfShopsText;
    public Text buyButtonText;
    public Text moneyText;
    public Text buyAmount;
    public Text upgradeButtonText;
    public Button buyButton;
    public Button openButton;
	public Image buyButtonImage;
    public float timer;
    public float runTime;
    public int shopLvl;
	public double buyingPrice;
	public double max;
	public double basePrice = 4;
	public double increment = 1.07;
    public int numberOfShops;
    public int ShopIndex = 0;
	public int upgradeIndex = 0;
    public double shopPrice;
    public bool autoShop = false;
    public bool shopRuns = false;
    public double shopReward;
    public int[] UpgradeArray = {25, 50, 100, 200, 300, 400, 500};
    double[] shopUpgradePrice = new double[]{1000, 250000, (20 * Math.Pow(10, 12)), (2 * Math.Pow(10, 18)),(25 * Math.Pow(10, 21)), (1 * Math.Pow(10, 27)),(25 * Math.Pow(10, 45)),(500 * Math.Pow(10, 48)),(100 * Math.Pow(10, 78)),(10 * Math.Pow(10, 93)),(300 * Math.Pow(10, 99)),(500 * Math.Pow(10, 114)),(750 * Math.Pow(10, 114)),(1 * Math.Pow(10, 117)),(555 * Math.Pow(10, 120)),(1 * Math.Pow(10, 127)),(71 * Math.Pow(10, 129)),(900 * Math.Pow(10, 135)), (799 * Math.Pow(10, 141)), (300 * Math.Pow(10, 144)), (240 * Math.Pow(10, 150)), (32 * Math.Pow(10, 156)), (200 * Math.Pow(10, 159)), (500 * Math.Pow(10, 171)), (233 * Math.Pow(10, 204)), (10 * Math.Pow(10, 213)), (150 * Math.Pow(10, 213)), (800 * Math.Pow(10, 216)), (600 * Math.Pow(10, 219)), (1 * Math.Pow(10, 228)), (3 * Math.Pow(10, 231)), (63 * Math.Pow(10, 234)), (1 * Math.Pow(10, 240)), (2 * Math.Pow(10, 243)), (10 * Math.Pow(10, 252)), (50 * Math.Pow(10, 252)), (6.4 * Math.Pow(10, 258)), (700 * Math.Pow(10, 261)), (1 * Math.Pow(10, 273))};
    IdleGame ig;
	BuyManager BM;
    Coroutine routine;
    public Slider progressBar;


    void Start()
      {
        ig = GameObject.Find("GameManager").GetComponent<IdleGame>();
        BM = GameObject.Find("BuyManager").GetComponent<BuyManager>();


        numberOfShopsText = GameObject.Find("ComicShops").GetComponent<Text>();
        buyButtonText = GameObject.Find("buyComicText").GetComponent<Text>();
        moneyText = GameObject.Find("comicMoneyText").GetComponent<Text>();
        buyAmount = GameObject.Find("buyAmount").GetComponent<Text>();
        buyButton = GameObject.Find("buyComicShop").GetComponent<Button>();
		buyButtonImage = GameObject.Find("buyComicShop").GetComponent<Image>();
        openButton = GameObject.Find("startComicBook").GetComponent<Button>();
        progressBar = GameObject.Find("Comic Progress Bar").GetComponent<Slider>();
        buyButton.onClick.AddListener(buyShop);
        openButton.onClick.AddListener(startShop);

        if(ig.saved == true){
            numberOfShops = ig.numShops[0];
            shopLvl = ig.shopLvls[0];
            shopReward = ig.shopRewards[0];
            autoShop = ig.shopAutomation[0];
            runTime = ig.shopRunTime[0];
            while(ig.numShops[ShopIndex] >= UpgradeArray[ig.shopUpgradeIndex[ShopIndex]]){
                ig.shopUpgradeIndex[ShopIndex]++;
            }
        }else{
            ig.numShops[0] = 0;
            shopPrice = 0;
            shopReward = 1;
            ig.shopRewards[0] = shopReward;
            autoShop = false;
            runTime = 1;
            ig.shopRunTime[0] = runTime;
        }
        progressBar.value = 0;
        timer = 0;
        if(autoShop == true && ig.numShops[0] > 0){
          routine = StartCoroutine(CountDown());
        }
      }



    IEnumerator CountDown()
    {	
		string time;
       shopRuns = true;
       timer = 0;
       progressBar.value = 0;
       while(timer  < (double)(ig.shopRunTime[0]/ig.speedval)){
        	time = string.Format("{0:00}:{1:00}:{2:00}",(Math.Floor(((Math.Ceiling((double)ig.shopRunTime[0]/ig.speedval))-timer)/3600)),((Math.Floor(((Math.Ceiling((double)ig.shopRunTime[0]/ig.speedval))-timer)/60))%60),(((Math.Ceiling((double)ig.shopRunTime[0]/ig.speedval))-timer)%60));
        	moneyText.text = "$" + ShortScale.ParseDouble(ig.numShops[0]*ig.shopRewards[ShopIndex]*ig.profitMultiplier, 2) + " " + time;
            timer++;
            progressBar.value = (float)(timer/(ig.shopRunTime[0]/ig.speedval));

            if(ig.shopRunTime[ShopIndex] < 1){
                yield return new WaitForSeconds(ig.shopRunTime[ShopIndex]);
            }else{
                yield return new WaitForSeconds(1f);
            }

       }
       timer = 0;
       time = string.Format("{0:00}:{1:00}:{2:00}",(Math.Floor(((Math.Ceiling((double)ig.shopRunTime[0]/ig.speedval))-timer)/3600)),((Math.Floor(((Math.Ceiling((double)ig.shopRunTime[0]/ig.speedval))-timer)/60))%60),(((Math.Ceiling((double)ig.shopRunTime[0]/ig.speedval))-timer)%60));
       moneyText.text = "$" + ShortScale.ParseDouble(ig.numShops[0]*ig.shopRewards[ShopIndex]*ig.profitMultiplier, 2) + " " + time;
       ig.coins += (double)((ig.shopRewards[0]*ig.numShops[0])*ig.profitMultiplier);
       ig.totalCoins +=(double)((ig.shopRewards[0]*ig.numShops[0])*ig.profitMultiplier);
       ig.UpdateAllText();
       progressBar.value = 0;
       if(autoShop == true && ig.numShops[0] > 0){
            routine = StartCoroutine(CountDown());
        }else{
            shopRuns = false;
        }
    }

    public void updateText(){
		double buyAmountInt;

		(buyingPrice, max, buyAmountInt) = BM.UpdateBuyingPrice(basePrice, increment, 0);
		if(ig.numShops[0] == 0 && buyAmountInt == 1){
			buyingPrice = 0;
		}

		if(ig.coins < buyingPrice){
			buyButtonImage.color = new Color32(7,87,12,255);
			buyButtonText.color = Color.white;
		}else{
			buyButtonImage.color = new Color32(9,250,0,255);
			buyButtonText.color = new Color32(50,50,50,255);
		}
        numberOfShopsText.text = ig.numShops[0] + "/" + UpgradeArray[ig.shopUpgradeIndex[0]];
        buyButtonText.text = "$" + ShortScale.ParseDouble(buyingPrice, 1);
		buyAmount.text = "x" + buyAmountInt;
        string time = string.Format("{0:00}:{1:00}:{2:00}",(Math.Floor(((Math.Ceiling((double)ig.shopRunTime[0]/ig.speedval))-timer)/3600)),((Math.Floor(((Math.Ceiling((double)ig.shopRunTime[0]/ig.speedval))-timer)/60))%60),(((Math.Ceiling((double)ig.shopRunTime[0]/ig.speedval))-timer)%60));
        moneyText.text = "$" + ShortScale.ParseDouble(ig.numShops[0]*ig.shopRewards[0]*ig.profitMultiplier, 2) + " " + time;
     }

    public void startShop()
       {
        if(ig.numShops[0] > 0 && shopRuns == false)
        {
            routine = StartCoroutine(CountDown());
           }
      }

    public void buyShop()
    {
        BM.buyShop(0, buyingPrice, max);
    }

    public bool upgradeShop(int multiplier, int index)
      {
         if(ig.coins >= shopUpgradePrice[index]){
              if(multiplier == 0){
                   autoShop = true;
                   ig.shopAutomation[0] = true;
                   if(shopRuns == false && ig.numShops[0] > 0){
                        routine = StartCoroutine(CountDown());
                   }
              }else{
                  ig.shopRewards[ShopIndex] = ig.shopRewards[ShopIndex]*multiplier;
              }
              ig.coins -= (double)shopUpgradePrice[index];
              shopLvl++;
              ig.shopLvls[0] = shopLvl;

              return true;
         }
         return false;
      }


    public void prestige(){
        progressBar.value = 0;
        shopPrice = 0;
        runTime = 1;
        ig.shopRunTime[0] = runTime;
        shopReward = 1;
        ig.shopRewards[0] = 1;
        shopLvl = 0;
        ig.shopLvls[0] = shopLvl;
        ig.numShops[0] = 0;
        timer = 0;
		upgradeIndex = 0;
        autoShop = false;
        if(shopRuns == true){
            StopCoroutine(routine);
        }
        shopRuns = false;

    }

}
