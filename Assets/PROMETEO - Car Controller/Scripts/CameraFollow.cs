using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform carTransform;
	[Range(1, 10)]
	public float followSpeed = 2;
	[Range(1, 10)]
	public float lookSpeed = 5;
	private float distance;
	private float height;

	void Start(){
		distance = Vector3.Distance(transform.position, carTransform.position);
		height = transform.position.y;
	}

	void FixedUpdate()
	{
		//Move to car
		Vector3 targetPos = carTransform.position - carTransform.forward * distance;
		targetPos.y = carTransform.position.y + height;
		transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);

		//Look at car
		Vector3 lookDirection = carTransform.position - transform.position;
		Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
		float clampedZ = Mathf.Clamp(rotation.eulerAngles.z, -1f, 1f); // clamp the z rotation between -1 and 1
		rotation = Quaternion.Euler(5f, rotation.eulerAngles.y, clampedZ);
		transform.rotation = Quaternion.Lerp(transform.rotation, rotation, lookSpeed * Time.deltaTime);
	}
}













/* using System.Collections; // СТАРЫЙ СКРИПТ НА ВСЯКИЙ СЛУЧАЙ
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform carTransform;
	[Range(1, 10)]
	public float followSpeed = 2;
	[Range(1, 10)]
	public float lookSpeed = 5;
	Vector3 initialCameraPosition;
	Vector3 initialCarPosition;
	Vector3 absoluteInitCameraPosition;

	void Start(){
		initialCameraPosition = gameObject.transform.position;
		initialCarPosition = carTransform.position;
		absoluteInitCameraPosition = initialCameraPosition - initialCarPosition;
	}

	void FixedUpdate()
	{
		//Look at car
		Vector3 _lookDirection = (new Vector3(carTransform.position.x, carTransform.position.y, carTransform.position.z)) - transform.position;
		Quaternion _rot = Quaternion.LookRotation(_lookDirection, Vector3.up);
		transform.rotation = Quaternion.Lerp(transform.rotation, _rot, lookSpeed * Time.deltaTime);

		//Move to car
		Vector3 _targetPos = absoluteInitCameraPosition + carTransform.transform.position;
		transform.position = Vector3.Lerp(transform.position, _targetPos, followSpeed * Time.deltaTime);

	}

} */
