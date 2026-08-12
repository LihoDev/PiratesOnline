using Mirror;
using PiratesOnline.Domain.Service;
using PiratesOnline.Infrastructure.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace PiratesOnline.Presentation.Player
{
    public class PlayerMovement : NetworkBehaviour
    {
        private IMapService _mapService;
        private PlayerShipController _shipController;

        [SyncVar] private Vector2 _targetPosition;
        [SyncVar] private bool _isMoving;

        private Camera _mainCam;

        [Inject]
        public void Construct(IMapService mapService)
        {
            _mapService = mapService;
        }

        private void Awake()
        {
            ProjectContext.Instance.Container.Inject(this);
            _shipController = GetComponent<PlayerShipController>();
            _mainCam = Camera.main;
        }

        public override void OnStartLocalPlayer()
        {
            InputManager.Instance.Actions.Player.Attack.performed += HandleInput;
        }

        private void OnDisable()
        {
            if (isLocalPlayer)
                InputManager.Instance.Actions.Player.Attack.performed -= HandleInput;
        }

        private void Update()
        {
            if (isServer)
            {
                MoveShip();
            }
        }

        [Client]
        private void HandleInput(InputAction.CallbackContext context)
        {
            Vector2 mousePos = _mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            CmdMoveTo(mousePos);
        }

        [Command]
        private void CmdMoveTo(Vector2 position)
        {
            _targetPosition = position;
            _isMoving = true;
        }

        [Server]
        private void MoveShip()
        {
            if (!_isMoving) return;

            float distance = Vector2.Distance(transform.position, _targetPosition);
            if (distance < 0.1f)
            {
                _isMoving = false;
                return;
            }

            float baseSpeed = _shipController.Stats.Speed;
            float biomeMultiplier = _mapService.GetSpeedMultiplier(transform.position);
            float mastBonus = 1f + (_shipController.Stats.MastsCount * 0.1f);
            float currentSpeed = baseSpeed * biomeMultiplier * mastBonus;
            transform.position = Vector2.MoveTowards(transform.position, _targetPosition, currentSpeed * Time.deltaTime);
            Vector2 direction = _targetPosition - (Vector2)transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    }
}