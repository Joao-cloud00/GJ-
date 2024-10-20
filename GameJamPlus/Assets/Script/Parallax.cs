using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public GameObject cam;          // Referência à câmera principal
    public GameObject[] backgrounds; // Array com os objetos de fundo (as duas imagens)
    public float parallaxSpeed;     // Velocidade do efeito parallax
    private float backgroundWidth;  // Largura da imagem de fundo (tamanho da imagem)
    private Vector3 lastCamPos;     // Posição anterior da câmera

    void Start()
    {
        // Definimos a posição inicial da câmera
        lastCamPos = cam.transform.position;

        // Calcula a largura do background com base na distância entre os dois objetos
        backgroundWidth = Vector3.Distance(backgrounds[0].transform.position, backgrounds[1].transform.position);
    }

    void Update()
    {
        // Calcula o movimento da câmera
        float deltaX = cam.transform.position.x - lastCamPos.x;

        // Aplica o efeito de parallax ao mover o fundo mais devagar que a câmera
        transform.position += Vector3.right * (deltaX * parallaxSpeed);

        // Atualiza a posição da câmera para o próximo frame
        lastCamPos = cam.transform.position;

        // Reposiciona as imagens quando a câmera passa por uma delas, tanto para frente quanto para trás
        foreach (GameObject background in backgrounds)
        {
            // Se a câmera passou da imagem para a direita
            if (cam.transform.position.x - background.transform.position.x >= backgroundWidth)
            {
                Vector3 newPos = background.transform.position;
                newPos.x += backgroundWidth * backgrounds.Length;  // Reposiciona para a frente
                background.transform.position = newPos;
            }
            // Se a câmera passou da imagem para a esquerda
            else if (background.transform.position.x - cam.transform.position.x >= backgroundWidth)
            {
                Vector3 newPos = background.transform.position;
                newPos.x -= backgroundWidth * backgrounds.Length;  // Reposiciona para trás
                background.transform.position = newPos;
            }
        }
    }
}



