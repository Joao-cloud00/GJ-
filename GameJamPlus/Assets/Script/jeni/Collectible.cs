using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectible : MonoBehaviour
{
    public int count;
    public Text collectText;

    void Start()
    {
        
    }

    void Update()
    {
        collectText.text = "Coletáveis: " + count.ToString();
    }
}
