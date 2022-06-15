using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class Grabber : MonoBehaviour
    {
        // This will grab disk from to the tower
        // player controlled Behavior
        
        [SerializeField] List<Tower> towers = new List<Tower>();
        [SerializeField] GameObject diskTransform;
        
        private Disk _currentDisk;
        private int _currentTowerIndex = 0;
        private Tower _currentTower;






        public void MoveToTower(int input)
        {
            _currentTowerIndex = input;

             if(_currentTowerIndex <= towers.Count-1)
            {
                _currentTowerIndex = 0;
            }
            
            _currentTower = towers[_currentTowerIndex];
            
            // do some Movement Here
        }


        
        
    }
}