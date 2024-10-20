using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Collectible : MonoBehaviour
{
    public int count;
    public TextMeshProUGUI collectText;

    void Update()
    {
        collectText.text = "Livros: " + count.ToString();
    }
}
