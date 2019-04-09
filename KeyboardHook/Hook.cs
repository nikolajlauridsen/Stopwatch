using System;
using System.Windows.Input;


namespace KeyboardHook
{
    internal class Hook
    {
        internal Key TargetKey;
        private Action<Key> _keyAction;
        private bool _singleFire;
        private bool _pressed = false;

        internal Hook(Key targeKey, Action<Key> keyAction, bool singleFire = true)
        {
            TargetKey = targeKey;
            _keyAction = keyAction;
            _singleFire = singleFire;
        }

        internal void Checkstatus()
        {
            if (Keyboard.IsKeyDown(TargetKey) && (!_pressed || !_singleFire))
            {
                _keyAction(TargetKey);
                _pressed = true;
            } else if (_pressed && Keyboard.IsKeyUp(TargetKey))
            {
                _pressed = false;
            }
        }

    }
}
