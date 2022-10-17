using System.Collections.Generic;
using System.Reflection;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AillieoUtils.CSReflectionUtils.Editor
{
    public class MemberInfoTreeView : TreeView
    {
        private List<MemberInfo> data = new List<MemberInfo>();

        public MemberInfoTreeView(TreeViewState state)
            : base(state)
        {
            showAlternatingRowBackgrounds = true;

            rowHeight = 20;
            showBorder = true;

            Reload();
        }

        public override void OnGUI(Rect rect)
        {
            base.OnGUI(rect);
        }

        protected override TreeViewItem BuildRoot()
        {
            return new TreeViewItem { id = 0, depth = -1, displayName = "root" };
        }

        protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
        {
            List<TreeViewItem> rows = new List<TreeViewItem>();
            foreach (var member in data)
            {
                if (!string.IsNullOrEmpty(searchString) && !member.Name.ContainsIgnoreCase(searchString))
                {
                    continue;
                }

                TreeViewItem item = new TreeViewItem();
                item.id = member.GetHashCode();
                item.displayName = member.Name;
                rows.Add(item);
            }

            return rows;
        }

        public void SetData(IEnumerable<MemberInfo> newData)
        {
            this.data.Clear();
            this.data.AddRange(newData);

            Reload();
        }
    }
}
