using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyManager : MonoBehaviour
{
	IdleGame ig;
	public int[] UpgradeArray = {25, 50, 100, 200, 300, 400, 500};

    // Start is called before the first frame update
    void Start()
    {
	    ig = GameObject.Find("GameManager").GetComponent<IdleGame>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    public void buyShop(int ShopIndex, double BuyingPrice, double max){

            if(ig.coins >= BuyingPrice){
              if (max > 0)
              {
	              ig.numShops[ShopIndex] = ig.numShops[ShopIndex] + (int)max;
              }
              else if (ig.buyMultiplier == 2)
              {
	              ig.numShops[ShopIndex] += UpgradeArray[ig.shopUpgradeIndex[ShopIndex]] - ig.numShops[ShopIndex];
              }
              else
              {
	              ig.numShops[ShopIndex] = ig.numShops[ShopIndex] + ig.buyMultiplier;
              }
                   ig.coins -= BuyingPrice;

                   if(ig.numShops.Min() >= ig.speedMultipliers[ig.speedIndex]){
                       ig.speedval = ig.speedval * 2;
                       ig.speedIndex++;
                   }

               while(ig.numShops[ShopIndex] >= UpgradeArray[ig.shopUpgradeIndex[ShopIndex]]){
                  if(ig.numShops[ShopIndex] >= 400){
                        ig.shopRunTime[ShopIndex] = ig.shopRunTime[ShopIndex]/2;
                  }else{
                      ig.shopRewards[ShopIndex] = ig.shopRewards[ShopIndex]*2;
                  }
				ig.shopUpgradeIndex[ShopIndex]++;
           	}
			ig.UpdateAllText();
           }
        }
	
	public (double, double, double) UpdateBuyingPrice(double BasePrice, double Increment, int ShopIndex){
		double buyingPrice;
		double max = 0;
		double buyAmountInt;
	
		if(ig.buyMultiplier == 10 || ig.buyMultiplier == 100){
			buyingPrice = BasePrice*((Math.Pow(Increment, ig.numShops[ShopIndex])*(Math.Pow(Increment, ig.buyMultiplier) - 1))/ (Increment - 1));
			buyAmountInt = ig.buyMultiplier;
		}
		else if (ig.buyMultiplier == 2)
		{
			buyingPrice = BasePrice*((Math.Pow(Increment, ig.numShops[ShopIndex])*(Math.Pow(Increment, (UpgradeArray[ig.shopUpgradeIndex[ShopIndex]] - ig.numShops[ShopIndex])) - 1))/ (Increment - 1));
			buyAmountInt = UpgradeArray[ig.shopUpgradeIndex[ShopIndex]] - ig.numShops[ShopIndex];
		}
		else if (ig.buyMultiplier == 3)
		{
       		max = Math.Floor((Math.Log(((ig.coins*(Increment-1))/(BasePrice*(Math.Pow(Increment, ig.numShops[ShopIndex])))) + 1)/(Math.Log(Increment))));
			if (max > 0)
			{
				buyingPrice = BasePrice*((Math.Pow(Increment, ig.numShops[ShopIndex])*(Math.Pow(Increment, max) - 1))/ (Increment - 1));
				buyAmountInt = max;	
			} else {
				buyingPrice = BasePrice*(Math.Pow(Increment,ig.numShops[ShopIndex]));
				buyAmountInt = 1;		
			}
		}
		else{
			buyingPrice = BasePrice*(Math.Pow(Increment,ig.numShops[ShopIndex]));
			buyAmountInt = 1;
            if(ig.numShops[ShopIndex] == 0){
                buyingPrice = BasePrice;
            }
		}


		
		return (buyingPrice, max, buyAmountInt);
	}
}
