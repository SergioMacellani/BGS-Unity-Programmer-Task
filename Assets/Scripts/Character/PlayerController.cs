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
        #region Variables

        /// <summary>
        /// Singleton instance of the PlayerController.
        /// </summary>
        public static PlayerController Instance { get; private set; }

        [Header("References")] [SerializeField]
        private Rigidbody2D rigidbody2D;

        [SerializeField] private Animator animator;

        [Space] [Header("Camera")] [SerializeField]
        private CinemachineVirtualCamera virtualCamera;

        [SerializeField] private Transform cameraTarget;
        [SerializeField] [Range(1, 20)] private float normalFOV = 12, focusFOV = 6;
        [SerializeField] private Vector2 focusPosition;
        private Vector2 normalPosition;
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
                virtualCamera.m_Lens.OrthographicSize = _focusMode ? focusFOV : normalFOV;
                var focus = transform.localScale.x > 0 ? focusPosition : new Vector2(-focusPosition.x, focusPosition.y);
                cameraTarget.localPosition = _focusMode ? focus : normalPosition;
                if (_focusMode) animator.SetFloat(Horizontal, 0);
            }
        }

        [Space] [Header("Movement")] [SerializeField]
        private float speed = 5f;

        private static readonly int Horizontal = Animator.StringToHash("Horizontal");

        #endregion

        #region Inputs

        /// <summary>
        /// Property for getting the horizontal input from the player.
        /// </summary>
        private float HorizontalInput => Input.GetAxis("Horizontal");
        private float SprintInput => Input.GetKey(KeyCode.LeftShift) ? 2f : 1f;
        private Vector2 MovementInput => new Vector2(HorizontalInput * SprintInput, 0);

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

            normalPosition = cameraTarget.localPosition;
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