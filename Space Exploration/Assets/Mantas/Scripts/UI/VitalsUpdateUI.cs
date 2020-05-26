using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitalsUpdateUI : MonoBehaviour
{
    public PlayerController player;

    public RectTransform healthProgress;
    public RectTransform oxygenProgress;
    public RectTransform foodProgress;

    private float progressBarInitialWidth;

    void Start()
    {
        progressBarInitialWidth = healthProgress.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        healthProgress.sizeDelta = new Vector2((player.health / player.healthMax) * progressBarInitialWidth, healthProgress.sizeDelta.y);
        oxygenProgress.sizeDelta = new Vector2( (player.oxygen / player.oxygenMax) * progressBarInitialWidth, oxygenProgress.sizeDelta.y);
        foodProgress.sizeDelta = new Vector2((player.food / player.foodMax) * progressBarInitialWidth, foodProgress.sizeDelta.y);
    }
}
