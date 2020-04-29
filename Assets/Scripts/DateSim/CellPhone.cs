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

        [SerializeField] AudioClip _audioReceiveMessage;
        [SerializeField] AudioClip _audioNoise;
        [SerializeField] AudioClip _audioOpenClose;
        [SerializeField] float _timeDelayRing;
        [SerializeField] Animator _uiFog;
        [SerializeField] GameObject _messageCounter;

        private DateSimText _actualDateSimText;
        private MessagesPanel _messagesBoard;
        [HideInInspector]
        public AnswerPanel _answersBoard;
        private Animator _animator;
        private AudioSource _audioSource;

        private Transform _ringArrow;
        private MainCamera _camera;

        public bool openedPhone;

        private void Awake()
        {
            _messagesBoard = GetComponentInChildren<MessagesPanel>();
            _answersBoard = GetComponentInChildren<AnswerPanel>();
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
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
            _audioSource.PlayOneShot(_audioNoise);
            yield return new WaitForSeconds(_timeDelayRing);
            ringArrowAnimation.Stop();
            _ringArrow.gameObject.SetActive(false);
            Ring();
        }

        private void Ring()
        {
            RingEvent();
            _messagesBoard.ReceiveMessage(_actualDateSimText);
            if (!openedPhone)
            {
                _messageCounter.SetActive(true);
                _audioSource.PlayOneShot(_audioReceiveMessage);
            }
        }

        public void OpenClosePhone()
        {
            if (openedPhone)
            {
                _camera.CellPhoneClose();
                openedPhone = false;
                _animator.SetTrigger("setClose");
                _uiFog.SetTrigger("setClose");
                _audioSource.PlayOneShot(_audioOpenClose);
            }
            else
            {
                StartCoroutine(_camera.CellPhoneOpen());
                openedPhone = true;
                _animator.SetTrigger("setOpen");
                _uiFog.SetTrigger("setOpen");
                OpenEvent();
                _messageCounter.SetActive(false);
                _audioSource.Stop();
                _audioSource.PlayOneShot(_audioOpenClose);
            }
        }

    }
}
