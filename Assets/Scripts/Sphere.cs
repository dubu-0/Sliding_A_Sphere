using UnityEngine;

public class Sphere : MonoBehaviour
{
	[SerializeField, Range(0f, 100f)] private float _maxSpeed = 10f;
	[SerializeField, Range(0f, 100f)] private float _maxAcceleration = 10f;
	[SerializeField, Range(0f, 3f)] private float _bounciness = 0.5f;
	[SerializeField] private Rect _allowedArea = new Rect(-4.5f, -4.5f, 9f, 9f);
	
	private const string HorizontalAxis = "Horizontal";
	private const string VerticalAxis = "Vertical";

	private Vector3 _velocity;
	
	private void Update()
	{
		Vector2 input;
		input.x = Input.GetAxis(HorizontalAxis);
		input.y = Input.GetAxis(VerticalAxis);
		input = Vector2.ClampMagnitude(input, 1f);
		
		var desiredVelocity = new Vector3(input.x, 0f, input.y) * _maxSpeed;
		var maxSpeedChange = _maxAcceleration * Time.deltaTime;

		_velocity.x = Mathf.MoveTowards(_velocity.x, desiredVelocity.x, maxSpeedChange);
		_velocity.z = Mathf.MoveTowards(_velocity.z, desiredVelocity.z, maxSpeedChange);

		var displacement = _velocity * Time.deltaTime;
		var newPosition = transform.position + displacement;

		if (newPosition.x < _allowedArea.xMin)
		{
			newPosition.x = _allowedArea.xMin;
			_velocity.x = -_velocity.x * _bounciness;
		}
		else if (newPosition.x > _allowedArea.xMax)
		{
			newPosition.x = _allowedArea.xMax;
			_velocity.x = -_velocity.x * _bounciness;
		}
		if (newPosition.z < _allowedArea.yMin)
		{
			newPosition.z = _allowedArea.yMin;
			_velocity.z = -_velocity.z * _bounciness;
		}
		else if (newPosition.z > _allowedArea.yMax)
		{
			newPosition.z = _allowedArea.yMax;
			_velocity.z = -_velocity.z * _bounciness;
		}
		
		transform.position = newPosition;
	}
}
