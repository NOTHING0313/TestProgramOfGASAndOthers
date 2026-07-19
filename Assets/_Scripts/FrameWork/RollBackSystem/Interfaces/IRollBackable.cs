
namespace RollBackSystem
{
    public interface IRollBackable
    {
        /// <summary>
        /// 在指定的本地逻辑帧记录并返回一个快照
        /// </summary>
        /// <param name="localizedLogicFrameCount">指定的本地逻辑帧</param>
        /// <returns></returns>
        public ISnapShot SnapShot(int localizedLogicFrameCount);
        /// <summary>
        /// 当检测到预测错误时，用给定 snapShot 把对象恢复到某个历史状态，并从错误开始帧重跑到当前帧
        /// </summary>
        /// <param name="snapShot"></param>
        /// <param name="errorStartLocalizedLogicFrameCount"></param>
        /// <param name="currentLocalizedLogicFrameCount"></param>
        public void RollBack(ISnapShot snapShot, int errorStartLocalizedLogicFrameCount, int currentLocalizedLogicFrameCount);
    }
}
