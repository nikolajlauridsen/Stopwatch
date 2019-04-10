using System;
using System.Windows.Input;


namespace KeyboardHook
{
    internal class Hook
    {
        internal Key TargetKey;
        internal Key? Modifier = null;
        private Action<Key> _keyAction;
        private bool _singleFire;
        private bool _pressed = false;

        internal Hook(Key targeKey, Action<Key> keyAction, bool singleFire = true)
        {
            TargetKey = targeKey;
            _keyAction = keyAction;
            _singleFire = singleFire;
        }

        internal Hook(Key targetKey, Key modifier, Action<Key> keyAction, bool singleFire = true) : this(targetKey,
            keyAction, singleFire)
        {
            Modifier = modifier;
        }

        internal void Checkstatus()
        {
            if (Keyboard.IsKeyDown(TargetKey) && (Modifier == null || Keyboard.IsKeyDown((Key)Modifier)) && (!_pressed || !_singleFire))
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
