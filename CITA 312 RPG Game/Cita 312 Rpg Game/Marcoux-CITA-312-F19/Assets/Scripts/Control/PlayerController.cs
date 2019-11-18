using RPG.Movement;
using UnityEngine;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;

        private void Start()
        {
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (health.IsDead()) return;
            if (InteractWithCombat()) return;           
            if (InteractWithMovement()) return;
            // print("Nothing to do.");
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                GameObject targetGameObject = target.gameObject;
                // if the target is null, skip the rest of the statements in this foreach loop
                if (!GetComponent<Fighter>().CanAttack(target.gameObject))
                {
                    continue; // continue to the next section of the foreach loop
                }
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit) // if something is found
            {
                if (Input.GetMouseButton(0))
                {
                    // we are including the speed for both the player and enemy and we always want our
                    // player to be moving at maximum speed, therefore we multiply the maximum speed by 1f
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);
                }
                return true;
            }
            return false; // if nothing is found
        } // MoveToCursor

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    } // PlayerController

}