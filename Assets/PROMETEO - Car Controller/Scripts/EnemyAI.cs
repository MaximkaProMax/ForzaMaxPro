using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : AI
{
    public Transform target; // Цель, за которой следует автомобиль
    public float targetDistanceThreshold = 5.0f; // Расстояние до цели, при котором автомобиль начинает поворачивать

    void Update()
    {
        Drive();
        CheckDistanceToTarget();
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
        // Вычислите угол между направлением автомобиля и направлением до цели
        float angle = Vector3.Angle(transform.forward, directionToTarget);

        // Если цель находится слева от автомобиля, поверните влево
        if (angle < 0)
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
}
