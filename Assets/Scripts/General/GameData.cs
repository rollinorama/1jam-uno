using System;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    [CreateAssetMenu(fileName = "GameData", menuName = "States/GameData")]
    public class GameData : ScriptableObject
    {
        public int playerDeaths;
        public int enemyKills;
        public List<MessageText> texts;
        public int goodGuyAnswers;
        public int badGuyAnswers;

        private int _tempEnemyKills;
        private List<MessageText> _tempTexts;
        private int _tempGoodGuyAnswers;
        private int _tempBadGuyAnswers;

        public void StartGame()
        {
            playerDeaths = 0;
            enemyKills = 0;
            texts.Clear();
        }

        public void StartScene()
        {
            _tempEnemyKills = enemyKills;
            _tempTexts = new List<MessageText>(texts);
            _tempGoodGuyAnswers = goodGuyAnswers;
            _tempBadGuyAnswers = badGuyAnswers;
        }

        public void RestartScene()
        {
            Debug.Log(texts.Count);
            Debug.Log(_tempTexts.Count);
            enemyKills = _tempEnemyKills;
            texts = new List<MessageText>(_tempTexts);
            goodGuyAnswers = _tempGoodGuyAnswers;
            badGuyAnswers = _tempBadGuyAnswers;
        }
    }

    [Serializable]
    public class MessageText
    {
        public string text;
        public MessageType messageType;

        public MessageText(string _text, MessageType _messageType)
        {
            text = _text;
            messageType = _messageType;
        }

        public enum MessageType
        {
            sendedMessage,
            receivedMessage,
        }
    }
}
