using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace SG
{
    public class EndManager : MonoBehaviour
    {
        [SerializeField] GameData _data;
        [SerializeField] GameObject _badEndingImage;
        [SerializeField] GameObject _badEndingText;
        [SerializeField] GameObject _goodEndingImage;
        [SerializeField] GameObject _goodEndingText;

        [SerializeField] TextMeshProUGUI _badAnswers;
        [SerializeField] TextMeshProUGUI _goodAnswers;
        [SerializeField] TextMeshProUGUI _playerDeaths;
        [SerializeField] TextMeshProUGUI _enemyKills;

        void Start()
        {
            if (_data.goodGuyAnswers > _data.badGuyAnswers)
            {
                _goodEndingImage.SetActive(true);
                _goodEndingText.SetActive(true);
            }
            else
            {
                _badEndingImage.SetActive(true);
                _badEndingText.SetActive(true);
            }
            SetStats();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
                SceneManager.LoadScene(0);
        }

        private void SetStats()
        {
            _badAnswers.text = _data.badGuyAnswers.ToString();
            _goodAnswers.text = _data.goodGuyAnswers.ToString();
            _playerDeaths.text = _data.playerDeaths.ToString();
            _enemyKills.text = _data.enemyKills.ToString();
        }
    }
}
