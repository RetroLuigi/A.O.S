using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TroopSpawnerUI : MonoBehaviour
{
    public int price;
    public TextMeshProUGUI pText;

    void Start()
    {
        pText.text = price.ToString();
    }

    public void Click()
    {
        print("clicked");
    }

    // Update is called once per frame
    void Update() { }
}
