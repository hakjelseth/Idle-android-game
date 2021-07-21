using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PolyLabs;


public class SushiManager : MonoBehaviour
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
    public int ShopIndex = 2;
    public double shopPrice;
    public double buyingPrice;
    public double max;
    public double basePrice = 725;
    public double increment = 1.14;
    public bool autoShop = false;
    public bool shopRuns = false;
    public double shopReward;
    public int[] UpgradeArray = {25, 50, 100, 200, 300, 400, 500};
    double[] shopUpgradePrice = new double[]{100000, 1000000, (100 * Math.Pow(10, 12)), (7 * Math.Pow(10, 18)), (100 * Math.Pow(10, 21)), (25 * Math.Pow(10, 27)), (25 * Math.Pow(10, 42)), (500 * Math.Pow(10, 45)), (100 * Math.Pow(10, 60))};

    IdleGame ig;
    BuyManager BM;
    Coroutine routine;
    public Slider progressBar;


    void Start()
      {

        ig = GameObject.Find("GameManager").GetComponent<IdleGame>();
        BM = GameObject.Find("BuyManager").GetComponent<BuyManager>();

        numberOfShopsText = GameObject.Find("SushiShops").GetComponent<Text>();
        buyButtonText = GameObject.Find("buySushiText").GetComponent<Text>();
        moneyText = GameObject.Find("sushiMoneyText").GetComponent<Text>();
        buyAmount = GameObject.Find("SushiBuyAmount").GetComponent<Text>();
        buyButton = GameObject.Find("buySushiShop").GetComponent<Button>();
        buyButtonImage = GameObject.Find("buySushiShop").GetComponent<Image>();
        openButton = GameObject.Find("startSushi").GetComponent<Button>();
        progressBar = GameObject.Find("Sushi Progress Bar").GetComponent<Slider>();
        buyButton.onClick.AddListener(buyShop);
        openButton.onClick.AddListener(startShop);
        if(ig.saved == true){
            numberOfShops = ig.numShops[2];
            shopLvl = ig.shopLvls[2];
            shopPrice = 725*Math.Pow(1.14, numberOfShops);
            shopReward = ig.shopRewards[2];
            autoShop = ig.shopAutomation[2];
            runTime = ig.shopRunTime[2];
            while(ig.numShops[ShopIndex] >= UpgradeArray[ig.shopUpgradeIndex[ShopIndex]]){
                ig.shopUpgradeIndex[ShopIndex]++;
            }
        }else{
            ig.numShops[ShopIndex] = 0;
            shopPrice = 725;
            shopReward = 540;
			ig.shopRewards[2] = shopReward;
            autoShop = false;
            runTime = 10;
            ig.shopRunTime[2] = runTime;
        }
        progressBar.value = 0;
        timer = 0;
        if(autoShop == true && ig.numShops[ShopIndex] > 0){
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

        (buyingPrice, max, buyAmountInt) = BM.UpdateBuyingPrice(basePrice, increment, 2);

        
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
        string time = string.Format("{0:00}:{1:00}:{2:00}",(Math.Floor(((Math.Ceiling((double)ig.shopRunTime[2]/ig.speedval))-timer)/3600)),((Math.Floor(((Math.Ceiling((double)ig.shopRunTime[2]/ig.speedval))-timer)/60))%60),(((Math.Ceiling((double)ig.shopRunTime[2]/ig.speedval))-timer)%60));
        moneyText.text = "$" + ShortScale.ParseDouble(ig.numShops[2]*ig.shopRewards[2]*ig.profitMultiplier, 2) + " " + time;
    }

    public void startShop()
       {
        if(ig.numShops[2] > 0 && shopRuns == false){

             routine = StartCoroutine(CountDown());
           }
      }

      public void buyShop()
              {
                  BM.buyShop(2, buyingPrice, max);
              }

    public bool upgradeShop(int multiplier, int index)
      {
         if(ig.coins >= shopUpgradePrice[index]){
              if(multiplier == 0){
                   autoShop = true;
                   ig.shopAutomation[2] = true;
                   if(shopRuns == false && ig.numShops[ShopIndex] > 0){
                        routine = StartCoroutine(CountDown());
                   }
              }else{
                  ig.shopRewards[ShopIndex] = ig.shopRewards[ShopIndex]*multiplier;
              }
              ig.coins -= (double)shopUpgradePrice[index];
              shopLvl++;
              ig.shopLvls[2] = shopLvl;
              return true;
         }
         return false;
      }

    public void prestige(){
        progressBar.value = 0;
        shopPrice = 725;
        runTime = 10;
        ig.shopRunTime[2] = runTime;
        shopReward = 540;
        ig.shopRewards[2] = shopReward;
        ig.numShops[ShopIndex] = 0;
        timer = 0;
        upgradeIndex = 0;
        shopLvl = 0;
        ig.shopLvls[2] = shopLvl;
        autoShop = false;
        if(shopRuns == true){
            StopCoroutine(routine);
        }
        shopRuns = false;

    }
}