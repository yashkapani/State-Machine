using UnityEngine;
using System.Collections;

namespace AISandbox {
    public class OrientedActor : MonoBehaviour, IActor {
        private const float STEERING_ACCEL      = 500.0f;
        private const float STEERING_LINE_SCALE = 40.0f;
        
        private float MAX_SPEED = 2.0f;

        public Vector2 initialVelocity = Vector2.zero;
        public bool wrapScreen = false;

        [SerializeField]
        private bool _DrawVectors = true;
        public bool DrawVectors {
            get {
                return _DrawVectors;
            }
            set {
                _DrawVectors = value;
                _steering_line.gameObject.SetActive(_DrawVectors);
            }
        }
        public LineRenderer _steering_line;

        private Renderer _renderer;
        private bool _screenWrapX = false;
        private bool _screenWrapY = false;

        private Vector2 _steering = Vector2.zero;
        private Vector2 _acceleration = Vector2.zero;
        private Vector2 _velocity = Vector2.zero;

        private void Start() {
            _renderer = GetComponent<Renderer>();
            _velocity = initialVelocity;
            DrawVectors = _DrawVectors;
        }

        public void SetInput( float x_axis, float y_axis ) {
            _steering = Vector2.ClampMagnitude( new Vector2(x_axis, y_axis), 1.0f );
            _acceleration = _steering * STEERING_ACCEL;
        }

        public float MaxSpeed {
            get { return MAX_SPEED; }
            set { MAX_SPEED = value; }
        }

        public Vector2 Velocity {
            get { return _velocity; }
        }

        public Vector2 Position {
            get { return transform.position; }
        }

        private Vector3 ScreenWrap() {
            Vector3 position = transform.position;
            if( wrapScreen ) {
                if( _renderer.isVisible ) {
                    _screenWrapX = false;
                    _screenWrapY = false;
                    return position;
                } else {
                    Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
                    if( !_screenWrapX && (viewportPosition.x > 1 || viewportPosition.x < 0) ) {
                        position.x = -position.x;
                        _screenWrapX = true;
                    }
                    if( !_screenWrapY && (viewportPosition.y > 1 || viewportPosition.y < 0 ) ) {
                        position.y = -position.y;
                        _screenWrapY = true;
                    }
                }
            }
            return position;
        }

        private void Update() {
            Vector3 position = ScreenWrap();
            _velocity += _acceleration * Time.deltaTime;
            _velocity = Vector2.ClampMagnitude(_velocity, MAX_SPEED);
            position += (Vector3)(_velocity * Time.deltaTime);
            transform.position = position;
            transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.Normalize(_velocity));

            _steering_line.transform.rotation = Quaternion.identity;
            _steering_line.SetPosition( 1, _steering * STEERING_LINE_SCALE );

            // The steering is reset every frame so SetInput() must be called every frame for continuous steering.
            _steering = Vector2.zero;
            _acceleration = Vector2.zero;
        }
    }
}