using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SG.DateSim
{

    public class CellPhone : MonoBehaviour
    {
        public event Action RingEvent;

        [SerializeField] GameObject _sendedMessagePrefab;
        [SerializeField] GameObject _myMessagePrefab;
        [SerializeField] GameObject _answerPrefab;
        [SerializeField] DateSimAnswer initialAnswer;
        [SerializeField] float _timeDelayRing;
        [SerializeField] float _closedPhonePosY;
        [SerializeField] float _openedPhonePosY;

        private Answer _actualAnswer;
        private VerticalLayoutGroup _messagesBoard;
        private GridLayoutGroup _answersBoard;


        private RectTransform _rectTransform;
        private Transform _ringArrow;
        private Scrollbar _scrollBar;

        private int _messagesCount;
        public bool openedPhone;
        public Vector2 InputNavigation { get; set; }

        private void Awake()
        {
            _messagesBoard = GetComponentInChildren<VerticalLayoutGroup>();
            _answersBoard = GetComponentInChildren<GridLayoutGroup>();



            _rectTransform = GetComponent<RectTransform>();
            _scrollBar = GetComponentInChildren<Scrollbar>();
            _ringArrow = transform.Find("RingArrow");
            _actualAnswer = new Answer(initialAnswer);
        }

        void Start()
        {
            StartCoroutine(Co_ReceiveMessage(_actualAnswer));
        }

        IEnumerator Co_ReceiveMessage(Answer answer)
        {
            yield return new WaitForSeconds(3f);
            GameObject sendedMessage = Instantiate(_sendedMessagePrefab, transform.position, Quaternion.identity, _messagesBoard.transform);
            sendedMessage.GetComponentInChildren<TextMeshProUGUI>().text = answer.textPath.textMessage;
            SetAnswers();
        }

        private void SetAnswers()
        {
            foreach (Transform child in _answersBoard.transform)
            {
                Destroy(child);
            }
            foreach (DateSimAnswer answer in _actualAnswer.textPath.answers)
            {
                var newAnswer = new Answer(answer);
               newAnswer = Instantiate(new Answer(answer), _answersBoard.transform.position, Quaternion.identity, _answersBoard.transform);
            }
        }

        public void SetRing()
        {
            _messagesCount++;
            Ring();
        }

        private IEnumerator Co_SetRing()
        {
            Ring();
            while (_messagesCount > 0)
            {
                yield return new WaitForSeconds(_timeDelayRing);
                Ring();
            }
            yield return null;
        }

        private void Ring()
        {
            RingEvent();
            _ringArrow.gameObject.SetActive(true);
        }

        public void OpenClosePhone()
        {
            if (openedPhone)
            {
                openedPhone = false;
                _rectTransform.position = new Vector2(_rectTransform.position.x, _closedPhonePosY);
                if (_messagesCount > 0)
                {
                    Ring();
                }
            }
            else
            {
                openedPhone = true;
                _rectTransform.position = new Vector2(_rectTransform.position.x, _openedPhonePosY);
                _scrollBar.Select();
                _scrollBar.value = -1;
                if (_messagesCount > 0)
                {
                    _ringArrow.gameObject.SetActive(false);
                    _messagesCount--;
                }
            }
        }

        public void SelectAnswer()
        {

        }
    }
}
