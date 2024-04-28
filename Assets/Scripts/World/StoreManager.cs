using BGS.Data;
using BGS.UI;
using UnityEngine;

namespace BGS.World
{
    /// <summary>
    /// This class is used to manage the store in the game world.
    /// It inherits from the TriggerInteract class and overrides its functionality to open the store when interacted with.
    /// </summary>
    public class StoreManager : TriggerInteract
    {
        #region Variables

        [Space] 
        [Header("Store Info")] 
        [SerializeField]
        [Tooltip("The type of body part that the store sells.")]
        private BodyPartType storeType;

        [Space] 
        [Header("Vendor Character")] 
        [SerializeField]
        [Tooltip("The character that represents the vendor.")]
        private Transform vendorCharacter;

        /// <summary>
        /// The initial scale of the vendor character.
        /// This is used to flip the character to face the player.
        /// </summary>
        private Vector3 _vendorCharacterInitialScale;
        
        #endregion

        #region Unity Callbacks
        
        /// <summary>
        /// It initializes the initial scale of the vendor character and adds a listener to the interaction event to open the store.
        /// </summary>
        private void Start()
        {
            _vendorCharacterInitialScale = vendorCharacter.localScale;
            onInteract.AddListener(() =>
            {
                Debug.Log("Open Store");
                CanvasManager.Instance.OpenStore(storeType);
            });
        }

        /// <summary>
        /// It flips the vendor character to face the player.
        /// </summary>
        private void FixedUpdate()
        {
            vendorCharacter.localScale =
                new Vector3(
                    Mathf.Sign(Camera.main!.transform.position.x - vendorCharacter.position.x) *
                    _vendorCharacterInitialScale.x,
                    _vendorCharacterInitialScale.y,
                    _vendorCharacterInitialScale.z);
        }
        
        #endregion
    }
}