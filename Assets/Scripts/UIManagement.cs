using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManagement : MonoBehaviour{

    public List<Sprite> images = new List<Sprite>();
    public Image troopIconImage;
    public int troopID;
    public bool mouseOverTroop;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackPowerText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI attackSpeedText;

    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        if(mouseOverTroop){
            troopIconImage.sprite = images[troopID];
        } else {
            troopIconImage.sprite = null;
        }
    }
}
