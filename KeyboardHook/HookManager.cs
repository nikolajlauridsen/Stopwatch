using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KeyboardHook
{
    public class HookManager
    {
        private List<Hook> _hooks = new List<Hook>();
        private bool _listening;
        public int GracePeriod = 50;

        public void RegisterHook(Key targetKey, Action<Key> keyAction, bool singleFire = true)
        {
            _hooks.Add(new Hook(targetKey, keyAction, singleFire));
        }

        public void DisableHook(Key targetKey)
        {
            _hooks.Remove(_hooks.Find(hook => hook.TargetKey == targetKey));
        }

        public void Listen()
        {
            if (!_listening)
            {
                _listening = true;
                new Thread(checkHooks).Start();
            }
            else
            {
                throw new Exception("HookManager can not listen more than once.");
            }

        }

        public void StopListening()
        {
            _listening = false;
        }

        public void ClearHooks()
        {
            _hooks.Clear();
        }

        private void checkHooks()
        {
            while (_listening)
            {
                Thread.Sleep(GracePeriod);
                foreach (Hook hook in _hooks) {
                    hook.Checkstatus();
                }
            }
        }
    }
}
