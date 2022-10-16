using System.Collections.Generic;
using System.Reflection;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

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
        IList<TreeViewItem> rows = new List<TreeViewItem>();

        //IList<TreeViewItem> rows = GetRows();
        //rows = new List<TreeViewItem>();

        if (!string.IsNullOrEmpty(searchString))
        {
        }
        else
        {
            foreach (var member in data)
            {
                TreeViewItem item = new TreeViewItem();
                item.id = member.GetHashCode();
                item.displayName = member.Name;
                root.AddChild(item);
            }
        }

        SetupDepthsFromParentsAndChildren(root);
        return base.BuildRows(root);
    }

    public void SetData(IEnumerable<MemberInfo> newData)
    {
        this.data.Clear();
        this.data.AddRange(newData);

        BuildRows(rootItem);

        Reload();
    }
}
