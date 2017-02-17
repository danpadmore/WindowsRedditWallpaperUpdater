using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsRedditWallpaperUpdater.SystemTrayIcon
{
    internal class TrayIcon : IDisposable
    {
        private readonly NotifyIcon notifyIcon;
        private readonly List<ToolStripMenuItem> menuItems;

        public TrayIcon(string name, Icon icon)
        {
            menuItems = new List<ToolStripMenuItem>();

            notifyIcon = new NotifyIcon();
            notifyIcon.Text = name;
            notifyIcon.Icon = icon;
        }

        public TrayIcon Initialize()
        {
            var contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.AddRange(menuItems.ToArray());
            contextMenuStrip.Name = "TrayIconContextMenu";
            contextMenuStrip.Size = new Size(153, 70);

            notifyIcon.ContextMenuStrip = contextMenuStrip;
            notifyIcon.Visible = true;

            return this;
        }

        public TrayIcon AddMenuItem(string name, EventHandler eventHandler)
        {
            var toolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem.Name = name;
            toolStripMenuItem.Size = new Size(152, 22);
            toolStripMenuItem.Text = name;
            toolStripMenuItem.Click += eventHandler;
            menuItems.Add(toolStripMenuItem);

            return this;
        }

        public void Dispose()
        {
            notifyIcon.Visible = false;
        }
    }
}