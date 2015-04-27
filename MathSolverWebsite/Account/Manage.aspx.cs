using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Membership.OpenAuth;
using System.Web.Security;

namespace MathSolverWebsite.Account
{
    public partial class Manage : System.Web.UI.Page
    {
        protected string SuccessMessage
        {
            get;
            private set;
        }

        protected bool CanRemoveExternalLogins
        {
            get;
            private set;
        }

        protected void Page_Load()
        {
            if (!IsPostBack)
            {
                // Determine the sections to render
                var hasLocalPassword = User.Identity.IsAuthenticated && OpenAuth.HasLocalPassword(User.Identity.Name);
                changePassword.Visible = hasLocalPassword;
                deleteAccountBtn.Visible = hasLocalPassword;
                var message = Request.QueryString["m"];
                if (!hasLocalPassword)
                {
                    if (message == null)
                    {
                        Response.Redirect("/");
                        return;
                    }
                }


                // Render success message
                if (message != null)
                {
                    // Strip the query string from action
                    Form.Action = ResolveUrl("~/Account/Manage");

                    if (message.EndsWith(".aspx"))
                        message = message.Remove(message.Length - ".aspx".Length, ".aspx".Length);

                    SuccessMessage =
                        message == "ChangePwdSuccess" ? "Your password has been changed."
                        : message == "RemoveLoginSuccess" ? "The account was deleted."
                        : message == "RemoveLoginFailure" ? "The account could not be found."
                        : String.Empty;
                    successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
                }
            }

        }

        protected void deleteAccountBtn_Click(object sender, EventArgs e)
        {
            string userName;
            if (User.Identity.IsAuthenticated)
                userName = User.Identity.Name;
            else
            {
                Response.Redirect("~/account/manage?m=RemoveLoginFailure");
                return;
            }

            FormsAuthentication.SignOut();
            Membership.DeleteUser(userName);
            Response.Redirect("~/account/manage?m=RemoveLoginSuccess");
        }
    }
}