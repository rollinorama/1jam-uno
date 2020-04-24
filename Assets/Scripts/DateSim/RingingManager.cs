using System.Collections;
using UnityEngine;

namespace SG.DateSim
{
    public class RingingManager : MonoBehaviour
    {
        [SerializeField] CellPhone _cellphone;
        [SerializeField] float _checkCollideTime;

        private bool loaded = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") &&  !loaded)
            {
                Debug.Log("ring ring");
                StartCoroutine(Co_CheckRinging());
            }
        }

        private IEnumerator Co_CheckRinging()
        {
            loaded = true;
            yield return new WaitForSeconds(_checkCollideTime);
            _cellphone.SetRing();
            Destroy(gameObject);
        }
    }
}
