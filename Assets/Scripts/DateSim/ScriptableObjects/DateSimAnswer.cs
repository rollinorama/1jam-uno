using UnityEngine;
using System.Collections;

namespace SG.DateSim
{
    [CreateAssetMenu(fileName = "DateSimAnswer", menuName = "States/DateSimAnswer")]
    public class DateSimAnswer : ScriptableObject
    {
        public string answerText;
        [TextArea(14, 10)] public string trueAnswerText;
        public DateSimText textPath;
        public bool goodAnswer;
    }
}
