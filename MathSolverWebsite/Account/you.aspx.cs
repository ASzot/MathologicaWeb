using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.ModelBinding;
using System.Web.Routing;

using System.Web.Security;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using MathSolverWebsite.Website_Logic;


namespace MathSolverWebsite.Account
{
    public partial class You : System.Web.UI.Page
    {
        public const string START_MATH = MathSolverLibrary.WorkMgr.STM;
        public const string END_MATH = MathSolverLibrary.WorkMgr.EDM;

        private string GetUserId()
        {
            MembershipUser currentUser = Membership.GetUser(User.Identity.Name);
            Guid userId = (Guid)currentUser.ProviderUserKey;
            string userIdStr = userId.ToString("N");

            return userIdStr;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("/");
                return;
            }
            if (!this.IsPostBack)
            {
                genError.Text = "";
                BindListView(GetUserId());
            }
            
        }

        private void BindListView(string userId, int indexToRemove = -1)
        {
            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT DISTINCT id, problem, entry_time FROM SavedProblems WHERE id='" + userId + "';";
                    cmd.Connection = con;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (indexToRemove != -1)
                            dt.Rows[indexToRemove].Delete();

                        savedProblemsList.DataSource = dt;
                        savedProblemsList.DataBind();
                    }
                }
            }
        }

        protected void savedProblemsList_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                string probId = (string)e.CommandArgument;
                int removeIndex = e.Item.DisplayIndex;
                savedProblemsList.DeleteItem(removeIndex);
                BindListView(GetUserId(), removeIndex);

                if (!SqlHelper.Run("DELETE FROM SavedProblems WHERE problem='" + probId + "'"))
                {
                    // Some error message.
                    genError.Text = "Couldn't delete";
                }
            }
        }

        protected void savedProblemsList_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
        }
    }
}