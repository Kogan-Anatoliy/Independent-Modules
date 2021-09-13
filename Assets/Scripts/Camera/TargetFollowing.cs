/*
 * author: Anatolii Kogan
 * e-mail: kogan.1anatoli@gmail.com
 */
using UnityEngine;

namespace DemoProject
{
    public class TargetFollowing : MonoBehaviour
    {
        public float DistanceToTarget { get; private set; } = 0.0f;

        public Transform target = null;
        public float followSpeed = 1.0f;

        public Vector3 offset = Vector3.zero;
        public float maxFollowingOffset = 3.0f;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            transform.position = target.position + offset;
        }

        private void LateUpdate()
        {
            Following();
        }

        private void Following()
        {
            DistanceToTarget = Vector3.Distance(transform.position, target.position + offset);
            if (DistanceToTarget <= maxFollowingOffset)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position + offset, followSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position + offset, followSpeed * DistanceToTarget * Time.deltaTime);
            }
        }        
    }
}