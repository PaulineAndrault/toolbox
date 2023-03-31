using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Axe deplacement
    private Vector2 _movement;
    private Vector2 _normalizedMovement;
    private Vector2 _clampedMovement;
    // Bouton roulade
    private bool _roll;
    private bool _rollDown;

    public Vector2 Movement { get => _movement; }
    public Vector2 NormalizedMovement { get => _normalizedMovement; }
    public Vector2 ClampedMovement { get => _clampedMovement; }
    public bool HasMovement { get => _movement.sqrMagnitude > 0f; }
    public bool Roll { get => _roll; }
    public bool RollDown { get => _rollDown; }

    private void Update()
    {
        // On stocke les valeurs brute, normalisée et clampée de l'axe de déplacement
        _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _normalizedMovement = _movement.normalized;
        _clampedMovement = Vector2.ClampMagnitude(_movement, 1f);

        // On stocke la valeur de l'input Roll
        _roll = Input.GetButton("Roll");
        // On stocke la valeur 'down' de l'input Roll
        _rollDown = Input.GetButtonDown("Roll");
    }
}
