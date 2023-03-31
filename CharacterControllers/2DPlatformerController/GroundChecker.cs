using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private Transform _topLeft;
    [SerializeField] private Transform _botRight;
    [SerializeField] private LayerMask _layerMask;

    public bool IsGrounded()
    {
        // On récupère une collision éventuelle avec Overlap
        Collider2D collider = Physics2D.OverlapArea(_topLeft.position, _botRight.position, _layerMask);

        // On retourne s'il y a une collision avec un sol.
        return collider != null;
    }

    private void OnDrawGizmosSelected()
    {
        // Dessiner les 4 lignes de l'area checkée
        Debug.DrawLine(_topLeft.position, new Vector2(_botRight.position.x, _topLeft.position.y), Color.red);
        Debug.DrawLine(new Vector2(_botRight.position.x, _topLeft.position.y), _botRight.position, Color.red);
        Debug.DrawLine(_botRight.position, new Vector2(_topLeft.position.x, _botRight.position.y), Color.red);
        Debug.DrawLine(new Vector2(_topLeft.position.x, _botRight.position.y), _topLeft.position, Color.red);
    }
}
