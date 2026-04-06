using TMPro;
using UnityEngine;


    public class GameUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _playerName;
        [SerializeField] TextMeshProUGUI _playerScore;

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }

        private void Start()
        {

        }

        private void OnScoreUpdate(int score)
        {
            _playerScore.text = score.ToString();
        }
    }

