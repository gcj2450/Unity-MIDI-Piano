using UnityEngine;

///https://catlikecoding.com/unity/tutorials/movement/sliding-a-sphere/
///
namespace CatlikeCodingTutorial
{
    /// <summary>
    /// 场景内放个面片，上面放个球，把脚本挂球身上 运行即可
    /// </summary>
    public class SlidingASphere : MonoBehaviour
    {
        /// <summary>
        /// 最大速度
        /// </summary>
        [SerializeField, Range(0f, 100f)]
        float maxSpeed = 10f;

        /// <summary>
        /// 最大加速度,如果想要球立即听下，把这个值调到最大即可
        /// </summary>
        [SerializeField, Range(0f, 100f)]
        float maxAcceleration = 10f;

        /// <summary>
        /// 反弹力
        /// </summary>
        [SerializeField, Range(0f, 1f)]
        float bounciness = 0.5f;

        /// <summary>
        /// 允许行走区域，要考虑球半径
        /// </summary>
        [SerializeField]
        Rect allowedArea = new Rect(-5f, -5f, 10f, 10f);

        Vector3 velocity;

        void Update()
        {
            Vector2 playerInput;
            playerInput.x = Input.GetAxis("Horizontal");
            playerInput.y = Input.GetAxis("Vertical");
            playerInput = Vector2.ClampMagnitude(playerInput, 1f);

            Vector3 desiredVelocity =
          new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;

            float maxSpeedChange = maxAcceleration * Time.deltaTime;
            velocity.x =
                Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
            velocity.z =
                Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

            Vector3 displacement = velocity * Time.deltaTime;
            Vector3 newPosition = transform.localPosition + displacement;
            if (newPosition.x < allowedArea.xMin)
            {
                newPosition.x = allowedArea.xMin;
                velocity.x = -velocity.x * bounciness;
            }
            else if (newPosition.x > allowedArea.xMax)
            {
                newPosition.x = allowedArea.xMax;
                velocity.x = -velocity.x * bounciness;
            }
            if (newPosition.z < allowedArea.yMin)
            {
                newPosition.z = allowedArea.yMin;
                velocity.z = -velocity.z * bounciness;
            }
            else if (newPosition.z > allowedArea.yMax)
            {
                newPosition.z = allowedArea.yMax;
                velocity.z = -velocity.z * bounciness;
            }
            transform.localPosition = newPosition;
        }
    }
}