using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gameplay
{
    public class Tower : MonoBehaviour
    {
        // Start is called before the first frame update


        [SerializeField] private Stack<Disk> disksStack = new Stack<Disk>();
        [SerializeField] private List<Disk> disksList = new List<Disk>();
        [SerializeField] private bool isWinningTower = false;



        public Stack<Disk>GetDiskStack() => disksStack; 
        public List<Disk> GetDisksList() => disksList; // to make sure that the tower has the disks
        // check if Disk are already in the list
        bool CheckDiskList(Object disk) => disksList.Any(t => t == disk);
        
        // checks the Tower if is the winning tower and it is in order
        public void CheckDisks()
        {
            if (!isWinningTower) return;
            if (disksList.Count != GameManager.Instance.numberOfDisks) return;
            disksList.Reverse(); // reverse the list to check if it is in order
            for (var i = 0; i < disksList.Count; i++)
            {
                if (i != disksList[i].GetDiskIndex())
                {
                    return;
                }
            }

            Debug.Log("You win!");
            
            // invoke events 
            GameManager.Instance.onWinEvent?.Invoke();
            // Do some Game Over stuff
        }

        // Resets the Tower data
        public void Reset()
        {
            disksStack.Clear();
            disksList.Clear();
        }

        // initialize the Tower with disks from runtime
        public void InitializeDiskStack(Disk disk)
        {
            if (CheckDiskList(disk)) return;
            var position = transform.position;
            if (disksStack.Count == 0)
            {
                disksStack.Push(disk);
                disk.transform.position = new Vector3(position.x, 0, position.z);
                Debug.Log("disk: " + disk.GetDiskIndex());
            }
            else
            {
                disk.transform.position = new Vector3(position.x,
                    disksStack.Peek().transform.position.y + 1, position.z);

                disksStack.Peek().isSelectable = false;
                disksStack.Push(disk);
                disksStack.Peek().isSelectable = true;
                Debug.Log("disk: " + disk.GetDiskIndex());
            }

            disk.SetCurrentTower(this);
            
            disksList.Add(disk);
        }

        // used to check disk placed by the player
        public void PlaceDiskStack(Disk disk)
        {
            if (CheckDiskList(disk)) return;
            var position = transform.position;
            if (disksStack.Count == 0)
            {
                disksStack.Push(disk);
                Debug.Log("disk: " + disk.GetDiskIndex());
            }
            else
            {
                disksStack.Peek().isSelectable = false;
                disksStack.Push(disk);
                disksStack.Peek().isSelectable = true;
                Debug.Log("disk: " + disk.GetDiskIndex());
            }
            
            disksList.Add(disk);
            disk.isOnTower = true;
        }





        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Disk"))
            {

                
                GameObject o = other.gameObject;
                var currDisk = o.GetComponent<Disk>();
                if (disksStack.Count > 0)
                {
                   // Debug.Log($"Tower: {name} disk: {currDisk.GetDiskIndex()} > Disk on Stack {disksStack.Peek().GetDiskIndex()}");
                    
                    if (currDisk.GetDiskIndex() > disksStack.Peek().GetDiskIndex())
                    {
                        currDisk.GetCurrentTower().InitializeDiskStack(currDisk);
                        return;
                    }
                }


                currDisk.MovementRestriction(true); // restrict movement of Disk
                
                var position = o.transform.position;
                position = new Vector3(transform.position.x, position.y,
                    position.z); // Center Disk if it touches the tower
                o.transform.position = position;
                Debug.Log("Disk entered to "+gameObject.name);

                PlaceDiskStack(o.GetComponent<Disk>());

                if (isWinningTower)
                {
                    CheckDisks();
                }
                
                Debug.Log($"{gameObject.name} Stack Count: {disksStack.Count}");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Disk"))
            {
                other.gameObject.GetComponent<Disk>().MovementRestriction(false);
                Debug.Log("Disk exited from tower "+gameObject.name);
                other.GetComponent<Disk>().isOnTower = false;

                if (disksStack.Count <= 0) return;
                if (other.gameObject.GetComponent<Disk>().GetDiskIndex() == disksStack.Peek().GetDiskIndex()) // prevent from removing the wrong disk
                {
                    disksStack.Pop();
                    disksList.Remove(other.GetComponent<Disk>());
                }


                if (disksStack.Count == 0) return;

                Debug.Log(disksStack.Peek().GetDiskIndex());
                disksStack.Peek().isSelectable = true;
                Debug.Log($"{gameObject.name} Stack Count: {disksStack.Count}");
                

            }
        }
    }
}