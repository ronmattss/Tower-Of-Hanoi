using System.Collections.Generic;
using Gameplay;
using Helpers;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public Tower winningTower;
        public Tower startingTower;
        public Tower secondTower;
        public List<GameObject> disks = new List<GameObject>();
        
        [SerializeField] private List<GameObject> disksInPlay = new List<GameObject>();

        [Header("Disks")]
        public int numberOfDisks = 3;

        public int numberOfMoves = 0;

        public Solver solver;

        public UnityEvent onRestart;
        public UnityEvent onWinEvent;


        void Start()
        {
            ReadyDisks();
            PlaceDiskToTower();
        }


        public void RestartGame()
        {
            StopAllCoroutines();
            numberOfMoves =0;
            UIManager.Instance.ChangeMovesText(GameManager.Instance.numberOfMoves);
            ResetTowers();
            ReadyDisks();
            PlaceDiskToTower();
            onRestart?.Invoke();

        }

 

        public void ReadyDisks()
        {
            foreach (var disks in disksInPlay)
            {
                disks.SetActive(false);
            }
            
            
            disksInPlay.Clear();

            disks.Reverse(); // reverse order to form stack
            for (int i = 0; i < numberOfDisks; i++)
            {
                disksInPlay.Add(disks[i]);
            }

            disksInPlay.Reverse(); // reverse order to form stack
            disks.Reverse(); // reverse order to normal order
            UIManager.Instance.ChangeDisksText(numberOfDisks);
        }

        public void SetDisks(int diskCount)
        {
           // Debug.Log("Invoked SetDisks");
            numberOfDisks = Mathf.Clamp(numberOfDisks+diskCount, 3, 10);
            ResetTowers();
            RestartGame();
            solver.SolveTower();
        }

        private void ResetTowers()
        {
            startingTower.Reset();
            winningTower.Reset();
            secondTower.Reset();
        }


        private void PlaceDiskToTower()
        {
            var initTower = startingTower;
            for (int i = 0; i < disksInPlay.Count; i++)
            {
                disksInPlay[i].SetActive(true);
                initTower.InitializeDiskStack(disksInPlay[i].GetComponent<Disk>());
            }
        }
    }
}
