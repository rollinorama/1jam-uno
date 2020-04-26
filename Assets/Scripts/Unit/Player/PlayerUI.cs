using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SG.Unit
{
    public class PlayerUI : MonoBehaviour
    {

        [SerializeField] TextMeshProUGUI _buttonText;
        [SerializeField] TextMeshProUGUI _mainText;

        private Animator _animator;
        private bool _isOpened;
        private PlayerUIButtonType _buttonType;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void OpenUI(string buttonText, string mainText, PlayerUIButtonType buttonType)
        {
            if (!_isOpened)
            {
                _isOpened = true;
                _buttonType = buttonType;
                _animator.SetBool("isOpen", true);
                _buttonText.text = buttonText;
                _mainText.text = mainText;
            }
        }

        public void CloseUI(PlayerUIButtonType buttonType)
        {
            if (_isOpened && _buttonType == buttonType)
            {
                _isOpened = false;
                _animator.SetBool("isOpen", false);
            }
        }


        

    }

    public enum PlayerUIButtonType
    {
        Grab,
        Sewer,
        Phone
    }
}
