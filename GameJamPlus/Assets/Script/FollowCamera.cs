using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform player;  // Referência ao transform do jogador
    public float smoothSpeed = 0.125f;  // Velocidade de suavização da câmera
    public Vector3 offset;  // Distância entre a câmera e o jogador

    // Limites da câmera
    public Vector2 minLimits;  // Limite mínimo (inferior esquerdo) da câmera
    public Vector2 maxLimits;  // Limite máximo (superior direito) da câmera

    private float initialZ; // Posição inicial em Z da câmera

    void Start()
    {
        // Armazena a posição inicial em Z da câmera
        initialZ = transform.position.z;

        // Posiciona corretamente a câmera na primeira vez
        if (player != null)
        {
            Vector3 startPosition = player.position + offset;

            // Aplica os limites na posição inicial
            float clampedX = Mathf.Clamp(startPosition.x, minLimits.x, maxLimits.x);
            float clampedY = Mathf.Clamp(startPosition.y, minLimits.y, maxLimits.y);

            // Define a posição inicial da câmera, mantendo a posição inicial em Z
            transform.position = new Vector3(clampedX, clampedY, initialZ);
        }
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            // Posição desejada da câmera
            Vector3 desiredPosition = player.position + offset;

            // Suaviza a transição da posição da câmera
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Aplica os limites na posição suavizada
            float clampedX = Mathf.Clamp(smoothedPosition.x, minLimits.x, maxLimits.x);
            float clampedY = Mathf.Clamp(smoothedPosition.y, minLimits.y, maxLimits.y);

            // Atualiza a posição da câmera, mantendo o valor fixo de Z
            transform.position = new Vector3(clampedX, clampedY, initialZ);
        }
    }
}
