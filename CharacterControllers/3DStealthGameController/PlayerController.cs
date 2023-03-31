using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedController
{

    public class PlayerController : MonoBehaviour
    {
        //Reference settings
        [SerializeField] private PlayerInput _input;
        [SerializeField] private PlayerMovementStateMachine _moveStateMachine;
        [SerializeField] private PlayerAirStateMachine _airStateMachine;
        [SerializeField] private GroundChecker _groundChecker;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Transform _tr;
        [SerializeField] private Transform _cam;

        //Speed settings
        [Header("Speed Settings")]
        [Min(0f)]
        [SerializeField]
        [Tooltip("Vitesse de marche")]
        private float _speed = 5f;
        [Min(0f)]
        [SerializeField]
        [Tooltip("Vitesse de course")]
        private float _sprintSpeed = 8f;
        [Min(0f)]
        [SerializeField]
        [Tooltip("Vitesse accroupi")]
        private float _crouchSpeed = 2.5f;
        [Min(0f)]
        [SerializeField]
        [Tooltip("Vitesse de rotation (degré/sec)")]
        private float _rotationSpeed = 1f;

        // Jump settings
        [Header("Jump settings")]
        [Min(0f)]
        [SerializeField]
        [Tooltip("Force du saut")]
        private float _jumpHeight = 2f;
        private bool _isGrounded;
        [Min(0f)]
        [SerializeField]
        [Tooltip("Durée post Jump avant de pouvoir rechecker le sol")]
        private float _groundCheckDisableAfterJumpDuration;
        private float _groundCheckDisableAterJumpEndTime;


        // Move settings
        [Header("Move and Slope settings")]
        [Min(0f)]
        [SerializeField]
        [Tooltip("Distance entre le pivot du Player et le sol en face, en deça de laquelle on ne peut se déplacer sur ce sol")]
        private float _minSlopeDistance;


        // Propriétés publiques
        public bool IsGrounded { get => _isGrounded; }

        private void Awake()
        {
            // On confine la souris à l'intérieur de la fenêtre de jeu
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void Update()
        {
            DetectGround();
            //ApplyGravityOnAir();
        }

        private void FixedUpdate()
        {

            if (_airStateMachine.CurrentState == AirState.GROUNDED)
            {
                ResetHeight();
            }

            // Ce sera dans la State Machine plus tard
            if (CanMoveOrRotate())
            {
                ApplyMovement();
                ApplyRotation();
            }
        }

        private bool CanMoveOrRotate()
        {
            // A compléter plus tard si besoin
            // Exemple : pendant la visée ?

            bool canMove = true;

            // Normalement, pas besoin : c'est le taf du Collider (varier sa taille pour toucher ou non une hauteur de marche)
            //if(_groundChecker.GroundY <= _minSlopeDistance)
            //{
            //    canMove = false;
            //}

            return canMove;
        }

        private void ApplyMovement()
        {
            float currentSpeed;

            // On choisit la vitesse de mouvement selon le Current State de la Movement SM
            switch (_moveStateMachine.CurrentState)
            {
                case MovementState.IDLE:
                    currentSpeed = 0f;
                    break;
                case MovementState.WALK:
                    currentSpeed = _speed;
                    break;
                case MovementState.RUN:
                    currentSpeed = _sprintSpeed;
                    break;
                case MovementState.CROUCH:
                    currentSpeed = _crouchSpeed;
                    break;
                default:
                    currentSpeed = _speed;
                    break;
            }

            _rb.velocity = new Vector3(_input.Movement.Value.x * currentSpeed, _rb.velocity.y, _input.Movement.Value.z * currentSpeed);
        }

        private void ApplyRotation()
        {
            // On ne tourne que si l'on est en déplacement
            if (_input.Movement.HasMovement)
            {
                Quaternion angleCam = Quaternion.LookRotation(new Vector3(_cam.forward.x, 0, _cam.forward.z));
                Quaternion rotation = Quaternion.RotateTowards(_rb.rotation, angleCam, _rotationSpeed);
                _rb.MoveRotation(rotation);
            }
        }

        // A appeler quand on repasse en Idle en fin de mouvement, pour éviter les glissades.
        public void ResetHorizontalVelocity()
        {
            _rb.velocity = new Vector3(Vector3.zero.x, _rb.velocity.y, Vector3.zero.z);
        }

        public void ResetVerticalVelocity()
        {
            _rb.velocity = new Vector3(_rb.velocity.x, Vector3.zero.y, _rb.velocity.z);
        }
        public void ResetHeight()
        {
            _rb.position = new Vector3(_rb.position.x, _groundChecker.GroundY + 1f, _rb.position.z);
        }

        public void Jump()
        {
            // On met un petit timer avant de pouvoir redétecter le sol
            _groundCheckDisableAterJumpEndTime = Time.time + _groundCheckDisableAfterJumpDuration;

            // On désactive manuellement _isGrounded (car on n'écoute pas le Ground Checker le temps du timer)
            _isGrounded = false;

            // Pour utiliser une HAUTEUR de saut plutôt qu'une FORCE de saut, il faut multiplier la force par le double de la gravité pour la contrebalancer
            _rb.AddForce(Vector3.up * Mathf.Sqrt(_jumpHeight * 2 * Mathf.Abs(Physics.gravity.y)), ForceMode.VelocityChange);
        }

        private void DetectGround()
        {
            // regarde le _isGround du check ground uniquement si le timer du jump est passé
            if (Time.time >= _groundCheckDisableAterJumpEndTime)
            {
                _isGrounded = _groundChecker.IsGrounded;

            }
        }

        //private void ApplyGravityOnAir()
        //{
        //    // On désactive la gravité si on est au sol
        //    _rb.useGravity = _isGrounded ? false : true;
        //}


    }

}
