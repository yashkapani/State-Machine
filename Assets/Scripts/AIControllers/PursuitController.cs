using UnityEngine;
using System.Collections;

namespace AISandbox {
    [RequireComponent(typeof (IActor))]
    public class PursuitController : MonoBehaviour {
        private IActor _actor;

        //public SimpleActor _target_actor;

        private void Awake() {
            _actor = GetComponent<IActor>();
        }

        private void FixedUpdate() {
            float target_speed = _actor.Velocity.magnitude;
            //float distance_to_target = Vector2.Distance(_target_actor.transform.position, transform.position);

            // Pursuing the target
            //Vector2 desired_velocity = (Vector2)_target_actor.transform.position - (Vector2)gameObject.transform.position;
            //Vector2 steering = desired_velocity - _actor.Velocity;

            // Pass all parameters to the character control script.
            //_actor.SetInput( steering.x, steering.y);
        }
    }
}