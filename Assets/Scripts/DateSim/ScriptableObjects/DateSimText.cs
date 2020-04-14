using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG.DateSim
{
    [CreateAssetMenu(fileName = "DateSimState", menuName = "States/DateSimState")]
    public class DateSimText : ScriptableObject
    {
        [TextArea(14, 10)] public  string textMessage;
        public DateSimAnswer[] answers;
        public MessageActionType messageActionType;
    }
    public enum MessageActionType
    {
        AllEnemiesDefeated,
        WaypointReach,
        EnterLevel
    }
}
