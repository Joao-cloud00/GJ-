using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placa : MonoBehaviour
{
    public GameObject placa;
    public GameObject placa1;
    public GameObject placa2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Placa"))
        {
            placa.SetActive(true);
        }

        if (other.gameObject.CompareTag("Placa1"))
        {
            placa1.SetActive(true);
        }

        if (other.gameObject.CompareTag("Placa2"))
        {
            placa2.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Placa"))
        {
            placa.SetActive(false);
        }

        if (other.gameObject.CompareTag("Placa1"))
        {
            placa1.SetActive(false);
        }

        if (other.gameObject.CompareTag("Placa2"))
        {
            placa2.SetActive(false);
        }
    }
}
