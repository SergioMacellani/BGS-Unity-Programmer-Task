using Cinemachine;
using UnityEngine;

namespace BGS.Character
{
    /// <summary>
    /// This class is used to control the player character in the game.
    /// It handles the movement and animation of the player character.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance of the PlayerController.
        /// </summary>
        public static PlayerController Instance { get; private set; }

        #region Variables
        
        [Header("References")] 
        [SerializeField]
        [Tooltip("The Rigidbody2D component of the player.")]
        private Rigidbody2D rigidbody2D;

        [SerializeField] 
        [Tooltip("The Animator component of the player.")]
        private Animator animator;

        [Space]
        [Header("Camera")]
        [SerializeField]
        [Tooltip("The virtual camera from the Cinemachine package.")]
        private CinemachineVirtualCamera virtualCamera;

        [SerializeField] 
        [Tooltip("The camera target for the focus mode.")]
        private Transform cameraTarget;
        
        [SerializeField]
        [Range(1, 20)]
        [Tooltip("The normal field of view of the camera.")]
        private float normalFOV = 12;
        [SerializeField]
        [Range(1, 20)]
        [Tooltip("The focus field of view of the camera.")]
        private float focusFOV = 6;
        
        [SerializeField] 
        [Tooltip("The position of the camera target in focus mode.")]
        private Vector2 focusPosition;
        [Tooltip("The position of the camera target in normal mode.")]
        private Vector2 _normalPosition;
        [Tooltip("The focus mode of the camera.")]
        private bool _focusMode = false;

        /// <summary>
        /// Property for setting and getting the focus mode of the camera.
        /// </summary>
        public bool FocusMode
        {
            get => _focusMode;
            set
            {
                Debug.Log("Focus Mode: " + value);
                _focusMode = value;
                // Change the field of view and position of the camera target
                virtualCamera.m_Lens.OrthographicSize = _focusMode ? focusFOV : normalFOV;
                
                // Change the position of the camera target
                var focus = transform.localScale.x > 0 ? focusPosition : new Vector2(-focusPosition.x, focusPosition.y);
                cameraTarget.localPosition = _focusMode ? focus : _normalPosition;
                
                // Reset the horizontal input
                if (_focusMode) animator.SetFloat(Horizontal, 0);
            }
        }

        [Space] 
        [Header("Movement")]
        [SerializeField]
        [Tooltip("The speed of the player character.")]
        private float speed = 5f;

        // Animator parameters
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");

        #endregion

        #region Inputs
        
        private static float HorizontalInput => Input.GetAxis("Horizontal");
        private static float SprintInput => Input.GetKey(KeyCode.LeftShift) ? 2f : 1f;
        private static Vector2 MovementInput => new Vector2(HorizontalInput * SprintInput, 0);

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// It initializes the instance of this class and the Rigidbody2D and Animator components.
        /// </summary>
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            if (!rigidbody2D) TryGetComponent(out rigidbody2D);
            if (!animator) TryGetComponent(out animator);

            _normalPosition = cameraTarget.localPosition;
        }

        /// <summary>
        /// It handles the movement of the player character.
        /// </summary>
        private void Update()
        {
            Move();
        }

        #endregion

        #region Movement

        /// <summary>
        /// Handles the movement of the player character.
        /// </summary>
        private void Move()
        {
            if (_focusMode) return;

            rigidbody2D.velocity = MovementInput * speed;
            animator.SetFloat(Horizontal, MovementInput.x);
            transform.localScale = MovementInput.x switch
            {
                > 0 => new Vector3(1, 1, 1),
                < 0 => new Vector3(-1, 1, 1),
                _ => transform.localScale
            };
        }

        #endregion
    }
}