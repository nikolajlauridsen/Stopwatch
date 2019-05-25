using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KeyboardHook
{
    public class HookManager
    {
        public int GracePeriod = 50;
        private List<Hook> _hooks = new List<Hook>();
        public bool Listening { get; private set; }
        private ApartmentState _threadApartmentState;

        public HookManager(ApartmentState threadApartmentState = ApartmentState.STA)
        {
            _threadApartmentState = threadApartmentState;
        }

        public void RegisterHook(Key targetKey, Action<Key> keyAction, bool singleFire = true)
        {
            _hooks.Add(new Hook(targetKey, keyAction, singleFire));
        }

        public void RegisterHook(string targetKey, Action<Key> keyAction, bool singleFire = true)
        {
            KeyConverter converter = new KeyConverter();
            Key _key = (Key) converter.ConvertFromString(targetKey);
            RegisterHook(_key, keyAction, singleFire);
        }

        public void RegisterHook(Key targetKey, Key modifier, Action<Key> keyAction, bool singleFire = true)
        {
            _hooks.Add(new Hook(targetKey, modifier, keyAction, singleFire));
        }

        public void DisableHook(Key targetKey)
        {
            _hooks.Remove(_hooks.Find(hook => hook.TargetKey == targetKey));
        }

        public void Listen()
        {
            if (!Listening)
            {
                Listening = true;
                Thread t = new Thread(checkHooks);
                t.IsBackground = true;
                t.SetApartmentState(_threadApartmentState);
                t.Start();
            }
            else
            {
                throw new Exception("HookManager can not listen more than once.");
            }

        }

        public void StopListening()
        {
            Listening = false;
        }

        public void ClearHooks()
        {
            _hooks.Clear();
        }

        private void checkHooks()
        {
            while (Listening)
            {
                Thread.Sleep(GracePeriod);
                foreach (Hook hook in _hooks) {
                    hook.Checkstatus();
                }
            }
        }
    }
}
