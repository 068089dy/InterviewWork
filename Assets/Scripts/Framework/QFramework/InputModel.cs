using CoreAssembly;
using UnityEngine;

namespace DefaultNamespace.Core.Model
{
    public interface IInputModel : IModel
    {
        public Vector2 MoveDirection { get; set; }
    }

    public class InputModel: AbstractModel, IInputModel
    {
        public Vector2 MoveDirection { get; set; }
        protected override void OnInit()
        {
        }
    }
}