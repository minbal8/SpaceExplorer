using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipUpdateUI : MonoBehaviour
{

    public RectTransform engineProgress;

    private float progressBarInitialWidth;
    private ShipController ship;
    private PlayerController player;


    // Start is called before the first frame update
    void Start()
    {
        progressBarInitialWidth = engineProgress.rect.width;
        ship = GameManager.instance.ship;
        player = GameManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isInShip)
        {
            if (!engineProgress.parent.gameObject.activeInHierarchy)
                engineProgress.parent.gameObject.SetActive(true);

            engineProgress.sizeDelta = new Vector2((ship.currentThrust / ship.maxThrust) * progressBarInitialWidth, engineProgress.sizeDelta.y);
        }
        else
        {           
            if (engineProgress.parent.gameObject.activeInHierarchy)
                engineProgress.parent.gameObject.SetActive(false);
        }
    }
}
