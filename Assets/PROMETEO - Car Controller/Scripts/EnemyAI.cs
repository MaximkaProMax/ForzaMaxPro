using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : AI
{
    public Transform target; // Цель, за которой следует автомобиль
    public float targetDistanceThreshold = 5.0f; // Расстояние до цели, при котором автомобиль начинает поворачивать
    public float maxSteeringAngle = 30.0f;

    void Update()
    {
        Drive();
        CheckDistanceToTarget();
        RayCasting();
    }

    void Drive()
    {
        // Установите максимальную скорость и ускорение
        frontLeftCollider.motorTorque = maxSpeed * accelerationMultiplier;
        frontRightCollider.motorTorque = maxSpeed * accelerationMultiplier;
    }

    void CheckDistanceToTarget()
    {
        // Проверьте расстояние до цели
        if (Vector3.Distance(transform.position, target.position) < targetDistanceThreshold)
        {
            // Если автомобиль близко к цели, начните поворачивать
            Turn();
        }
    }

    void Turn()
    {
        // Вычислите направление до цели
        Vector3 directionToTarget = target.position - transform.position;
        directionToTarget.y = 0; // Игнорируем высоту

        // Вычислите векторное произведение между направлением автомобиля и направлением до цели
        float crossProduct = Vector3.Cross(transform.forward, directionToTarget).y;

        // Если цель находится слева от автомобиля, поверните влево
        if (crossProduct < 0)
        {
            frontLeftCollider.steerAngle = -maxSteeringAngle;
            frontRightCollider.steerAngle = -maxSteeringAngle;
        }
        // Если цель находится справа от автомобиля, поверните вправо
        else
        {
            frontLeftCollider.steerAngle = maxSteeringAngle;
            frontRightCollider.steerAngle = maxSteeringAngle;
        }
    }

    void RayCasting()
    {
        // Создайте лучи слева, справа и спереди от автомобиля
        Ray leftRay = new Ray(transform.position, -transform.right);
        Ray rightRay = new Ray(transform.position, transform.right);
        Ray frontRay = new Ray(transform.position, transform.forward);

        // Используйте RaycastHit для хранения информации о том, что пересек луч
        RaycastHit leftHit;
        RaycastHit rightHit;
        RaycastHit frontHit;

        // Определите дистанцию для лучей
        float rayDistance = 15.0f;

        // Нарисуйте лучи для визуализации в редакторе Unity
        Debug.DrawRay(leftRay.origin, leftRay.direction * rayDistance, Color.red);
        Debug.DrawRay(rightRay.origin, rightRay.direction * rayDistance, Color.red);
        Debug.DrawRay(frontRay.origin, frontRay.direction * rayDistance, Color.blue);

        // Выполните лучевое трассирование и проверьте, пересек ли луч какой-либо объект
        if (Physics.Raycast(leftRay, out leftHit, rayDistance))
        {
            // Если левый луч пересекает объект, поверните вправо
            frontLeftCollider.steerAngle = maxSteeringAngle;
            frontRightCollider.steerAngle = maxSteeringAngle;
        }
        else if (Physics.Raycast(rightRay, out rightHit, rayDistance))
        {
            // Если правый луч пересекает объект, поверните влево
            frontLeftCollider.steerAngle = -maxSteeringAngle;
            frontRightCollider.steerAngle = -maxSteeringAngle;
        }

        // Если передний луч пересекает объект, начните плавно поворачивать
        if (Physics.Raycast(frontRay, out frontHit, rayDistance))
        {
            // Вычислите направление до цели
            Vector3 directionToTarget = frontHit.point - transform.position;
            directionToTarget.y = 0; // Игнорируем высоту

            // Вычислите векторное произведение между направлением автомобиля и направлением до цели
            float crossProduct = Vector3.Cross(transform.forward, directionToTarget).y;

            // Если цель находится слева от автомобиля, поверните влево
            if (crossProduct < 0)
            {
                frontLeftCollider.steerAngle -= Mathf.Lerp(-frontLeftCollider.steerAngle, maxSteeringAngle, Time.deltaTime);
                frontRightCollider.steerAngle -= Mathf.Lerp(-frontRightCollider.steerAngle, maxSteeringAngle, Time.deltaTime);
            }
            // Если цель находится справа от автомобиля, поверните вправо
            else
            {
                frontLeftCollider.steerAngle += Mathf.Lerp(frontLeftCollider.steerAngle, maxSteeringAngle, Time.deltaTime);
                frontRightCollider.steerAngle += Mathf.Lerp(frontRightCollider.steerAngle, maxSteeringAngle, Time.deltaTime);
            }
        }
    }
}
