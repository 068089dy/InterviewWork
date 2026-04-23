using System;
using UnityEngine;

namespace _3rdTools.Joystick_Pack.Scripts
{
    public class ActiveOr : MonoBehaviour
    {
        public GameObject Target;

        private void OnEnable()
        {
            if (Target != null)
            {
                Target.SetActive(false);
            }
        }

        private void OnDisable()
        {
            if (Target != null)
            {
                Target.SetActive(true);
            }
        }
    }
}