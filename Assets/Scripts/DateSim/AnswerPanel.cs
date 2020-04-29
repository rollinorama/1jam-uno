using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

namespace SG.DateSim
{
    public class AnswerPanel : MonoBehaviour
    {
        public event Action<Answer> SendMessage;

        [SerializeField] GameObject _answerPrefab;
        [SerializeField] Sprite _selectedAnswerSprite;
        [SerializeField] Sprite _unselectedAnswerSprite;
        [SerializeField] AudioClip _audioAnswer;


        private GameManager _gameManager;
        private CellPhone _cellPhone;
        private AudioSource _audioSource;

        private List<Answer> _answers = new List<Answer>();
        private List<GameObject> _answersGM = new List<GameObject>();
        private int _selectedAnswerIndex = 0;

        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _cellPhone = GetComponentInParent<CellPhone>();
            _audioSource = GetComponentInParent<AudioSource>();

            _cellPhone.OpenEvent += NavigateToFirstAnswer;
        }

        private void NavigateToFirstAnswer()
        {
            NavigateAnswer();
        }

        private void NavigateAnswer(int index = 0)
        {
            if (_answersGM.Count > 0)
            {
                _answersGM[_selectedAnswerIndex].GetComponent<Image>().sprite = _unselectedAnswerSprite;
                _selectedAnswerIndex = index;
                _answersGM[_selectedAnswerIndex].GetComponent<Image>().sprite = _selectedAnswerSprite;
            }
        }

        public void SetAnswers(DateSimAnswer[] answers)
        {
            ClearAnswers();
            foreach (DateSimAnswer answer in answers)
            {
                _answers.Add(new Answer(answer));
                GameObject newAnswer = Instantiate(_answerPrefab, transform.position, Quaternion.identity, transform);
                newAnswer.GetComponentInChildren<TextMeshProUGUI>().text = answer.answerText;
                _answersGM.Add(newAnswer);
            }

            _selectedAnswerIndex = 0;
            NavigateAnswer();
        }

        private void SelectAnswer()
        {
            _gameManager.SetAnswerType(_answers[_selectedAnswerIndex].goodAnswer);
            SendMessage(_answers[_selectedAnswerIndex]);
            _audioSource.PlayOneShot(_audioAnswer);
            ClearAnswers();
        }

        private void ClearAnswers()
        {
            if (_answers.Count > 0)
            {
                _answersGM.Clear();
                _answers.Clear();
                foreach (Transform child in transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        private void OnSelect(InputValue value)
        {
            if (_cellPhone.openedPhone && _answers.Count > 0 && value.Get<float>() > 0)
            {
                SelectAnswer();
            }
        }

        private void OnNavigateLeft(InputValue value)
        {
            if (_cellPhone.openedPhone && _answers.Count > 0 && value.Get<float>() > 0)
            {
                int idx = (_selectedAnswerIndex + 1) % _answers.Count;
                NavigateAnswer(idx);
            }
        }

        // REFATORAR
        private void OnNavigateRight(InputValue value)
        {
            if (_cellPhone.openedPhone && value.Get<float>() > 0)
            {
                int idx = (_selectedAnswerIndex + 1) % _answers.Count;
                NavigateAnswer(idx);
            }
        }
    }
}
