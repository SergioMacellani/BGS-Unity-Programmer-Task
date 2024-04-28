using BGS.UI;
using UnityEngine;
using UnityEngine.Events;

namespace BGS.World
{
    /// <summary>
    /// This class is used to manage the interaction triggers in the game world.
    /// It handles the interaction icon and the events that occur when the player interacts with the trigger.
    /// </summary>
    public class TriggerInteract : MonoBehaviour
    {

        #region Variables
        
        [SerializeField] 
        [Tooltip("The location of the icon that appears when the player can interact with the trigger.")]
        private Transform interactionIcon;

        [SerializeField] 
        [Tooltip("The event that is invoked when the player interacts with the trigger.")]
        protected UnityEvent onInteract = new UnityEvent();

        [Tooltip("Flag that indicates if the player is in the trigger.")]
        private bool _triggerStay;

        #endregion

        #region Unity Callbacks
        
        /// <summary>
        /// It checks if the player is in the trigger and if the interaction key is pressed.
        /// If so, it invokes the interaction event.
        /// </summary>
        private void Update()
        {
            if (!_triggerStay) return;
            if (Input.GetKeyDown(KeyCode.E))
            {
                onInteract.Invoke();
            }
        }

        /// <summary>
        /// It checks if the collider is the player.
        /// If so, it sets the interaction icon position and sets the interaction stay flag to true.
        /// </summary>
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.CompareTag("Player")) return;

            CanvasManager.Instance.InteractWorldPosition = interactionIcon.position;
            CanvasManager.Instance.InteractStay = _triggerStay = true;
        }

        /// <summary>
        /// It checks if the collider is the player.
        /// If so, it sets the interaction stay flag to false.
        /// </summary>
        private void OnTriggerExit2D(Collider2D col)
        {
            if (!col.CompareTag("Player")) return;

            CanvasManager.Instance.InteractStay = _triggerStay = false;
        }

        #endregion

    }
}