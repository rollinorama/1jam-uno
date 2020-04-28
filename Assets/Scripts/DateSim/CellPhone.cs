using System;
using System.Collections;
using UnityEngine;
using SG.Unit;

namespace SG.DateSim
{

    public class CellPhone : MonoBehaviour
    {
        public event Action RingEvent;
        public event Action OpenEvent;
        
        [SerializeField] float _timeDelayRing;
        [SerializeField] Animator _uiFog;
        [SerializeField] GameObject _messageCounter;

        private DateSimText _actualDateSimText;
        private MessagesPanel _messagesBoard;
        [HideInInspector]
        public AnswerPanel _answersBoard;
        private Animator _animator;

        private Transform _ringArrow;
        private MainCamera _camera;

        public bool openedPhone;

        private void Awake()
        {
            _messagesBoard = GetComponentInChildren<MessagesPanel>();
            _answersBoard = GetComponentInChildren<AnswerPanel>();
            _animator = GetComponent<Animator>();
            _ringArrow = transform.Find("RingArrow");
            _camera = FindObjectOfType<MainCamera>();
        }

        public void SetRing(DateSimText dateSimText)
        {
            _actualDateSimText = dateSimText;
            StartCoroutine(Co_SetRing());
        }

        private IEnumerator Co_SetRing()
        {
            Animation ringArrowAnimation = _ringArrow.GetComponentInChildren<Animation>();
            _ringArrow.gameObject.SetActive(true);
            ringArrowAnimation.Play();
           yield return new WaitForSeconds(_timeDelayRing);
            ringArrowAnimation.Stop();
            _ringArrow.gameObject.SetActive(false);
           Ring();
        }

        private void Ring()
        {
            RingEvent();
            _messagesBoard.ReceiveMessage(_actualDateSimText);
            _messageCounter.SetActive(true);
        }

        public void OpenClosePhone()
        {
            if (openedPhone)
            {
                _camera.CellPhoneClose();
                openedPhone = false;
                _animator.SetTrigger("setClose");
                _uiFog.SetTrigger("setClose");
            }
            else
            {
                StartCoroutine(_camera.CellPhoneOpen());
                openedPhone = true;
                _animator.SetTrigger("setOpen");
                _uiFog.SetTrigger("setOpen");
                OpenEvent();
                _messageCounter.SetActive(false);
            }
        }

    }
}
