using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace SG.DateSim
{
    public class CellphoneSelectAnswer : MonoBehaviour
    {
        private CellPhone _cellPhone;

        private void Awake()
        {
            _cellPhone = GetComponent<CellPhone>();
        }

        private void OnNavigateLeft(InputValue value)
        {
            if (value.Get<float>() > 0)
            {

            }
        }

    }
}
