using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG.DateSim;

namespace SG
{
    [CreateAssetMenu(fileName = "GameData", menuName = "States/GameData")]
    public class GameData : ScriptableObject
    {
        public int playerDeaths;
        public int enemyKills;
        public List<DateSimText> texts;
        public int goodGuyAnswers;
        public int badGuyAnswers;

        private int _tempEnemyKills;
        private List<DateSimText> _tempTexts;
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
            _tempTexts = texts;
            _tempGoodGuyAnswers = goodGuyAnswers;
            _tempBadGuyAnswers = badGuyAnswers;
        }

        public void RestartScene()
        {
            enemyKills = _tempEnemyKills;
            texts = _tempTexts;
            goodGuyAnswers = _tempGoodGuyAnswers;
            badGuyAnswers = _tempBadGuyAnswers;
        }
    }
}
