using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using SG.Unit;
using SG.DateSim;

namespace SG
{
    public class GameManager : MonoBehaviour
    {
        public Action<List<MessageText>> SetOldMessages;

        [SerializeField] public GameData _data;
        [SerializeField] TextMeshProUGUI _levelName;
        [SerializeField] Transform _startWaypoint;
        [SerializeField] Animator _sceneTransition;
        [SerializeField] float _deathDelay;

        private Player _player;
        private Scene _actualScene;

        private void Awake()
        {
            _player = FindObjectOfType<Player>();
            _actualScene = SceneManager.GetActiveScene();
        }

        private void Start()
        {
            _player.transform.position = _startWaypoint.position;
            _levelName.text = _actualScene.name.ToString();
            if (_actualScene.buildIndex == 0 && _data.playerDeaths == 0)
                _data.StartGame();
            else
            {
                _data.StartScene();
                SetOldMessages(_data.texts);
            }
        }

        public void LoadNextScene(Transform endWaypoint = null)
        {
            if (endWaypoint.name == "EndWaypointBack")
                StartCoroutine(Co_LoadScene(_actualScene.buildIndex));
            else
                StartCoroutine(Co_LoadScene(_actualScene.buildIndex + 1));
        }

        private IEnumerator Co_LoadScene(int sceneIndex)
        {
            _sceneTransition.SetTrigger("start");
            yield return new WaitForSeconds(_deathDelay);
            SceneManager.LoadScene(sceneIndex);
        }

        public void PlayerDeath()
        {
            _data.playerDeaths++;
            _data.RestartScene();
            StartCoroutine(Co_LoadScene(_actualScene.buildIndex));
        }
        public void EnemyDeath()
        {
            _data.enemyKills++;
        }

        public void SetAnswerType(bool goodAnswer)
        {
            if (goodAnswer)
                _data.goodGuyAnswers++;
            else
                _data.badGuyAnswers++;
        }

        public void AddText(String text, MessageText.MessageType messageType)
        {
            MessageText messageText = new MessageText(text, messageType);
            _data.texts.Add(messageText);
            for (int i = 0; i < _data.texts.Count; i++)
            {
                Debug.Log(_data.texts[i]);

            }
        }
    }

}
