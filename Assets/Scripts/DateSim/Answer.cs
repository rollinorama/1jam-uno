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
        public MessageActionType messageActionType;

        public Answer(DateSimAnswer answer)
        {
            answerText = answer.answerText;
            trueAnswerText = answer.trueAnswerText;
            goodAnswer = answer.goodAnswer;
            textPath = answer.textPath;
            messageActionType = answer.textPath.messageActionType;
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
