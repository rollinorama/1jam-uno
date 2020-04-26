using UnityEngine;
using System.Collections;

namespace SG.DateSim
{
    public class Answer : IAnswer
    {
        public string answerText;
        public string trueAnswerText;
        public bool goodAnswer;
        public DateSimText textPath;

        public Answer(DateSimAnswer answer)
        {
            answerText = answer.answerText;
            trueAnswerText = answer.trueAnswerText;
            goodAnswer = answer.goodAnswer;
            textPath = answer.textPath;
        }

        public void Action()
        {
        }
    }

    public interface IAnswer
    {
        void Action();
    }
}
