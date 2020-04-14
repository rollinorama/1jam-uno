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
            if (_player.IsDead) return;

            if (_cellPhone.openedPhone)
            {
                _player.InputMovement = new Vector2(0, 0);
                _cellPhone.InputNavigation = value.Get<Vector2>();
            }
            else
                _player.InputMovement = value.Get<Vector2>();

        }

        private void OnAttack(InputValue value)
        {
            if (_player.IsDead) return;

            if (value.Get<float>() > 0)
                if (_cellPhone.openedPhone)
                    _cellPhone.SelectAnswer();
                else
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
    }
}
