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
        [Tooltip("Layer qui déclenche le IsGrounded")]
        private LayerMask _groundLayer;

        [SerializeField]
        [Min(0f)]
        [Tooltip("Longueur du Ray qui part depuis les pieds du Player vers le sol")]

        private Vector3 _activeRay;

        // Variables privées
        [SerializeField] private bool _isGrounded;
        private Vector3[] _originList;
        private float _maxDistance = 1.2f;
        [SerializeField] private float _groundY;

        // Slope settings
        [Header("Slope settings")]
        [Range(0f, 0.9f)]
        [SerializeField]
        [Tooltip("Distance entre le pivot du Player et le sol en face, en deça de laquelle on ne peut se déplacer sur ce sol")]
        private float _minGroundDistance;

        // Références
        private Transform _transform;
        private PlayerInput _playerInput;

        // Propriétés
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
            // Est-ce qu'on est Grounded ? Cette valeur est ensuite récupérée dans le Controller puis les State Machines.
            _isGrounded = DoesAnyRayTouchGround() ? true : false;

            // A quel niveau est le sol ? Cette valeur est utilisée dans Controller pour établir la hauteur du player

            // 1. On récupère le Ray actif selon le déplacement actuel du Player
            GetActiveRay();
            Debug.DrawRay(_transform.position + _activeRay * 0.5f, Vector3.down * _maxDistance, Color.red);

            // 2. On vérifie que ce Ray actif touche bien le sol et on récupère sa hauteur
            if (DoesRayTouchGround(_transform.position + _activeRay * 0.5f))
            {
                SetGroundY(_transform.position + _activeRay * 0.5f);
            }
            // 3. S'il ne touche pas le sol, on garde la valeur précédente de GroundY intacte.



            // Modifier le Can Move en fonction de la taille du Ray actif
            // et c'est tout, le ground checker se contente de regarder si on touche le sol et de calculer la distance (sans conséquence sur le jouer)
            // La state machine utilise le _is grounded du controller pour eregarder s'il doit changer de state ?

        }

        private void GetActiveRay()
        {
            _activeRay = _playerInput.Movement.Value.normalized;
        }

        // Méthode qui vérifie si un Ray spécifique touche le sol
        private bool DoesRayTouchGround(Vector3 origin)
        {
            // On déclare un hit sans assigner de valeur
            RaycastHit hit;

            // On effectue un test avec Physics.Raycast
            return Physics.Raycast(origin, Vector3.down, out hit, _maxDistance, _groundLayer);
        }


        // Méthode qui vérifie qu'au moins un Ray touche le sol
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

        // Méthode qui actualise la hauteur de sol sur laquelle placer le joueur via le Controller
        private void SetGroundY(Vector3 origin)
        {
            RaycastHit hit;

            if (Physics.Raycast(origin, Vector3.down, out hit, _maxDistance, _groundLayer))
            {
                // Si le sol est suffisamment loin du pivot du player (et donc accessible pour le déplacement)
                if (hit.distance >= _minGroundDistance)
                {
                    // On définit une nouvelle hauteur de sol de référence
                    _groundY = hit.point.y;
                }
            }
        }


    }
}