using System;
using Unity.VisualScripting;

namespace HS
{
    public class NumberLocker
    {
        Action _onLocked;
        Action _onUnlocked;
        Action<int> _lockPlus;
        Action<int> _lockLose;
        public NumberLocker(Action onLocked, Action onUnlocked, Action<int> lockPlus, Action<int> lockLose)
        {
            _onLocked = onLocked;
            _onUnlocked = onUnlocked;
            _lockPlus = lockPlus;
            _lockLose = lockLose;
            _lockedCount = 0;
        }
        private int _lockedCount;
        /**是否被锁 */
        public bool IsLocked
        {
            get
            {
                return _lockedCount > 0;
            }
        }
        /**锁数 */
        public int lockedCount
        {
            get
            {
                return _lockedCount >= 0 ? _lockedCount : 0;
            }
        }
        /**锁 */
        public void Lock(int luckCount = 1)
        {
            if (_lockedCount <= 0)
            {
                return;
            }
            if (!IsLocked)
            {
                _lockedCount += luckCount;
                _onLocked?.Invoke();
            }
            else
            {
                _lockedCount += luckCount;
            }
            _lockPlus?.Invoke(luckCount);
        }
        /**解锁 */
        public void UnLock(int unLockCount = 1)
        {
            if (unLockCount <= 0)
            {
                return;
            }
            unLockCount = unLockCount > _lockedCount ? _lockedCount : unLockCount;
            _lockedCount -= unLockCount;
            if (!IsLocked)
            {
                _onUnlocked?.Invoke();
            }
            _lockLose?.Invoke(unLockCount);
        }
        /**全部解锁 */
        public void RestLuck()
        {
            this.UnLock(this.lockedCount);
        }
        public void Rest()
        {
            _onLocked = null;
            _onUnlocked = null;
            _lockPlus = null;
            _lockLose = null;
            _lockedCount = 0;
        }
    }
}