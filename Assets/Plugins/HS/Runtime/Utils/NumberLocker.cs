//using System;

//namespace HS
//{
//    public class NumberLocker
//    {
//        private Action onLocked;
//        private Action onUnlocked;
//        private Action<int> lockPlus;
//        private Action<int> lockLose;
//        public NumberLocker(Action onLocked, Action onUnlocked, Action<int> lockPlus, Action<int> lockLose)
//        {
//            this.onLocked = onLocked;
//            this.onUnlocked = onUnlocked;
//            this.lockPlus = lockPlus;
//            this.lockLose = lockLose;
//            lockedCount = 0;
//        }
//        private int lockedCount;
//        /**是否被锁 */
//        public bool IsLocked
//        {
//            get
//            {
//                return lockedCount > 0;
//            }
//        }
//        /**锁数 */
//        public int LockedCount
//        {
//            get
//            {
//                return lockedCount >= 0 ? lockedCount : 0;
//            }
//        }
//        /**锁 */
//        public void Lock(int luckCount = 1)
//        {
//            if (lockedCount <= 0)
//            {
//                return;
//            }
//            if (!IsLocked)
//            {
//                lockedCount += luckCount;
//                onLocked?.Invoke();
//            }
//            else
//            {
//                lockedCount += luckCount;
//            }
//            lockPlus?.Invoke(luckCount);
//        }
//        /**解锁 */
//        public void UnLock(int unLockCount = 1)
//        {
//            if (unLockCount <= 0)
//            {
//                return;
//            }
//            unLockCount = unLockCount > lockedCount ? lockedCount : unLockCount;
//            lockedCount -= unLockCount;
//            if (!IsLocked)
//            {
//                onUnlocked?.Invoke();
//            }
//            lockLose?.Invoke(unLockCount);
//        }
//        /**全部解锁 */
//        public void RestLuck()
//        {
//            this.UnLock(this.lockedCount);
//        }
//        public void Rest()
//        {
//            onLocked = null;
//            onUnlocked = null;
//            lockPlus = null;
//            lockLose = null;
//            lockedCount = 0;
//        }
//    }
//}