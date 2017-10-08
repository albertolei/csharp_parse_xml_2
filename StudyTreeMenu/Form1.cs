using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace StudyTreeMenu
{
    public partial class treeForm : Form
    {
        String filepath = null;

        public treeForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void TreeForm_Load(object sender, EventArgs e)
        {

        }

        private void importXML_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                filepath = fileDialog.FileName;
                string ext = filepath.Split('.').Last();
                if (filepath.EndsWith(".xml", true, null) || filepath.EndsWith(".html", true, null))
                {
                    treeForm.ActiveForm.Text = "XML查看    " + filepath;
                    parse(filepath);
                }
                else
                {
                    MessageBox.Show("输入文件格式错误！");
                }
            }
        }

        private void parse(String filename)
        {
            this.treeView1.Nodes.Clear();
            this.listView1.Clear();
            XmlDocument doc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(filename, settings);
            try
            { 
                doc.Load(reader); 
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            XmlNode root = doc.DocumentElement;
            TreeNode rootTreeNode = new TreeNode(root.Name); 
            treeView1.Nodes.Add(rootTreeNode);
            XmlNodeList rootChildren = root.ChildNodes;
            getChild(rootChildren, rootTreeNode);
            rootTreeNode.Expand();
        }
        private void getChild(XmlNodeList xmlNodeList, TreeNode preTreeNode)
        {
            if (xmlNodeList.Count > 0)
            {
                foreach (XmlNode child in xmlNodeList)
                {
                    //MessageBox.Show(child.Name + "," + child.NodeType);
                    
                    if (child.NodeType == XmlNodeType.Text)
                    {
                        TreeNode childTreeNode = new TreeNode(child.Name + "," + child.InnerText);
                        preTreeNode.Nodes.Add(childTreeNode);
                    }
                    else if (child.NodeType == XmlNodeType.Element)
                    {
                        TreeNode childTreeNode = new TreeNode(child.Name);
                        XmlAttributeCollection xac = child.Attributes;
                        string attris = "";
                        foreach (XmlAttribute xa in xac)
                        {
                            attris += xa.Name + "=" + xa.Value + "\n";
                        }
                        int len = attris.Length;
                        childTreeNode.Name = len > 0 ? attris.Substring(0, len - 1) : "无=空";

                        preTreeNode.Nodes.Add(childTreeNode);
                        getChild(child.ChildNodes, childTreeNode);
                    }

                }
            }
        }

        private void quit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void treeView1_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            //treeView1.Width = treeView1.Width - 50;
        }

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            //treeView1.Width = treeView1.Width + 50;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.listView1.Clear();
            if (e.Action == TreeViewAction.ByMouse) 
            {
                TreeNode treeNode = treeView1.SelectedNode;
                string attri = treeNode.Name;
                if (attri != null && !attri.Equals(""))
                {
                    string[] attris = attri.Split('\n');
                    

                    string[] names = new string[attris.Length];
                    string[] values = new string[attris.Length];
                    this.listView1.BeginUpdate();

                    this.listView1.Columns.Add("属性", 120);
                    this.listView1.Columns.Add("值", 300);

                    foreach (string a in attris)
                    {
                        ListViewItem lvi = new ListViewItem();
                        lvi.Text = a.Split('=')[0];
                        lvi.SubItems.Add(a.Split('=')[1]);
                        this.listView1.Items.Add(lvi);
                    }
                    this.listView1.EndUpdate();
                }
                
            }
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point clickPoint = new Point(e.X, e.Y);
                TreeNode currentNode = treeView1.GetNodeAt(clickPoint);
                if (currentNode != null) 
                {
                    //MessageBox.Show(currentNode.Name);
                    currentNode.ContextMenuStrip = contextMenuStrip1;

                }
                else
                {
                    MessageBox.Show("请添加根节点!");
                }
            }
        }


        private void addChildNode_Click(object sender, EventArgs e)
        {
            ChildNodeAdd cna = new ChildNodeAdd();
            cna.Show();
            cna.Activate();
        }

        private void contextMenuStrip1_Click(object sender, EventArgs e)
        {

        }

    }
}
