using UnityEngine;

public class Sphere : MonoBehaviour
{
	[SerializeField, Range(0f, 100f)] private float _maxSpeed = 10f;
	[SerializeField, Range(0f, 100f)] private float _maxAcceleration = 10f;
	[SerializeField, Range(0f, 100f)] private float _maxAirAcceleration = 1f;
	[SerializeField, Range(0f, 90f)] private float _maxGroundSlope = 60f;
	[SerializeField, Range(0f, 10f)] private float _maxJumpHeight = 2f;
	[SerializeField, Range(0, 5)] private int _maxAirJumps = 1;
	
	private const string HorizontalAxis = "Horizontal";
	private const string VerticalAxis = "Vertical";
	private const string JumpAxis = "Jump";

	private Rigidbody _body;
	private Vector3 _velocity;
	private Vector3 _desiredVelocity;
	private bool _jumpRequired;
	private bool _onGround;
	private int _airJumps;
	private float _groundNormalY;

	private void Awake()
	{
		_body = GetComponent<Rigidbody>();
		
		// Why not Mathf.Cos(...)? Because cos(-a) = cos(a) and I need that minus
		_groundNormalY = Mathf.Sin(Mathf.Deg2Rad * (90f - _maxGroundSlope));
	}

	private void Update()
	{
		var input = ReadInput();
		_desiredVelocity = new Vector3(input.x, 0f, input.y) * _maxSpeed;
		_jumpRequired |= Input.GetButtonDown(JumpAxis);
	}

	private void FixedUpdate()
	{
		CalculateVelocity();

		if (_jumpRequired && (_onGround || _airJumps < _maxAirJumps))
		{
			Jump();
			_jumpRequired = false;
			_airJumps++;
		}

		if (_onGround) 
			_airJumps = 0;
		
		_body.velocity = _velocity;
		_onGround = false;
	}
	
	private void OnCollisionStay(Collision collisionInfo)
	{
		EvaluateCollision(collisionInfo);
	}

	private Vector2 ReadInput()
	{
		Vector2 input;
		input.x = Input.GetAxis(HorizontalAxis);
		input.y = Input.GetAxis(VerticalAxis);
		input = Vector2.ClampMagnitude(input, 1f);
		return input;
	}

	private void CalculateVelocity()
	{
		var maxSpeedChange = _onGround ? _maxAcceleration : _maxAirAcceleration;
		maxSpeedChange *= Time.deltaTime;

		_velocity = _body.velocity;
		_velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, maxSpeedChange);
		_velocity.z = Mathf.MoveTowards(_velocity.z, _desiredVelocity.z, maxSpeedChange);
	}

	private void Jump()
	{
		var jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * _maxJumpHeight);
		jumpSpeed = Mathf.Max(jumpSpeed - _velocity.y, 0f);
		_velocity.y += jumpSpeed;
	}

	private void EvaluateCollision(Collision collision)
	{
		for (var i = 0; i < collision.contactCount; i++)
		{
			var contact = collision.GetContact(i);
			_onGround |= contact.normal.y >= _groundNormalY;
		}
	}
}
