using UnityEngine;
using UnityEngine.InputSystem;
using SG.DateSim;

namespace SG.Unit
{
    public class PlayerInput : MonoBehaviour
    {
        private Player _player;
        private CellPhone _cellPhone;

        private void Awake()
        {
            _player = GetComponent<Player>();
            _cellPhone = FindObjectOfType<CellPhone>();
        }

        private void OnMove(InputValue value)
        {
            if (_player.IsDead || _cellPhone.openedPhone)
                _player.InputMovement = Vector2.zero;
            else
                _player.InputMovement = value.Get<Vector2>();
        }

        private void OnAttack(InputValue value)
        {
            if (_player.IsDead || _cellPhone.openedPhone) return;

            if (value.Get<float>() > 0)
                _player.Attack();
        }

        private void OnGrab(InputValue value)
        {
            if (_player.IsDead || _cellPhone.openedPhone) return;

            if (value.Get<float>() > 0)
                _player.Grab();
        }

        private void OnOpenPhone(InputValue value)
        {
            if (_player.IsDead) return;

            if (value.Get<float>() > 0)
                _player.OpenClosePhone();
        }

        private void OnTeleport(InputValue value)
        {
            if (value.Get<float>() > 0)
                _player.Teleport();
        }
    }
}
