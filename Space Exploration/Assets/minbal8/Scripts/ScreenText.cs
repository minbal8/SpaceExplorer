using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenText : MonoBehaviour
{
    public Text countText;
    public int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        countText.text = "No items picked up";
    }

    public void AddCount()
    {
        count++;
        countText.text = "Picked up :"+count+"Items";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
