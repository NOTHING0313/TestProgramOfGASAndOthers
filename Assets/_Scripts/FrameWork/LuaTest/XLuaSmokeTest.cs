using UnityEngine;
using XLua;

namespace HotUpdate
{
    /// <summary>
    /// 验证xLua运行环境。
    /// </summary>
    public sealed class XLuaSmokeTest : MonoBehaviour
    {
        private LuaEnv _luaEnv;

        private void Start()
        {
            _luaEnv = new LuaEnv();
            _luaEnv.DoString("CS.UnityEngine.Debug.Log('XLua Smoke Test Success')");
        }

        private void Update() => _luaEnv?.Tick();

        private void OnDestroy()
        {
            _luaEnv?.Dispose();
            _luaEnv = null;
        }
    }
}