using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG.DateSim
{
    [CreateAssetMenu(fileName = "DateSimText", menuName = "States/DateSimText")]
    public class DateSimText : ScriptableObject
    {
        [TextArea(14, 10)] public  string textMessage;
        public DateSimAnswer[] answers;
        public Image _image;
    }

}
