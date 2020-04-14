using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace SG.DateSim
{
    public class MessagesPanel : MonoBehaviour
    {
        [SerializeField] GameObject _sendedMessagePrefab;
        [SerializeField] GameObject _myMessagePrefab;
        [SerializeField] AnswerPanel _answerPanel;
        [SerializeField] DateSimAnswer _firstAnswer;

        private CellPhone _cellPhone;
        private Answer _actualAnswer;
        private Scrollbar _scrollBar;

        private void Awake()
        {
            _cellPhone = GetComponentInParent<CellPhone>();
            _scrollBar = FindObjectOfType<Scrollbar>();
            _cellPhone.OpenEvent += ScrollToBottom;
            _answerPanel.SendMessage += SendMessage;
        }

        private void Start()
        {
            _actualAnswer = new Answer(_firstAnswer);
        }

        private void SendMessage(Answer answer)
        {
            _actualAnswer = answer;
            GameObject sendedMessage = Instantiate(_myMessagePrefab, transform.position, Quaternion.identity, transform);
            sendedMessage.GetComponentInChildren<TextMeshProUGUI>().text = answer.trueAnswerText;
        }

        public void ReceiveMessage()
        {
            GameObject sendedMessage = Instantiate(_sendedMessagePrefab, transform.position, Quaternion.identity, transform);
            sendedMessage.GetComponentInChildren<TextMeshProUGUI>().text = _actualAnswer.textPath.textMessage;
            _answerPanel.SetAnswers(_actualAnswer.textPath.answers);
        }

        private void ScrollToBottom()
        {
            _scrollBar.Select();
            _scrollBar.value = -1;
        }
    }
}

