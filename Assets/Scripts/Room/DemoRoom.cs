using UnityEngine;

namespace Room
{
    public class DemoRoom : Room
    {
        public override void Enter()
        {
            Debug.Log("Enter");
        }

        public override void Exit()
        {
            Debug.Log("Exit");
        }
    }
}