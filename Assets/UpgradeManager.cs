using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeManager : MonoBehaviour
{
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
    GameObject upgradeMenu;
    IdleGame ig;
    Button upgradeButton;
    ArrayList hiddenUpgrades = new ArrayList();
    public bool upgMenuShowing;
    bool forsteGang;
    int allUpgrades;
    int comicLvl;
    int mediaLvl;
    int sushiLvl;
    int vincyLvl;
    int shoeLvl;
    int majorkeyLvl;
    int pineappleLvl;
    int jewelsLvl;
    int donLvl;
    int galacticLvl;

    double[] allUpgradePrice = new double[]{(1 * Math.Pow(10, 12)), (50 * Math.Pow(10, 15)), (500 * Math.Pow(10, 18)), (900 * Math.Pow(10, 21)), (1 * Math.Pow(10, 42)), (100 * Math.Pow(10, 45)), (1 * Math.Pow(10, 51)), (1 * Math.Pow(10, 54)), (1 * Math.Pow(10, 60)), (1 * Math.Pow(10, 66)), (1 * Math.Pow(10, 72)), (1 * Math.Pow(10, 75)), (1 * Math.Pow(10, 78))};


    // Start is called before the first frame update
    void Start()
    {
          ig = GameObject.Find("GameManager").GetComponent<IdleGame>();

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
          upgradeMenu = GameObject.Find("upgradeMenu");
          upgradeButton = GameObject.Find("upgradeButton").GetComponent<Button>();
          upgMenuShowing = false;
          upgradeMenu.SetActive(false);
          upgradeButton.onClick.AddListener(clickedUpgrade);
          forsteGang = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void clickedUpgrade()
    {
        if (upgMenuShowing == false){
            upgradeMenu.SetActive(true);
            upgMenuShowing = true;
            ig.prestigeManager.prestigeMenuShowing = false;
            ig.prestigeBackground.SetActive(false);
            if(forsteGang == true){
                foreach (string btnName in ig.oldUpgrades){
                    //GameObject BtnParent = GameObject.Find("")
                    GameObject thisBtn = GameObject.Find(btnName);
                    thisBtn.SetActive(false);
                }
                forsteGang = false;
            }

        }else{
            upgradeMenu.SetActive(false);
            upgMenuShowing = false;
        }
    }

       public void UpgradeComic(int multiplier)
          {
              if(cm.upgradeShop(multiplier, comicLvl) == true){
                      if(ig.forsteGang == false){
                          string btnName = EventSystem.current.currentSelectedGameObject.name;
                          GameObject thisBtn = GameObject.Find(btnName);
                          ig.oldUpgrades.Add(btnName);
                          hiddenUpgrades.Add(thisBtn);
                          thisBtn.SetActive(false);
                      }

                 }
          }

       public void setComicLvl(int lvl)
       {
          comicLvl = lvl;
       }

       public void UpgradeMedia(int multiplier)
          {
            if(mm.upgradeShop(multiplier, mediaLvl) == true){
                   string btnName = EventSystem.current.currentSelectedGameObject.name;
                   GameObject thisBtn = GameObject.Find(btnName);
                   ig.oldUpgrades.Add(btnName);
                   hiddenUpgrades.Add(thisBtn);
                   thisBtn.SetActive(false);
              }
          }

       public void setMediaLvl(int lvl)
       {
          mediaLvl = lvl;
       }

       public void UpgradeSushi(int multiplier)
          {
           if(sm.upgradeShop(multiplier, sushiLvl) == true){
                   string btnName = EventSystem.current.currentSelectedGameObject.name;
                   GameObject thisBtn = GameObject.Find(btnName);
                   ig.oldUpgrades.Add(btnName);
                   hiddenUpgrades.Add(thisBtn);
                   thisBtn.SetActive(false);
                  }
          }

       public void setSushiLvl(int lvl)
       {
          sushiLvl = lvl;
       }

       public void UpgradeVincy(int multiplier)
          {
           if(vm.upgradeShop(multiplier, vincyLvl) == true){
                  string btnName = EventSystem.current.currentSelectedGameObject.name;
                  GameObject thisBtn = GameObject.Find(btnName);
                  ig.oldUpgrades.Add(btnName);
                  hiddenUpgrades.Add(thisBtn);
                  thisBtn.SetActive(false);
               }
          }

       public void setVincyLvl(int lvl)
       {
          vincyLvl = lvl;
       }

       public void UpgradeShoe(int multiplier)
          {
              if(som.upgradeShop(multiplier, shoeLvl) == true){
                    string btnName = EventSystem.current.currentSelectedGameObject.name;
                    GameObject thisBtn = GameObject.Find(btnName);
                    ig.oldUpgrades.Add(btnName);
                    hiddenUpgrades.Add(thisBtn);
                    thisBtn.SetActive(false);
              }
          }

       public void setShoeLvl(int lvl)
       {
          shoeLvl = lvl;
       }

       public void UpgradeMajorkey(int multiplier)
          {
               if(mkm.upgradeShop(multiplier, majorkeyLvl) == true){
                        string btnName = EventSystem.current.currentSelectedGameObject.name;
                        GameObject thisBtn = GameObject.Find(btnName);
                        ig.oldUpgrades.Add(btnName);
                        hiddenUpgrades.Add(thisBtn);
                        thisBtn.SetActive(false);
                    }

          }

       public void setMajorkeyLvl(int lvl)
       {
          majorkeyLvl = lvl;
       }

       public void UpgradePineapple(int multiplier)
          {
             if(pm.upgradeShop(multiplier, pineappleLvl) == true){
                       string btnName = EventSystem.current.currentSelectedGameObject.name;
                       GameObject thisBtn = GameObject.Find(btnName);
                       ig.oldUpgrades.Add(btnName);
                       hiddenUpgrades.Add(thisBtn);
                       thisBtn.SetActive(false);
                   }
          }

       public void setPineappleLvl(int lvl)
       {
          pineappleLvl = lvl;
       }

       public void UpgradeJewels(int multiplier)
          {
                if(jm.upgradeShop(multiplier, jewelsLvl) == true){
                          string btnName = EventSystem.current.currentSelectedGameObject.name;
                          GameObject thisBtn = GameObject.Find(btnName);
                          ig.oldUpgrades.Add(btnName);
                          hiddenUpgrades.Add(thisBtn);
                          thisBtn.SetActive(false);
                    }

          }

       public void setJewelsLvl(int lvl)
       {
          jewelsLvl = lvl;
       }

       public void UpgradeDon(int multiplier)
          {
            if(dm.upgradeShop(multiplier, donLvl) == true){
                     string btnName = EventSystem.current.currentSelectedGameObject.name;
                     GameObject thisBtn = GameObject.Find(btnName);
                     ig.oldUpgrades.Add(btnName);
                     hiddenUpgrades.Add(thisBtn);
                     thisBtn.SetActive(false);
               }
          }

       public void setDonLvl(int lvl)
       {
          donLvl = lvl;
       }

       public void UpgradeGalactic(int multiplier)
          {
              if(gm.upgradeShop(multiplier, galacticLvl) == true){
                    string btnName = EventSystem.current.currentSelectedGameObject.name;
                    GameObject thisBtn = GameObject.Find(btnName);
                    ig.oldUpgrades.Add(btnName);
                    hiddenUpgrades.Add(thisBtn);
                    thisBtn.SetActive(false);
              }


          }

       public void setGalacticLvl(int lvl)
       {
          galacticLvl = lvl;
       }

       public void UpgradeAll(int multiplier)
       {
           if(ig.coins >= allUpgradePrice[allUpgrades]){
               string btnName = EventSystem.current.currentSelectedGameObject.name;
               GameObject thisBtn = GameObject.Find(btnName);
               thisBtn.SetActive(false);
               ig.oldUpgrades.Add(btnName);
               hiddenUpgrades.Add(thisBtn);
               ig.coins -= allUpgradePrice[allUpgrades];
               ig.profitMultiplier = ig.profitMultiplier*multiplier;
               allUpgrades++;
           }

       }

       public void setAllLvl(int allLvl)
       {
          allUpgrades = allLvl;
       }

       public void prestige(){
            foreach (GameObject thisBtn in hiddenUpgrades){
                thisBtn.SetActive(true);
            }
            hiddenUpgrades.Clear();
            ig.oldUpgrades.Clear();

      }
}
