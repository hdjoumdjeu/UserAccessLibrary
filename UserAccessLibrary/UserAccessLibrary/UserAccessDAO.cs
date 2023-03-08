using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace UserAccessLibrary
{
    public class UserAccessDAO
    {
       static SqlConnection _Connection = null;
        public UserAccessDAO(SqlConnection con)
        {
            UserAccessDAO._Connection = con;
        }
        public Utilisateur Login(string Identifiant, string Password)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "Identifiant", DbType = System.Data.DbType.String, Value = Identifiant });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "Password", DbType = System.Data.DbType.String, Value = Password });
                SqlDataReader lecteur = cmd.ExecuteReader();
                if (lecteur.Read())
                    return GetUserAndGroup(new Utilisateur() { Id = Convert.ToInt32(lecteur["Id"]),CreateOn = Convert.ToDateTime(lecteur["CreateOn"]), IsActive = Convert.ToBoolean(lecteur["IsActive"]), Name = lecteur["Name"].ToString(), Login = lecteur["Login"].ToString(), LastConnect = Convert.ToDateTime(lecteur["LastConnect"]) });
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }
        public Utilisateur GetUserAndGroup(Utilisateur usertosearch)
        {
            Groupe groupe = null;
            Utilisateur user = null;
            try
            {

                List<Action> actions = new List<Action>();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Utilisateur user JOIN Groupe grp ON user.IdGroupe = grp.Id JOIN Action act ON grp.IdAction=act.Id WHERE user.Id=" + usertosearch.Id,_Connection);
                SqlDataReader lecteur = cmd.ExecuteReader();
                while (lecteur.Read())
                {
                    if(groupe == null)
                    {
                        user = new Utilisateur() { CreateOn = Convert.ToDateTime(lecteur["CreateOn"]), IsActive = Convert.ToBoolean(lecteur["IsActive"]), Name = lecteur["Name"].ToString(), Login = lecteur["Login"].ToString(), LastConnect = Convert.ToDateTime(lecteur["LastConnect"]) };
                        groupe = new Groupe() { Id = Convert.ToInt32(lecteur["idgroup"]), Code = lecteur["codegroup"].ToString(), Title = lecteur["titlegroup"].ToString() };
                    }
                    actions.Add(new Action() { Id = Convert.ToInt32(lecteur["idaction"]), Code = lecteur["codeaction"].ToString(), Title = lecteur["titleaction"].ToString() });
                }
                if (user != null)
                {
                    groupe.Actions = actions;
                    user.Group = groupe;
                    
                }
             
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }
        public static List<Action> GetActions()
        {
            List<Action> actions = new List<Action>();
            try
            {

                SqlCommand cmd = new SqlCommand("SELECT * FROM action", _Connection);
                SqlDataReader lecteur = cmd.ExecuteReader();
                while (lecteur.Read())
                {
                    actions.Add(new Action() { Id = Convert.ToInt32(lecteur["Id"]), Code = lecteur["Code"].ToString(), Title = lecteur["Title"].ToString() });
                }
                lecteur.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return actions;
        }
        public static List<Utilisateur> GetUsers()
        {
            List<Utilisateur> users = new List<Utilisateur>();
            try
            {

                SqlCommand cmd = new SqlCommand("SELECT * FROM Utilisateur", _Connection);
                SqlDataReader lecteur = cmd.ExecuteReader();
                while (lecteur.Read())
                {
                    users.Add(new Utilisateur() { Id = Convert.ToInt32(lecteur["Id"]), Name = lecteur["Name"].ToString(), Login = lecteur["Login"].ToString() });
                }
                lecteur.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return users;
        }
        public static List<Groupe> GetGroups()
        {
            List<Groupe> groupes = new List<Groupe>();
            try
            {

                SqlCommand cmd = new SqlCommand("SELECT * FROM [Group]", _Connection);
                SqlDataReader lecteur = cmd.ExecuteReader();
                while (lecteur.Read())
                {
                    groupes.Add(new Groupe() { Id = Convert.ToInt32(lecteur["Id"]), Code = lecteur["Code"].ToString(), Title = lecteur["Title"].ToString() });
                }
                lecteur.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return groupes;
        }
        public static bool CreateGroup(Groupe grouptoinsert)
        {
            try
            {
                string chaine = "INSERT INTO Group(Code,Title) VALUES('" + grouptoinsert.Code + "','" + grouptoinsert.Title + "')";
                SqlCommand cmd = new SqlCommand("INSERT INTO [Group](Code,Title) VALUES('" + grouptoinsert.Code + "','" + grouptoinsert.Title + "')", _Connection);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public static bool CreateAction(Action actiontoinsert)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("INSERT INTO Action(Code,Title) VALUES('" + actiontoinsert.Code + "','" + actiontoinsert.Title + "')", _Connection);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public static bool AssignActionToGroup(Action actiontoassign, Groupe group)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("INSERT INTO Group_Action(IdGroup,IdAction) VALUES('" + group.Id + "','" + actiontoassign.Id + "')", _Connection);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public static bool AssignGroupToUser(Groupe grouptoassign, Utilisateur user)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("INSERT INTO Group_Utilisateur(Id_Group,Id_User) VALUES('" + grouptoassign.Id+ "','" + user.Id + "')", _Connection);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public static bool CreateUser(Utilisateur usertosearch)
        {
            try
            {

                List<Action> actions = new List<Action>();
                SqlCommand cmd = new SqlCommand("INSERT INTO Utilisateur(CreateOn,IsActive,Name,Login,Password) VALUES(getdate(),'" + (usertosearch.IsActive==true?"1":"0") + "','" + usertosearch.Name + "','" + usertosearch.Login + "','" + usertosearch.Password + "')", _Connection);
                SqlDataReader lecteur = cmd.ExecuteReader();
                /*while (lecteur.Read())
                {
                    if (groupe == null)
                    {
                        user = new Utilisateur() { CreateOn = Convert.ToDateTime(lecteur["CreateOn"]), IsActive = Convert.ToBoolean(lecteur["IsActive"]), Name = lecteur["Name"].ToString(), Login = lecteur["Login"].ToString(), LastConnect = Convert.ToDateTime(lecteur["LastConnect"]) };
                        groupe = new Groupe() { Id = Convert.ToInt32(lecteur["idgroup"]), Code = lecteur["codegroup"].ToString(), Title = lecteur["titlegroup"].ToString() };
                    }
                    actions.Add(new Action() { Id = Convert.ToInt32(lecteur["idaction"]), Code = lecteur["codeaction"].ToString(), Title = lecteur["titleaction"].ToString() });
                }
                if (user != null)
                {
                    groupe.Actions = actions;
                    user.Group = groupe;

                }*/
            }
            catch (Exception ex)
            {
                return false;
                //throw new Exception(ex.Message);
            }
            return true;
        }

    }
}
