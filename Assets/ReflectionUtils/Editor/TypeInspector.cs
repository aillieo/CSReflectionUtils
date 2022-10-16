using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace AillieoUtils.CSReflectionUtils.Editor
{
    public class TypeInspector : EditorWindow
    {
        [MenuItem("AillieoUtils/ReflectionUtils/TypeInspector")]
        public static void Open()
        {
            GetWindow<TypeInspector>("Type Inspector");
        }

        private static Assembly[] assemblies;
        private string assemblyNameFilter = "AillieoUtils.ReflectionUtils.Editor";
        private string[] assemblyNames;
        private int assemblySelectedValue = -1;
        private string assemblySelectedStr;

        private Type[] types;
        private string typeNameFilter = "TypeInspector";
        private string[] typeNames;
        private int typeSelectedValue = -1;
        private string typeSelectedStr;

        private BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        private MemberTypes memberTypes = MemberTypes.All;

        private MemberInfo[] members;
        private string memberNameFilter = "Open";
        private string[] memberNames;

        private Vector2 scrollPosition;
        [SerializeField]
        private TreeViewState markViewState;
        private MemberInfoTreeView treeView;

        private int assemblySelected
        {
            get => assemblySelectedValue;
            set
            {
                if (assemblySelectedValue != value)
                {
                    assemblySelectedValue = value;
                    assemblySelectedStr = assemblyNames[assemblySelectedValue];
                    Reload(4);
                }
            }
        }

        private int typeSelected
        {
            get => typeSelectedValue;
            set
            {
                if (typeSelectedValue != value)
                {
                    typeSelectedValue = value;
                    typeSelectedStr = typeNames[typeSelectedValue];
                    Reload(2);
                }
            }
        }

        private void Awake()
        {
            Reload(6);

            if (markViewState == null)
            {
                markViewState = new TreeViewState();
            }

            treeView = new MemberInfoTreeView(markViewState);
        }

        private void OnGUI()
        {
            DrawAssemblies();

            DrawTypes();

            DrawMembers();
        }

        private void Reload(int level)
        {
            // 重新获取所有Assembly
            if (level >= 6)
            {
                // UnityEngine.Debug.Log($"Reload({6})");
                assemblies = AppDomain.CurrentDomain.GetAssemblies();
            }

            // 根据filter筛选Assembly
            if (level >= 5)
            {
                // UnityEngine.Debug.Log($"Reload({5})");
                if (string.IsNullOrWhiteSpace(assemblyNameFilter))
                {
                    assemblyNames = assemblies
                        .Select(a => a.GetName().Name)
                        .ToArray();
                }
                else
                {
                    assemblyNames = assemblies
                        .Select(a => a.GetName().Name)
                        .Where(n => n.ContainsIgnoreCase(assemblyNameFilter))
                        .ToArray();
                }

                int index = Array.IndexOf(assemblyNames, assemblySelectedStr);
                if (index < 0)
                {
                    index = 0;
                }

                assemblySelected = index;
            }

            // 重新获取所有Type
            if (level >= 4)
            {
                // UnityEngine.Debug.Log($"Reload({4})");
                Assembly selected = assemblies.FirstOrDefault(ass => ass.GetName().Name == assemblySelectedStr);
                if (selected != null)
                {
                    types = selected.GetTypes();
                }
                else
                {
                    types = Array.Empty<Type>();
                }
            }

            // 根据filter筛选Type
            if (level >= 3)
            {
                // UnityEngine.Debug.Log($"Reload({3})");
                if (string.IsNullOrWhiteSpace(typeNameFilter))
                {
                    typeNames = types
                        .Select(t => t.FullName)
                        .ToArray();
                }
                else
                {
                    typeNames = types
                        .Select(t => t.FullName)
                        .Where(n => n.ContainsIgnoreCase(typeNameFilter))
                        .ToArray();
                }

                int index = Array.IndexOf(typeNames, typeSelectedStr);
                if (index < 0)
                {
                    index = 0;
                }

                typeSelected = index;
            }

            // 重新获取所有Member
            if (level >= 2)
            {
                // UnityEngine.Debug.Log($"Reload({2})");
                Type selected = types.FirstOrDefault(t => t.FullName == typeSelectedStr);
                if (selected != null)
                {
                    members = selected.GetMembers(bindingFlags).Where(m => (m.MemberType & memberTypes) != 0).ToArray();
                }
                else
                {
                    members = Array.Empty<MemberInfo>();
                }
            }

            // 根据filter筛选Member
            if (level >= 1)
            {
                // UnityEngine.Debug.Log($"Reload({1})");
                if (string.IsNullOrWhiteSpace(memberNameFilter))
                {
                    memberNames = members
                        .Select(m => m.Name)
                        .ToArray();
                }
                else
                {
                    memberNames = members
                        .Select(m => m.Name)
                        .Where(n => n.ContainsIgnoreCase(memberNameFilter))
                        .ToArray();
                }
            }
        }

        private void DrawAssemblies()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Assembly", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            assemblyNameFilter = EditorGUILayout.TextField(GUIContent.none, assemblyNameFilter);
            if (EditorGUI.EndChangeCheck())
            {
                Reload(5);
            }

            assemblySelected = EditorGUILayout.Popup(GUIContent.none, assemblySelected, assemblyNames);
            EditorGUILayout.EndVertical();
        }

        private void DrawTypes()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Type", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            typeNameFilter = EditorGUILayout.TextField(GUIContent.none, typeNameFilter);
            if (EditorGUI.EndChangeCheck())
            {
                Reload(3);
            }

            typeSelected = EditorGUILayout.Popup(GUIContent.none, typeSelected, typeNames);
            EditorGUILayout.EndVertical();
        }

        private void DrawMembers()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Member filters", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            bindingFlags = (BindingFlags)EditorGUILayout.EnumFlagsField("BindingFlags", bindingFlags);
            memberTypes = (MemberTypes)EditorGUILayout.EnumFlagsField("MemberTypes", memberTypes);
            if (EditorGUI.EndChangeCheck())
            {
                Reload(2);
            }

            EditorGUI.BeginChangeCheck();
            memberNameFilter = EditorGUILayout.TextField("Members", memberNameFilter);
            if (EditorGUI.EndChangeCheck())
            {
                Reload(1);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField("Members", EditorStyles.boldLabel);

            treeView.SetData(members);
            if (memberNames != null && memberNames.Length > 0)
            {
                //scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

                //for (int i = 0; i < memberNames.Length; ++i)
                //{
                //    GUILayout.Label(memberNames[i]);
                //}

                //EditorGUILayout.EndScrollView();

                var rect = GUILayoutUtility.GetRect(0, EditorGUIUtility.currentViewWidth, 0, this.treeView.totalHeight);
                this.treeView.OnGUI(rect);
            }

            EditorGUILayout.EndVertical();
        }
    }
}
