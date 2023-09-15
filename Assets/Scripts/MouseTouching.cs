using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTouching : MonoBehaviour
{
    void Start() { }

    // Update is called once per frame
    void Update() { }

    void OnMouseOver()
    {
        transform.parent.gameObject.GetComponent<Troop>().mouseOver = true;
    }

    void OnMouseExit()
    {
        transform.parent.gameObject.GetComponent<Troop>().mouseOver = false;
    }
}
