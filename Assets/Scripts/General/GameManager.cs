using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using SG.Unit;

namespace SG
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] GameData _data;
        [SerializeField] TextMeshProUGUI _levelName;
        [SerializeField] Transform _startWaypoint;
        [SerializeField] Transform _endWaypoint;
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
                _data.StartScene();
        }

        private void Init()
        {
            
        }

        public void LoadNextScene()
        {
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
    }

}
