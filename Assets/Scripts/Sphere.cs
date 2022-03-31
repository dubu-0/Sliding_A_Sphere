using System;
using UnityEngine;

public class Sphere : MonoBehaviour
{
	[SerializeField, Range(0f, 100f)] private float _maxSpeed = 10f;
	[SerializeField, Range(0f, 100f)] private float _maxAcceleration = 10f;
	[SerializeField, Range(0f, 10f)] private float _maxJumpHeight = 2f;
	
	private const string HorizontalAxis = "Horizontal";
	private const string VerticalAxis = "Vertical";
	private const string JumpAxis = "Jump";

	private Rigidbody _body;
	private Vector3 _velocity;
	private Vector3 _desiredVelocity;
	private bool _jumpRequired;

	private void Awake()
	{
		_body = GetComponent<Rigidbody>();
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

		if (_jumpRequired)
		{
			Jump();
			_jumpRequired = false;
		}
		
		_body.velocity = _velocity;
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
		var maxSpeedChange = _maxAcceleration * Time.deltaTime;
		_velocity = _body.velocity;
		_velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, maxSpeedChange);
		_velocity.z = Mathf.MoveTowards(_velocity.z, _desiredVelocity.z, maxSpeedChange);
	}

	private void Jump()
	{
		var jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * _maxJumpHeight);
		_velocity.y += jumpSpeed;
	}
}