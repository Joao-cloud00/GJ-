using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform player;  // Refer�ncia ao transform do jogador
    public float smoothSpeed = 0.125f;  // Velocidade de suaviza��o da c�mera
    public Vector3 offset;  // Dist�ncia entre a c�mera e o jogador

    // Limites da c�mera
    public Vector2 minLimits;  // Limite m�nimo (inferior esquerdo) da c�mera
    public Vector2 maxLimits;  // Limite m�ximo (superior direito) da c�mera

    private float initialZ; // Posi��o inicial em Z da c�mera

    void Start()
    {
        // Armazena a posi��o inicial em Z da c�mera
        initialZ = transform.position.z;

        // Posiciona corretamente a c�mera na primeira vez
        if (player != null)
        {
            Vector3 startPosition = player.position + offset;

            // Aplica os limites na posi��o inicial
            float clampedX = Mathf.Clamp(startPosition.x, minLimits.x, maxLimits.x);
            float clampedY = Mathf.Clamp(startPosition.y, minLimits.y, maxLimits.y);

            // Define a posi��o inicial da c�mera, mantendo a posi��o inicial em Z
            transform.position = new Vector3(clampedX, clampedY, initialZ);
        }
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            // Posi��o desejada da c�mera
            Vector3 desiredPosition = player.position + offset;

            // Suaviza a transi��o da posi��o da c�mera
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Aplica os limites na posi��o suavizada
            float clampedX = Mathf.Clamp(smoothedPosition.x, minLimits.x, maxLimits.x);
            float clampedY = Mathf.Clamp(smoothedPosition.y, minLimits.y, maxLimits.y);

            // Atualiza a posi��o da c�mera, mantendo o valor fixo de Z
            transform.position = new Vector3(clampedX, clampedY, initialZ);
        }
    }
}
