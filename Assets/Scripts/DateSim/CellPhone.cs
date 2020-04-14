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

        private Answer _actualAnswer;
        private MessagesPanel _messagesBoard;
        [HideInInspector]
        public AnswerPanel _answersBoard;


        private RectTransform _rectTransform;
        private Animation _ringArrow;

        public bool openedPhone;

        private void Awake()
        {
            _messagesBoard = GetComponentInChildren<MessagesPanel>();
            _answersBoard = GetComponentInChildren<AnswerPanel>();


            _rectTransform = GetComponent<RectTransform>();
           
            _ringArrow = transform.Find("RingArrow").GetComponent<Animation>();
            _actualAnswer = new Answer(initialAnswer);
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
            _ringArrow.gameObject.SetActive(true);
            _ringArrow.Play();
           yield return new WaitForSeconds(_timeDelayRing);
            _ringArrow.Stop();
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
                openedPhone = false;
                _rectTransform.position = new Vector2(_rectTransform.position.x, _closedPhonePosY);
                
            }
            else
            {
                openedPhone = true;
                OpenEvent();
                _rectTransform.position = new Vector2(_rectTransform.position.x, _openedPhonePosY);
            }
        }

    }
}
