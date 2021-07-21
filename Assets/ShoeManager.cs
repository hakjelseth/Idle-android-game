using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PolyLabs;


public class ShoeManager : MonoBehaviour
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
    public int ShopIndex = 4;
    public double shopPrice;
    public double buyingPrice;
    public double max;
    public double basePrice = 103860;
    public double increment = 1.12;
    public bool autoShop = false;
    public bool shopRuns = false;
    public double shopReward;
    public int[] UpgradeArray = {25, 50, 100, 200, 300, 400, 500};
    double[] shopUpgradePrice = new double[]{1200000, 10000000, (1 * Math.Pow(10, 15)), (300 * Math.Pow(10, 21)), (250 * Math.Pow(10, 27)), (100 * Math.Pow(10, 42)), (1 * Math.Pow(10, 48)), (100 * Math.Pow(10, 66))};

    IdleGame ig;
    BuyManager BM;
    Coroutine routine;
    public Slider progressBar;


    void Start()
      {

        ig = GameObject.Find("GameManager").GetComponent<IdleGame>();
        BM = GameObject.Find("BuyManager").GetComponent<BuyManager>();

        numberOfShopsText = GameObject.Find("ShoeShops").GetComponent<Text>();
        buyButtonText = GameObject.Find("buyShoeText").GetComponent<Text>();
        moneyText = GameObject.Find("shoeMoneyText").GetComponent<Text>();
        buyAmount = GameObject.Find("ShoeBuyAmount").GetComponent<Text>();
        buyButton = GameObject.Find("buyShoeShop").GetComponent<Button>();
        buyButtonImage = GameObject.Find("buyShoeShop").GetComponent<Image>();
        openButton = GameObject.Find("startShoes").GetComponent<Button>();
        progressBar = GameObject.Find("Shoe Progress Bar").GetComponent<Slider>();
        buyButton.onClick.AddListener(buyShop);
        openButton.onClick.AddListener(startShop);
        if(ig.saved == true){
            numberOfShops = ig.numShops[4];
            shopLvl = ig.shopLvls[4];
            shopPrice = 103860*Math.Pow(1.12, numberOfShops);
            shopReward = ig.shopRewards[4];
            autoShop = ig.shopAutomation[4];
            runTime = ig.shopRunTime[4];
            while(ig.numShops[ShopIndex] >= UpgradeArray[ig.shopUpgradeIndex[ShopIndex]]){
                ig.shopUpgradeIndex[ShopIndex]++;
            }
        }else{
            ig.numShops[ShopIndex] = 0;
            shopPrice = 103860;
            shopReward = 51840;
            ig.shopRewards[4] = shopReward;
            autoShop = false;
            runTime = 20;
            ig.shopRunTime[4] = runTime;
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

        (buyingPrice, max, buyAmountInt) = BM.UpdateBuyingPrice(basePrice, increment, 4);
        
        if(ig.coins < buyingPrice){
            buyButtonImage.color = new Color32(7,87,12,255);
            buyButtonText.color = Color.white;
        }else{
            buyButtonImage.color = new Color32(9,250,0,255);
            buyButtonText.color = new Color32(50,50,50,255);
        }
        numberOfShopsText.text = ig.numShops[4] + "/" + UpgradeArray[ig.shopUpgradeIndex[4]];
        buyButtonText.text = "$" + ShortScale.ParseDouble(buyingPrice, 1);
        buyAmount.text = "x" + buyAmountInt;
        string time = string.Format("{0:00}:{1:00}:{2:00}",(Math.Floor(((Math.Ceiling((double)ig.shopRunTime[4]/ig.speedval))-timer)/3600)),((Math.Floor(((Math.Ceiling((double)ig.shopRunTime[4]/ig.speedval))-timer)/60))%60),(((Math.Ceiling((double)ig.shopRunTime[4]/ig.speedval))-timer)%60));
        moneyText.text = "$" + ShortScale.ParseDouble(ig.numShops[4]*ig.shopRewards[4]*ig.profitMultiplier, 2) + " " + time;
    }

    public void startShop()
    {
        if(ig.numShops[4] > 0 && shopRuns == false){

            routine = StartCoroutine(CountDown());
        }
    }

    public void buyShop()
     {
         BM.buyShop(4, buyingPrice, max);

     }

    public bool upgradeShop(int multiplier, int index)
      {
         if(ig.coins >= shopUpgradePrice[index]){
              if(multiplier == 0){
                   autoShop = true;
                   ig.shopAutomation[4] = true;
                   if(shopRuns == false && ig.numShops[ShopIndex] > 0){
                        routine = StartCoroutine(CountDown());
                   }
              }else{
                  ig.shopRewards[ShopIndex] = ig.shopRewards[ShopIndex]*multiplier;
              }
              ig.coins -= (double)shopUpgradePrice[index];
              shopLvl++;
              ig.shopLvls[4] = shopLvl;
              return true;
         }
         return false;
      }
    

    public void prestige(){
        progressBar.value = 0;
        shopPrice = 103860;
        runTime = 20;
        ig.shopRunTime[4] = runTime;
        shopReward = 51840;
        ig.shopRewards[4] = shopReward;
        ig.numShops[ShopIndex] = 0;
        timer = 0;
        shopLvl = 0;
        ig.shopLvls[4] = shopLvl;
        autoShop = false;
        if(shopRuns == true){
            StopCoroutine(routine);
        }
        shopRuns = false;

    }
}