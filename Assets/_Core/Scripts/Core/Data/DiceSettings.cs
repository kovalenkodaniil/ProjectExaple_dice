using UnityEngine;

namespace Core.Data
{
    [CreateAssetMenu(fileName = "New Dice Settings", menuName = "Game Data/Dice Settings", order = 1)]
    public class DiceSettings : ScriptableObject
    {
        [Header("Настройки анимаций в ожидании броска")]
        public float diceFloatDistance;
        public float diceFloatDelta;
        public float diceFloatSpeed;
        public float diceHoldTime;
        
        [Header("Настройки анимаций gjlghsubdfybz")]
        public float dicejumpDisatance;
        public float dicejumpTime;
        
        [Header("Настройки броска")]
        public float throwDisatance;
        public float throwTime;
        public float throwMinImpulse;
        public float throwMaxImpulse;
        public float spinMuliplier;
        public float spinDelay;
        public float ZSpreding;
        public float ZStartVector;
        public float YStartVector;
        public float XSpreding;
        
        [Header("Вектора для поворота граней на нужную сторону")]
        public Vector3 diceEdgeRotationUp;
        public Vector3 diceEdgeRotationDown;
        public Vector3 diceEdgeRotationLeft;
        public Vector3 diceEdgeRotationRight;
        public Vector3 diceEdgeRotationForward;
        public Vector3 diceEdgeRotationBack;
    }
}