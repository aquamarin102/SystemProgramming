using Main;
using Mechanics;
using Mirror;
using Network;
using TMPro;
using UI;
using UnityEngine;

namespace Characters
{
    public class ShipController : NetworkBehaviour
    {
        public string PlayerName
        {
            get => _playerName;
            set => _playerName = value;
        }

        [SerializeField] private Transform _cameraAttach;
        private CameraOrbit _cameraOrbit;
        private PlayerLabel _playerLabel;
        private float _shipSpeed;
        private Rigidbody _rb;
        [SyncVar] private string _playerName;

        private NetworkTransform _networkTransform;
        private void OnGUI()
        {
            if (!isOwned) return;

            if (_cameraOrbit == null)
            {
                return;
            }
            _cameraOrbit.ShowPlayerLabels(_playerLabel);
        }
        public override void OnStartAuthority()
        {
            _rb = GetComponent<Rigidbody>();
            if (_rb == null)
            {
                return;
            }
            gameObject.name = _playerName;
            _cameraOrbit = FindObjectOfType<CameraOrbit>();
            _cameraOrbit.Initiate(_cameraAttach == null ? transform :
            _cameraAttach);
            _playerLabel = GetComponentInChildren<PlayerLabel>();
            //Initiate();
            TMP_InputField inputFieldPlayerName = FindObjectOfType<InputFieldPlayerName>().InputField;
            inputFieldPlayerName.onValueChanged.AddListener(ChangePlayerName);
            ChangePlayerName(inputFieldPlayerName.text);

            _networkTransform = GetComponent<NetworkTransform>();

        }

        private void ChangePlayerName(string value)
        {
            gameObject.name = value;
            CmdChangePlayerName(value);
        }

        [Command]
        private void CmdChangePlayerName(string value)
        {
            _playerName = value;
        }

        private void Update()
        {
            if (isOwned)
            {
                HasAuthorityMovement();
            }
            else
            {
                FromServerUpdate();
            }
        }

        private void HasAuthorityMovement()
        {
            var spaceShipSettings =
            SettingsContainer.Instance?.SpaceShipSettings;
            if (spaceShipSettings == null)
            {
                return;
            }
            var isFaster = Input.GetKey(KeyCode.LeftShift);
            var speed = spaceShipSettings.ShipSpeed;
            var faster = isFaster ? spaceShipSettings.Faster : 1.0f;
            _shipSpeed = Mathf.Lerp(_shipSpeed, speed * faster,
            SettingsContainer.Instance.SpaceShipSettings.Acceleration);
            var currentFov = isFaster
            ? SettingsContainer.Instance.SpaceShipSettings.FasterFov
        : SettingsContainer.Instance.SpaceShipSettings.NormalFov;
            _cameraOrbit.SetFov(currentFov,
            SettingsContainer.Instance.SpaceShipSettings.ChangeFovSpeed);
            var velocity =
            _cameraOrbit.transform.TransformDirection(Vector3.forward) * _shipSpeed;
            _rb.velocity = velocity * Time.deltaTime;
            if (!Input.GetKey(KeyCode.C))
            {
                var targetRotation = Quaternion.LookRotation(
                Quaternion.AngleAxis(_cameraOrbit.LookAngle,
                -transform.right) *
                velocity);
                transform.rotation = Quaternion.Slerp(transform.rotation,
                targetRotation, Time.deltaTime * speed);
            }
        }
        private void FromServerUpdate()
        {
            //Debug.Log($"{gameObject.name} =  {_playerName}");
            gameObject.name = _playerName;
        }


        [ClientCallback]
        private void LateUpdate()
        {
            _cameraOrbit?.CameraMovement();
        }

        private bool IsDangerCollision(Collision collision)
        {
            if (collision.collider.name == "Sun") return true;

            if (collision.collider.GetComponentInParent<PlanetOrbit>() != null) return true;

            if (collision.collider.GetComponentInParent<ShipController>() != null) return true;

            return false;
        }

        [ClientCallback]
        private void OnCollisionEnter(Collision collision)
        {
            if (!IsDangerCollision(collision)) return;

            Transform newPosition = NetworkManager.singleton.GetStartPosition();
            _networkTransform.CmdTeleport(newPosition.position);
        }
    }
}
