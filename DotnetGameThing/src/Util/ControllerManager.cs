using Breakout.Window;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Util
{
    internal class ControllerManager
    {
        private int id;

        public ControllerManager(int id) {
            this.id = id;
        }

        public bool IsAvailable() => Raylib.IsGamepadAvailable(id);

        public float GetLeftStickX()
        {
            if (Raylib.IsGamepadAvailable(id)) 
                return Raylib.GetGamepadAxisMovement(id, GamepadAxis.GAMEPAD_AXIS_LEFT_X);
            else return 0f;
        }
        public float GetLeftStickY() 
        {
            if (Raylib.IsGamepadAvailable(id)) 
                return Raylib.GetGamepadAxisMovement(id, GamepadAxis.GAMEPAD_AXIS_LEFT_Y);
            else return 0f;
        }
        public float GetRightStickX()
        {
            if (Raylib.IsGamepadAvailable(id))
                return Raylib.GetGamepadAxisMovement(id, GamepadAxis.GAMEPAD_AXIS_RIGHT_X);
            else return 0f;
        }
        public float GetRightStickY()
        {
            if (Raylib.IsGamepadAvailable(id))
                return Raylib.GetGamepadAxisMovement(id, GamepadAxis.GAMEPAD_AXIS_RIGHT_Y);
            else return 0f;
        }

        public bool IsKeyPressed(GamepadButton button)
        {
            if (Raylib.IsGamepadAvailable(id))
                return Raylib.IsGamepadButtonPressed(id, button);
            else return false;
        }
    }
}
