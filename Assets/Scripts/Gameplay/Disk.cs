using System;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class Disk : MonoBehaviour
    {

       [SerializeField] private int _index;
       [SerializeField] private Tower currentTower;
     public  bool isSelectable = true;
     public bool isOnTower = false;
     public bool movementRestricted = false;
    [SerializeField]   string selectableMask; 
    [SerializeField]   string unselectableMask;



         
        
        
        
        // Start is called before the first frame update
        void Start()
        {
            MovementRestriction(true);
        }

        // Update is called once per frame
        void Update()
        {
            transform.gameObject.tag = isSelectable ? selectableMask : unselectableMask;
            if (!isSelectable)
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }

        

       
       // Restrict Disk movement 
      public void MovementRestriction(bool isRestricted)
      {
          movementRestricted = isRestricted;
           if (isRestricted)
           {
               // restrict Position X and Position Z
               GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
           }
           else
           {
               GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
           }
       }

      private void OnCollisionEnter(Collision other)
      {
          if (other.collider.CompareTag("Base"))
          {
              if(!isOnTower)
              {GetCurrentTower().InitializeDiskStack(this);
              }
          }
      }


      public Tower GetCurrentTower() => currentTower;
       public void SetCurrentTower(Tower tower) => currentTower = tower;
       
        public int GetDiskIndex() => _index;
        
        
        

    }
}
