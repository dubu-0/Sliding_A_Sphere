using System;
using UnityEngine;

public class Sphere : MonoBehaviour
{
	[SerializeField, Range(0f, 100f)] private float _maxSpeed = 10f;
	[SerializeField, Range(0f, 100f)] private float _maxAcceleration = 10f;
	
	private const string HorizontalAxis = "Horizontal";
	private const string VerticalAxis = "Vertical";

	private Rigidbody _body;
	private Vector3 _desiredVelocity;

	private void Awake()
	{
		_body = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		var input = ReadInput();
		_desiredVelocity = new Vector3(input.x, 0f, input.y) * _maxSpeed;
	}

	private void FixedUpdate()
	{
		_body.velocity = CalculateVelocity();
	}

	private Vector2 ReadInput()
	{
		Vector2 input;
		input.x = Input.GetAxis(HorizontalAxis);
		input.y = Input.GetAxis(VerticalAxis);
		input = Vector2.ClampMagnitude(input, 1f);
		return input;
	}

	private Vector3 CalculateVelocity()
	{
		var maxSpeedChange = _maxAcceleration * Time.deltaTime;
		var velocity = _body.velocity;
		velocity.x = Mathf.MoveTowards(velocity.x, _desiredVelocity.x, maxSpeedChange);
		velocity.z = Mathf.MoveTowards(velocity.z, _desiredVelocity.z, maxSpeedChange);
		return velocity;
	}
}
