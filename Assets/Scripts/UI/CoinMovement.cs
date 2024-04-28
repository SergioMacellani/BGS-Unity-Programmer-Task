using UnityEngine;

namespace BGS.UI
{
    /// <summary>
    /// This class is responsible for the movement of coins in the game when they are gained selling body parts.
    /// </summary>
    public class CoinMovement : MonoBehaviour
    {
        // The target position for the coin to move towards
        private Vector2 _targetPosition;
        // The time elapsed since the coin started moving
        private float time = 0;

        /// <summary>
        /// Initiates the movement of the coin towards the target position.
        /// </summary>
        /// <param name="targetPosition">The target position for the coin to move towards.</param>
        public void Move(Vector2 targetPosition)
        {
            _targetPosition = targetPosition;
        }

        /// <summary>
        /// Updates the position of the coin every frame.
        /// The coin moves towards the target position over time.
        /// If the time elapsed is more than 1 second, the coin game object is destroyed.
        /// </summary>
        private void Update()
        {
            time += Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, _targetPosition, time);
            if (time > 1) Destroy(gameObject);
        }
    }
}