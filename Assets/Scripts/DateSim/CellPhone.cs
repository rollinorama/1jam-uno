using System;
using System.Collections;
using UnityEngine;


namespace SG.DateSim
{

    public class CellPhone : MonoBehaviour
    {
        public event Action RingEvent;
        public event Action OpenEvent;
        
        [SerializeField] DateSimAnswer initialAnswer;
        [SerializeField] float _timeDelayRing;
        [SerializeField] float _closedPhonePosY;
        [SerializeField] float _openedPhonePosY;
        [SerializeField] Animator _uiFog;

        private Answer _actualAnswer;
        private MessagesPanel _messagesBoard;
        [HideInInspector]
        public AnswerPanel _answersBoard;
        private Animator _animator;

        private RectTransform _rectTransform;
        private Transform _ringArrow;
        private MainCamera _camera;

        public bool openedPhone;

        private void Awake()
        {
            _messagesBoard = GetComponentInChildren<MessagesPanel>();
            _answersBoard = GetComponentInChildren<AnswerPanel>();

            _animator = GetComponent<Animator>();
            _rectTransform = GetComponent<RectTransform>();

            _ringArrow = transform.Find("RingArrow");
            _actualAnswer = new Answer(initialAnswer);

            _camera = FindObjectOfType<MainCamera>();
        }

        void Start()
        {
            StartCoroutine(Co_ReceiveMessage());
        }

        IEnumerator Co_ReceiveMessage()
        {
            yield return new WaitForSeconds(3f);
            _messagesBoard.ReceiveMessage();
        }

        public void SetRing()
        {
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
            _messagesBoard.ReceiveMessage();
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
            }
        }

    }
}
