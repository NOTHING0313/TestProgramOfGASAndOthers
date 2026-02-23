using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace OdinLearningTest
{
    public class DemoWindow : OdinMenuEditorWindow
    {
        private static DemoWindow _instance;
        public static DemoWindow Instance => _instance;
        public SOItemConfigue itemTable;
        public ContainerForConfigue containerTable;
        [MenuItem("配置表窗口/配置表")]
        private static void OpenWindow()
        {
            _instance = GetWindow<DemoWindow>();
            _instance.WindowPadding = default;
            _instance.minSize = new Vector2(600, 400);
            _instance.MenuWidth = 800;
            _instance.titleContent = new GUIContent("信息配置表");
            _instance.Show();
        }
        protected override void OnImGUI()
        {
            base.OnImGUI();
            if (_instance == null)
                _instance = this;
            var select = Selection.activeObject as SOItemConfigue;
            if (select == null)
            {
                Debug.LogError("Wrong Type");
                return;
            }
            if (select != itemTable)
            {
                itemTable = select;
                _instance.ForceMenuTreeRebuild();//帧调用重建菜单将导致交互失灵
            }
        }
        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree();
            if (itemTable == null)
                itemTable = Selection.activeObject as SOItemConfigue;
            string mainMenuName = itemTable ? $"已选中{itemTable.name}" : "未选中";
            tree.Add(mainMenuName, new ContainerForConfigue() { isValid = _instance, SOItemConfigue = itemTable });
            //加入枝节
            if (_instance && itemTable)
            {
                int i = 0;
                foreach (SOSingle temp in itemTable.friend)
                {
                    if (temp)
                    {
                        tree.Add($"{mainMenuName}/--单元{temp.rename}{i++}", new ContainerForSingle() { parent = itemTable, _item = temp });
                    }
                }
            }
            //加入关于
            tree.Add("关于", new WindowAbout());
            return tree;
        }
        public void OnValidate()
        {
            if (_instance == null)
                _instance = this;
        }
    }
    [Serializable]
    public class ContainerForConfigue
    {
        [HorizontalGroup("水平")]

        [VerticalGroup("水平/数据")]
        [LabelText("是否有效")]
        public bool isValid;

        [VerticalGroup("水平/数据")]
        [LabelText("警报")]
        [InfoBox(message: "当前未配置任何配置表", InfoMessageType = InfoMessageType.Error, VisibleIf = "@!isValid")]
        //"@isValid"等价于nameof(isValid)，它出现在像 [ShowIf] [HideIf] [EnableIf] [DisableIf] [InfoBox] 这类 Odin 的“条件”参数里时，代表一段要被 Odin 在编辑器里执行的表达式
        //表达式里可以直接访问当前对象的字段/属性/方法
        //nameof(valid) 是 C# 语言特性，它会在编译期变成字符串 "valid"
        //还可以更加复杂，"@isValid && !string.IsNullOrEmpty(infoBox)"
        [ShowIf("@!isValid")]
        public string infoBox = "当前未配置任何配置表";
        [LabelText("配置表"), InlineEditor]
        public SOItemConfigue SOItemConfigue;

        [HorizontalGroup("水平", width: 100)]
        [VerticalGroup("水平/控件"), Button(name: "添加", Icon = SdfIconType.Fan, ButtonHeight = 50)]
        public void CreateNewInstance()
        {
            var SOInstance = ScriptableObject.CreateInstance<SOSingle>();
            SOInstance.name = SOInstance.rename = "新建文件";
            SOItemConfigue?.friend.Add(SOInstance);
            AssetDatabase.AddObjectToAsset(SOInstance, SOItemConfigue);
            AssetDatabase.SaveAssets();
        }
        [VerticalGroup("水平/控件"), Button(name: "刷新", Icon = SdfIconType.Record2, ButtonHeight = 50)]
        public void Refresh()
        {
            List<SOSingle> contain = new();
            for (int i = 0; i < SOItemConfigue.friend.Count; i++)
            {
                if (SOItemConfigue.friend[i] == null)
                    SOItemConfigue.friend.RemoveAt(i--);
                else if (contain.Contains(SOItemConfigue.friend[i]))
                    SOItemConfigue.friend.RemoveAt(i--);
                else
                    contain.Add(SOItemConfigue.friend[i]);
            }
            contain = null;
            AssetDatabase.SaveAssets();
            DemoWindow.Instance.ForceMenuTreeRebuild();
        }
        [VerticalGroup("水平/控件"), Button(name: "关闭", Icon = SdfIconType.X, ButtonHeight = 50)]
        public void Close()
        {
            DemoWindow.Instance?.Close();
        }
    }
    [Serializable]
    public class ContainerForSingle
    {
        [HorizontalGroup("水平")]

        [VerticalGroup("水平/配置表"), ReadOnly, LabelText("配置表列表")]
        public SOItemConfigue parent;
        [VerticalGroup("水平/配置表"), InlineEditor, LabelText("数据")]
        public SOSingle _item;
        [HorizontalGroup("水平", width: 100)]
        [VerticalGroup("水平/控件"), Button(name: "刷新", Icon = SdfIconType.Record2, ButtonHeight = 50)]
        public void Refresh()
        {
            _item.name = _item.rename;
            AssetDatabase.SaveAssets();
        }
        [VerticalGroup("水平/控件"), Button(name: "删除", Icon = SdfIconType.X, ButtonHeight = 50)]
        public void Delete()
        {
            _item.DeleteSelf();
        }

    }
    [Serializable]
    public class WindowAbout
    {
        [Title("Test Odin Window")]
        [LabelText("学习日期"), ReadOnly]
        public string date = "1.23";
    }
}