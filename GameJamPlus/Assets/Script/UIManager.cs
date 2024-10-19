using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject Opcoes;
    [SerializeField]
    GameObject Creditos;


    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void ComecarJogo()
    {
        SceneManager.LoadScene("Fase1");
    }

    public void AbrirOpcoes()
    {
        Opcoes.SetActive(true);
    }

    public void FecharOpcoes()
    {
        Opcoes.SetActive(false);
    } 
    
    public void AbrirCreditos()
    {
        Creditos.SetActive(true);
        Opcoes.SetActive(false);
    }    
    
    public void FecharCreditos()
    {
        Creditos.SetActive(false);
        Opcoes.SetActive(true);
    }



    public void FecharJogo()
    {
        Application.Quit();
    }
}
