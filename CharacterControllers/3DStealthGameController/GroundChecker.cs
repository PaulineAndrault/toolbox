using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedController
{
    public class GroundChecker : MonoBehaviour
    {
        //Ray settings
        [Header("Ray Settings")]
        [SerializeField]
        [Tooltip("Layer qui d�clenche le IsGrounded")]
        private LayerMask _groundLayer;

        [SerializeField]
        [Min(0f)]
        [Tooltip("Longueur du Ray qui part depuis les pieds du Player vers le sol")]

        private Vector3 _activeRay;

        // Variables priv�es
        [SerializeField] private bool _isGrounded;
        private Vector3[] _originList;
        private float _maxDistance = 1.2f;
        [SerializeField] private float _groundY;

        // Slope settings
        [Header("Slope settings")]
        [Range(0f, 0.9f)]
        [SerializeField]
        [Tooltip("Distance entre le pivot du Player et le sol en face, en de�a de laquelle on ne peut se d�placer sur ce sol")]
        private float _minGroundDistance;

        // R�f�rences
        private Transform _transform;
        private PlayerInput _playerInput;

        // Propri�t�s
        public bool IsGrounded { get => _isGrounded; }
        public float GroundY { get => _groundY; }


        private void Awake()
        {
            _transform = transform;
            _playerInput = GetComponent<PlayerInput>();
            _originList = new Vector3[5] { Vector3.zero, Vector3.right, Vector3.left, Vector3.forward, Vector3.back };

            // On initialise la valeur de la hauteur du sol
            _groundY = 0f;
        }

        private void Update()
        {
            // Est-ce qu'on est Grounded ? Cette valeur est ensuite r�cup�r�e dans le Controller puis les State Machines.
            _isGrounded = DoesAnyRayTouchGround() ? true : false;

            // A quel niveau est le sol ? Cette valeur est utilis�e dans Controller pour �tablir la hauteur du player

            // 1. On r�cup�re le Ray actif selon le d�placement actuel du Player
            GetActiveRay();
            Debug.DrawRay(_transform.position + _activeRay * 0.5f, Vector3.down * _maxDistance, Color.red);

            // 2. On v�rifie que ce Ray actif touche bien le sol et on r�cup�re sa hauteur
            if (DoesRayTouchGround(_transform.position + _activeRay * 0.5f))
            {
                SetGroundY(_transform.position + _activeRay * 0.5f);
            }
            // 3. S'il ne touche pas le sol, on garde la valeur pr�c�dente de GroundY intacte.



            // Modifier le Can Move en fonction de la taille du Ray actif
            // et c'est tout, le ground checker se contente de regarder si on touche le sol et de calculer la distance (sans cons�quence sur le jouer)
            // La state machine utilise le _is grounded du controller pour eregarder s'il doit changer de state ?

        }

        private void GetActiveRay()
        {
            _activeRay = _playerInput.Movement.Value.normalized;
        }

        // M�thode qui v�rifie si un Ray sp�cifique touche le sol
        private bool DoesRayTouchGround(Vector3 origin)
        {
            // On d�clare un hit sans assigner de valeur
            RaycastHit hit;

            // On effectue un test avec Physics.Raycast
            return Physics.Raycast(origin, Vector3.down, out hit, _maxDistance, _groundLayer);
        }


        // M�thode qui v�rifie qu'au moins un Ray touche le sol
        private bool DoesAnyRayTouchGround()
        {
            bool oneRayTouch = false;

            foreach (Vector3 origin in _originList)
            {
                if (DoesRayTouchGround(_transform.position + origin * 0.5f))
                {
                    oneRayTouch = true;
                    break;
                }
            }
            return oneRayTouch;
        }

        // M�thode qui actualise la hauteur de sol sur laquelle placer le joueur via le Controller
        private void SetGroundY(Vector3 origin)
        {
            RaycastHit hit;

            if (Physics.Raycast(origin, Vector3.down, out hit, _maxDistance, _groundLayer))
            {
                // Si le sol est suffisamment loin du pivot du player (et donc accessible pour le d�placement)
                if (hit.distance >= _minGroundDistance)
                {
                    // On d�finit une nouvelle hauteur de sol de r�f�rence
                    _groundY = hit.point.y;
                }
            }
        }


    }
}