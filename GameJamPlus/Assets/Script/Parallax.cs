using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public GameObject cam;          // Refer�ncia � c�mera principal
    public GameObject[] backgrounds; // Array com os objetos de fundo (as duas imagens)
    public float parallaxSpeed;     // Velocidade do efeito parallax
    private float backgroundWidth;  // Largura da imagem de fundo (tamanho da imagem)
    private Vector3 lastCamPos;     // Posi��o anterior da c�mera

    void Start()
    {
        // Definimos a posi��o inicial da c�mera
        lastCamPos = cam.transform.position;

        // Calcula a largura do background com base na dist�ncia entre os dois objetos
        backgroundWidth = Vector3.Distance(backgrounds[0].transform.position, backgrounds[1].transform.position);
    }

    void Update()
    {
        // Calcula o movimento da c�mera
        float deltaX = cam.transform.position.x - lastCamPos.x;

        // Aplica o efeito de parallax ao mover o fundo mais devagar que a c�mera
        transform.position += Vector3.right * (deltaX * parallaxSpeed);

        // Atualiza a posi��o da c�mera para o pr�ximo frame
        lastCamPos = cam.transform.position;

        // Reposiciona as imagens quando a c�mera passa por uma delas, tanto para frente quanto para tr�s
        foreach (GameObject background in backgrounds)
        {
            // Se a c�mera passou da imagem para a direita
            if (cam.transform.position.x - background.transform.position.x >= backgroundWidth)
            {
                Vector3 newPos = background.transform.position;
                newPos.x += backgroundWidth * backgrounds.Length;  // Reposiciona para a frente
                background.transform.position = newPos;
            }
            // Se a c�mera passou da imagem para a esquerda
            else if (background.transform.position.x - cam.transform.position.x >= backgroundWidth)
            {
                Vector3 newPos = background.transform.position;
                newPos.x -= backgroundWidth * backgrounds.Length;  // Reposiciona para tr�s
                background.transform.position = newPos;
            }
        }
    }
}



