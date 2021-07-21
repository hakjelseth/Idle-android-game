using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{

    public Button muteButton;
    bool music;
    AudioSource sound;
    Image muteImage;
    // Start is called before the first frame update
    void Start()
    {
        music = true;
        muteButton = GameObject.Find("muteButton").GetComponent<Button>();
        muteImage = GameObject.Find("muteButton").GetComponent<Image>();
        muteButton.onClick.AddListener(muteFunc);
        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void muteFunc(){
        if(music == true){
            GetComponent<AudioSource>().volume = 0;
            muteImage.sprite = Resources.Load<Sprite>("SoundOff_Simple_Icons_UI");

        }

    }
}
