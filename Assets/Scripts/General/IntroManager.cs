using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SG
{
    public class IntroManager : MonoBehaviour
    {
        [SerializeField] GameData _data;
        [SerializeField] GameObject[] _introImages;
        [SerializeField] GameObject[] _introTexts;

        private int actualIndex = 0;
        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = GetComponent<GameManager>();
        }

        private void Start()
        {
           _data.StartGame();
        }

        private void Update() // REFATORAR TUDO!!!
        {
            if (Input.GetKeyDown(KeyCode.A))
            {

                if (actualIndex + 1 < _introImages.Length)
                    StartCoroutine(Co_InstantiateIntro());
                else
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        private IEnumerator Co_InstantiateIntro()
        {
            _introTexts[actualIndex].GetComponent<Animator>().SetTrigger("setClose");
            yield return new WaitForSeconds(1f);
            _introImages[actualIndex].SetActive(false);
            _introTexts[actualIndex].SetActive(false);
            actualIndex++;
            _introImages[actualIndex].SetActive(true);
            _introTexts[actualIndex].SetActive(true);
        }

    }
}
