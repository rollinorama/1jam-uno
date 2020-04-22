using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SG.Unit;

namespace SG
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] GameData _data;
        [SerializeField] Transform _startWaypoint;
        [SerializeField] Transform _endWaypoint;
        [SerializeField] Animator _sceneTransition;
        [SerializeField] float _deathDelay;

        private Player _player;
        private int _actualSceneIndex;

        private void Awake()
        {
            _player = FindObjectOfType<Player>();
            _actualSceneIndex = SceneManager.GetActiveScene().buildIndex;
        }

        private void Start()
        {
            _player.transform.position = _startWaypoint.position;
        }

        public void LoadNextScene()
        {
            StartCoroutine(Co_LoadScene(_actualSceneIndex + 1));
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
            StartCoroutine(Co_LoadScene(_actualSceneIndex));
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
