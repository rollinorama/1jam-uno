﻿using System;
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

        private CellPhone _cellPhone;

        private List<Answer> _answers = new List<Answer>();
        private List<GameObject> _answersGM = new List<GameObject>();
        private int _selectedAnswerIndex = 0;

        private void Awake()
        {
            _cellPhone = GetComponentInParent<CellPhone>();

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
                _answersGM[_selectedAnswerIndex].GetComponent<Image>().color = Color.red;
                _selectedAnswerIndex = index;
                _answersGM[_selectedAnswerIndex].GetComponent<Image>().color = Color.green;
            }
        }

        public void SetAnswers(DateSimAnswer[] answers)
        {
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
            SendMessage(_answers[_selectedAnswerIndex]);
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

            }
        }
    }
}