using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PolyLabs;


public class JewelManager : MonoBehaviour
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
    public int ShopIndex = 7;
    public double shopPrice;
    public double buyingPrice;
    public double max;
    public double basePrice = 179159040;
    public double increment = 1.09;
    public bool autoShop = false;
    public bool shopRuns = false;
    public double shopReward;
    public int[] UpgradeArray = {25, 50, 100, 200, 300, 400, 500};
    double[] shopUpgradePrice = new double[]{550000000, 10000000000, (75 * Math.Pow(10, 18)), (600 * Math.Pow(10, 21)), (5 * Math.Pow(10, 30)), (1 * Math.Pow(10, 45)), (50 * Math.Pow(10, 48)), (10 * Math.Pow(10, 75))};

    IdleGame ig;
    BuyManager BM;
    Coroutine routine;
    public Slider progressBar;


    void Start()
      {

        ig = GameObject.Find("GameManager").GetComponent<IdleGame>();
        BM = GameObject.Find("BuyManager").GetComponent<BuyManager>();


        numberOfShopsText = GameObject.Find("JewelShops").GetComponent<Text>();
        buyButtonText = GameObject.Find("buyJewelText").GetComponent<Text>();
        moneyText = GameObject.Find("jewelMoneyText").GetComponent<Text>();
        buyAmount = GameObject.Find("JewelBuyAmount").GetComponent<Text>();
        buyButton = GameObject.Find("buyJewelShop").GetComponent<Button>();
        buyButtonImage = GameObject.Find("buyJewelShop").GetComponent<Image>();
        openButton = GameObject.Find("startJewel").GetComponent<Button>();
        progressBar = GameObject.Find("Jewel Progress Bar").GetComponent<Slider>();
        buyButton.onClick.AddListener(buyShop);
        openButton.onClick.AddListener(startShop);
        if(ig.saved == true){
            numberOfShops = ig.numShops[7];
            shopLvl = ig.shopLvls[7];
            shopPrice = 179159040*Math.Pow(1.09, numberOfShops);
            shopReward = ig.shopRewards[7];
            autoShop = ig.shopAutomation[7];
            runTime = ig.shopRunTime[7];
            while(ig.numShops[ShopIndex] >= UpgradeArray[ig.shopUpgradeIndex[ShopIndex]]){
                ig.shopUpgradeIndex[ShopIndex]++;
            }
        }else{
            ig.numShops[ShopIndex] = 0;
            shopPrice = 179159040;
            shopReward = 89579520;
            ig.shopRewards[7] = shopReward;
            autoShop = false;
            runTime = 1500;
            ig.shopRunTime[7] = runTime;
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
       if(autoShop == true && numberOfShops > 0){
            routine = StartCoroutine(CountDown());
        }else{
            shopRuns = false;
        }
    }

    public void updateText(){
	    double buyAmountInt = ig.buyMultiplier;

	    (buyingPrice, max, buyAmountInt) = BM.UpdateBuyingPrice(basePrice, increment, 7);

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
        string time = string.Format("{0:00}:{1:00}:{2:00}",(Math.Floor(((Math.Ceiling((double)ig.shopRunTime[7]/ig.speedval))-timer)/3600)),((Math.Floor(((Math.Ceiling((double)ig.shopRunTime[7]/ig.speedval))-timer)/60))%60),(((Math.Ceiling((double)ig.shopRunTime[7]/ig.speedval))-timer)%60));
        moneyText.text = "$" + ShortScale.ParseDouble(ig.numShops[7]*ig.shopRewards[7]*ig.profitMultiplier, 2) + " " + time;
    }

    public void startShop()
    {
        if(ig.numShops[7] > 0 && shopRuns == false){

            routine = StartCoroutine(CountDown());
        }
    }

      public void buyShop()
              {
	              BM.buyShop(7, buyingPrice, max);

              }

    public bool upgradeShop(int multiplier, int index)
      {
         if(ig.coins >= shopUpgradePrice[index]){
              if(multiplier == 0){
                   autoShop = true;
                   ig.shopAutomation[7] = true;
                   if(shopRuns == false && ig.numShops[ShopIndex] > 0){
                        routine = StartCoroutine(CountDown());
                   }
              }else{
                  ig.shopRewards[ShopIndex] = ig.shopRewards[ShopIndex]*multiplier;
              }
              ig.coins -= (double)shopUpgradePrice[index];
              shopLvl++;
              ig.shopLvls[7] = shopLvl;
              return true;
         }
         return false;
      }
    
    public void prestige(){
        progressBar.value = 0;
        shopPrice = 179159040;
        runTime = 1500;
        ig.shopRunTime[7] = runTime;
        shopReward = 89579520;
        ig.shopRewards[7] = shopReward;
        ig.numShops[ShopIndex] = 0;
        shopLvl = 0;
        ig.shopLvls[7] = shopLvl;
        timer = 0;
		upgradeIndex = 0;
        autoShop = false;
        if(shopRuns == true){
            StopCoroutine(routine);
        }
        shopRuns = false;

    }
}