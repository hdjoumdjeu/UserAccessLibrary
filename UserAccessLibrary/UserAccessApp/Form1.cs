using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using UserAccessLibrary;
using System.Security.Cryptography;
namespace UserAccessApp
{
    public partial class Form1 : Telerik.WinControls.UI.RadForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                SqlConnection sql = new SqlConnection(@"server=DESKTOP-ONQO9EI\SQLREVPRO;User ID=sa;Password=12345;DATABASE=GLOBAL_HOTEL");
                sql.Open();
                UserAccessDAO udao = new UserAccessDAO(sql);
                ddgroup.DataSource = UserAccessDAO.GetGroups();
                ddgroup.DisplayMember = "Title";
                listactions.DataSource= UserAccessDAO.GetActions();
                listactions.DisplayMember = "Title";
                dduser.DataSource = UserAccessDAO.GetUsers();
                dduser.DisplayMember = "Name";
                listgroups.DataSource = UserAccessDAO.GetGroups();
                listgroups.DisplayMember = "Title";

            }
            catch(Exception ex)
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //UserAccessDAO.CreateAction(new UserAccessLibrary.Action() { Code = txtcodeaction1.Text, Title = txttitleaction1.Text });
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            UserAccessDAO.CreateAction(new UserAccessLibrary.Action() { Code = txtcodeaction.Text, Title = txttitleaction.Text });
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            UserAccessDAO.CreateGroup(new UserAccessLibrary.Groupe() { Code = txtcodegroup.Text, Title = txttitlegroup.Text });
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            MD5 md5 = MD5.Create();
            UserAccessDAO.CreateUser(new UserAccessLibrary.Utilisateur() { Name = txtname.Text, Password = txtpassword.Text,Login=txtlogin.Text,IsActive=chkactive.Checked });
        }

        private void radButton5_Click(object sender, EventArgs e)
        {
            try
            {
                for(int i = 0; i < listactions.SelectedItems.Count; i++)
                {
                    UserAccessLibrary.Action action = (UserAccessLibrary.Action)listactions.SelectedItems[i].DataBoundItem;
                    UserAccessLibrary.Groupe group = (UserAccessLibrary.Groupe)ddgroup.SelectedItem.DataBoundItem;
                    UserAccessDAO.AssignActionToGroup(action, group);
                    string elt = "";

                }
            }catch(Exception ex)
            {

            }
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < listgroups.SelectedItems.Count; i++)
                {
                    UserAccessLibrary.Groupe group = (UserAccessLibrary.Groupe)listgroups.SelectedItems[i].DataBoundItem;
                    UserAccessLibrary.Utilisateur user= (UserAccessLibrary.Utilisateur)dduser.SelectedItem.DataBoundItem;
                    UserAccessDAO.AssignGroupToUser(group,user);
                    string elt = "";

                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
