using Helpers;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private GameObject _winText;
        [SerializeField] private TMP_Text movesText;
        [SerializeField] private TMP_Text minMovesText;

        [SerializeField] private TMP_Text disksText;
        
        public void ChangeDisksText(int disks)
        {
            disksText.text = disks.ToString();
        }

        public void ChangeMinMovesText(int moves)
        {
            minMovesText.text = moves.ToString();
        }
        public void ChangeMovesText(int moves)
        {
            movesText.text = $"Moves: {moves}";
        }

    }
}