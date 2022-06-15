using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Gameplay
{
    public class Solver : MonoBehaviour
    {
        // Solve Tower of Hanoi

        // Get minimum number of moves
        private int totalDisks = 3;

        private int totalMoves = 0;
        private string startTower = "A";
        private string middleTower = "B";
        private string endTower = "C";


        //Solver
        public Tower firstTower;
        public Tower secondTower;
        public Tower thirdTower;
        public List<string> steps = new List<string>();
        
        [SerializeField] private float solverSpeed = .25f;


        private void Start()
        {

            totalDisks = GameManager.Instance.numberOfDisks;
            SolveTower();
            Debug.Log("Total moves: " + totalMoves);
        }
        
        

       public  void SetupSolver()
        {
            StopAllCoroutines();
            SolveTower();
            StartCoroutine(Solution());

        }

       public void SolveTower()
       {
           steps.Clear();
           totalDisks = GameManager.Instance.numberOfDisks;
           totalMoves = 0;
           SolveTowerOfHanoi(totalDisks, startTower, endTower, middleTower);
           UIManager.Instance.ChangeMinMovesText(totalMoves);
       }


       
       private Tower GetTower(string towerName)
       {
           
           switch (towerName)
           {
               case "A":
               {
                   return firstTower;
               }
               case "B":
               {
                   return secondTower;
               }
               case "C":
               {
                   return thirdTower;
               }
               default:
               {
                   return null;
               }
           }
       }

       IEnumerator Solution()
       {
           
           for(int i = 0; i < steps.Count; i++)
           {
               var _startTower = GetTower(steps[i][0].ToString());
               var _endTower = GetTower(steps[i][1].ToString());
               MoveDisk(_startTower, _endTower);
               
               UIManager.Instance.ChangeMovesText(i+1);
                yield return new WaitForSeconds(.5f);
           }

           yield return null;
       }


       // Using LeanTween to move disks
       void MoveDisk(Tower _startTower, Tower _endTower)
       {
           var disk = _startTower.GetDisksList()[_startTower.GetDisksList().Count - 1];
           var position = _endTower.transform.position;
           
           var seqOfMoves = LeanTween.sequence();
                 seqOfMoves.append(disk.transform
            .LeanMove(new Vector3(disk.transform.position.x, 14, disk.transform.position.z), solverSpeed));
                 
        seqOfMoves.append(disk.transform.LeanMove(new Vector3(position.x, 14, position.z), solverSpeed)
            .setDelay(.5f));

        if (_endTower.GetDiskStack().Count > 0)
        {
            seqOfMoves.append(disk.transform.LeanMove(new Vector3(position.x, _endTower.GetDiskStack().Peek().transform.position.y + 1, position.z),solverSpeed));
        }
        else
        {
            seqOfMoves.append(disk.transform.LeanMove(new Vector3(position.x,
                0, position.z),solverSpeed));
        }
        
       }
       private void SolveTowerOfHanoi(int i, string start, string end, string middle)
        {

            if (i > 0)
            {
                SolveTowerOfHanoi(i - 1, start, middle,end);
                totalMoves++;
                Debug.Log("Move disk " + i + " from " + start + " to " + end);
                steps.Add(string.Concat(start,end));
                SolveTowerOfHanoi(i - 1, middle, end, start);
            }
        }



        // replace with pop
        // private void Solve(int diskInPlay, string start, string end, string middle)
        // {
        //
        //     if (diskInPlay > 0)
        //     {
        //         SolveTowerOfHanoi(i - 1, start, end, middle);
        //         totalMoves++;
        //         Debug.Log("Move disk " + i + " from " + start + " to " + end);
        //         SolveTowerOfHanoi(i - 1, middle, start, end);
        //     }
        // }

        
        /*var seqOfMoves = LeanTween.sequence();
         *                seqOfMoves.append(disk.transform
                    .LeanMove(new Vector3(disk.transform.position.x, 14, disk.transform.position.z), 0.15f)
                    .setDelay(.5f));
                seqOfMoves.append(disk.transform.LeanMove(new Vector3(position.x, 14, position.x), 0.15f)
                    .setDelay(.5f));
                seqOfMoves.append(disk.transform.LeanMove(new Vector3(position.x, 0, position.z),0.15f));
         *
         *
         *
         *                 seqOfMoves.append(disk.transform
                    .LeanMove(new Vector3(disk.transform.position.x, 14, disk.transform.position.z), 0.15f)
                    .setDelay(.5f));
                seqOfMoves.append(disk.transform.LeanMove(new Vector3(position.x, 14, position.x), 0.15f)
                    .setDelay(.5f));
                seqOfMoves.append(disk.transform.LeanMove(new Vector3(position.x,
                        disksStack.Peek().transform.position.y + 1, position.z),0.15f));
         */
        
        
        // private void Solve(int _diskCount,Tower _start, Tower _end, Tower _middle)
        // {
        //     if (_diskCount <= 0)  return;
        //
        //     var disk = _start.GetDiskStack().Pop(); 
        //     _end.GetDiskStack().Push(disk);
        //     _end.InitializeDiskStack(disk);
        //
        //     
        //     _diskCount = _start.GetDiskStack().Count;
        //
        //      Solve(_diskCount, _start,  _end,_middle);  //start, end, middle);
        //
        //     totalMoves++;
        //
        //    // _middle.GetDiskStack().Pop();
        //    _start.GetDiskStack().Push(disk);
        //
        //     _start.InitializeDiskStack(disk);
        //     _diskCount = _middle.GetDiskStack().Count;
        //     
        //
        //  Solve(_diskCount,_middle,  _start,_end); //middle, start, end);
        //
        // }
    }
}