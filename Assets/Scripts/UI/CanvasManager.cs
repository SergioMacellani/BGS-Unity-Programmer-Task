using System.Collections;
using BGS.Character;
using BGS.Data;
using BGS.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace BGS.UI
{
    /// <summary>
    /// This class manages the UI canvas in the game.
    /// It handles the store window, custom character window, coins display, and interaction icon.
    /// </summary>
    public class CanvasManager : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance of the CanvasManager class.
        /// </summary>
        public static CanvasManager Instance { get; private set; }

        #region Variables

        [Header("References")] 
        [SerializeField]
        [Tooltip("Reference to the player's inventory data.")]
        private PlayerInventoryData inventory;

        [SerializeField]
        [Tooltip("Reference to the store manager HUD.")]
        private StoreManagerHUD store;

        [Space] 
        [Header("Windows")] 
        [SerializeField]
        [Tooltip("Reference to the store window.")]
        private CanvasGroup storeWindow;

        [SerializeField]
        [Tooltip("Reference to the custom character window.")]
        private CanvasGroup customCharacterWindow;
        
        [SerializeField]
        [Tooltip("The index of the active window.")]
        private uint windowIndex;

        [Space] 
        [Header("Coins")] 
        [SerializeField]
        [Tooltip("Reference to the coins text.")]
        private TextMeshProUGUI coinsText;

        [SerializeField]
        [Tooltip("Reference to the coins icon.")]
        private Image coinsIcon;

        /// <summary>
        /// The screen position of the coins icon.
        /// </summary>
        private Vector2 CoinsIconPosition => coinsIcon.rectTransform.position;
        
        /// <summary>
        /// The number of coins the player has.
        /// </summary>
        private uint _coins;

        [Space] 
        [Header("Interact")] 
        [SerializeField]
        [Tooltip("Reference to the interaction icon.")]
        private RectTransform interactIcon;

        [Tooltip("The world position of the interaction.")]
        internal Vector3 InteractWorldPosition;
        [Tooltip("Flag to check if the player is interacting.")]
        private bool _interactStay;

        #endregion

        #region Properties
        
        /// <summary>
        /// Property to get or set the interact stay flag and update the interaction icon accordingly.
        /// </summary>
        public bool InteractStay
        {
            get => _interactStay;
            set
            {
                _interactStay = value;
                interactIcon.gameObject.SetActive(value);
            }
        }
        
        #endregion

        #region Unity Callbacks

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// It initializes the singleton instance and loads the player's inventory data.
        /// </summary>
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            if (IOSystem.Exists("data")) JsonUtility.FromJsonOverwrite(IOSystem.LoadFile("data"), inventory);
            if (PlayerInventoryData.Instance == null) PlayerInventoryData.Instance = inventory;

            if (inventory.coins <= 0) inventory.coins = (uint)Random.Range(500, 1500);
            _coins = inventory.coins;
            coinsText.text = _coins.ToString("0000");
        }

        /// <summary>
        /// Start is called before the first frame update.
        /// It sets the initial window.
        /// </summary>
        private void Start()
        {
            ChangeWindow(0);
        }

        /// <summary>
        /// Update is called every frame.
        /// It updates the position of the interaction icon if it is active.
        /// </summary>
        private void Update()
        {
            if (_interactStay)
            {
                interactIcon.position = Camera.main!.WorldToScreenPoint(InteractWorldPosition);
            }
        }
        
        #endregion

        #region Public Methods

        /// <summary>
        /// Opens the store window and sets up the store items.
        /// </summary>
        public void OpenStore(BodyPartType storeType)
        {
            store.SetUpStoreItems(storeType);
            ChangeWindow(1);
        }

        /// <summary>
        /// Changes the active window.
        /// </summary>
        public void ChangeWindow(int index)
        {
            windowIndex = (uint)index;
            UpdateWindows();

            PlayerController.Instance.FocusMode = index > 0;
        }
        
        /// <summary>
        /// Adds coins to the player's inventory and updates the coins display.
        /// </summary>
        public void AddCoins(int value, Vector2 position = default)
        {
            if (value > 0) StartCoroutine(InstantiateCoins(value, position));
            StartCoroutine(ChangeCoins(value));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the visibility and interactivity of the windows based on the window index.
        /// </summary>
        private void UpdateWindows()
        {
            storeWindow.alpha = windowIndex == 1 ? 1 : 0;
            storeWindow.blocksRaycasts = windowIndex == 1;
            storeWindow.interactable = windowIndex == 1;

            customCharacterWindow.alpha = windowIndex == 2 ? 1 : 0;
            customCharacterWindow.blocksRaycasts = windowIndex == 2;
            customCharacterWindow.interactable = windowIndex == 2;
        }

        /// <summary>
        /// Changes the number of coins and updates the coins text.
        /// </summary>
        private IEnumerator ChangeCoins(int value)
        {
            var endValue = _coins + value;
            var duration = 1f;

            for (var time = 0f; time < duration; time += Time.deltaTime)
            {
                _coins = (uint)Mathf.Lerp(_coins, endValue, time / duration);
                coinsText.text = _coins.ToString("0000");
                yield return null;
            }
        }

        /// <summary>
        /// Instantiates coins icons and moves them to the coins icon position.
        /// </summary>
        private IEnumerator InstantiateCoins(int value, Vector2 position)
        {
            for (var i = 0; i < value; i++)
            {
                var c = Instantiate(coinsIcon, position, Quaternion.identity, transform);
                c.AddComponent<CoinMovement>().Move(CoinsIconPosition);
                yield return new WaitForSeconds(1f / value);
            }
        }
        
        #endregion

#if UNITY_EDITOR
        /// <summary>
        /// OnValidate is called when the script is loaded or a value is changed in the inspector.
        /// It updates the windows.
        /// </summary>
        private void OnValidate()
        {
            UpdateWindows();
        }
#endif
    }
}