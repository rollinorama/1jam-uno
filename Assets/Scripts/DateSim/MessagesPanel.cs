using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        private GameManager _gameManager;

        private void Awake()
        {
            _cellPhone = GetComponentInParent<CellPhone>();
            _scrollBar = FindObjectOfType<Scrollbar>();
            _cellPhone.OpenEvent += ScrollToBottom;
            _answerPanel.SendMessage += SendMessage;
            _gameManager = FindObjectOfType<GameManager>();
            _gameManager.SetOldMessages += SetOldMessages;
        }

        private void SetOldMessages(List<MessageText> texts)
        {
            if (texts.Count > 0)
            {
                foreach (MessageText text in texts)
                {
                    GameObject prefab = text.messageType == MessageText.MessageType.sendedMessage ? _myMessagePrefab : _sendedMessagePrefab;
                    GameObject sendedMessage = Instantiate(prefab, transform.position, Quaternion.identity, transform);
                    sendedMessage.GetComponentInChildren<TextMeshProUGUI>().text = text.text;
                }
            }
            _gameManager.SetOldMessages -= SetOldMessages;
        }

        private void Start()
        {
            //_actualAnswer = new Answer(_firstAnswer);
        }

        private void SendMessage(Answer answer)
        {
            _actualAnswer = answer;
            GameObject sendedMessage = Instantiate(_myMessagePrefab, transform.position, Quaternion.identity, transform);
            sendedMessage.GetComponentInChildren<TextMeshProUGUI>().text = answer.trueAnswerText;
            StartCoroutine(Co_ReceiveMessage(answer.textPath));
            _gameManager.AddText(answer.trueAnswerText, MessageText.MessageType.sendedMessage);
        }

        public void ReceiveMessage(DateSimText dateSimText)
        {
            GameObject sendedMessage = Instantiate(_sendedMessagePrefab, transform.position, Quaternion.identity, transform);
            sendedMessage.GetComponentInChildren<TextMeshProUGUI>().text = dateSimText.textMessage;
            _answerPanel.SetAnswers(dateSimText.answers);
            _gameManager.AddText(dateSimText.textMessage, MessageText.MessageType.receivedMessage);

        }

        IEnumerator Co_ReceiveMessage(DateSimText dateSimText)
        {
            yield return new WaitForSeconds(1f);
            ReceiveMessage(dateSimText);
        }

        private void ScrollToBottom()
        {
            _scrollBar.Select();
            _scrollBar.value = -1;
        }
    }
}

