using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MathSolverWebsite.Account
{
    public partial class Preferences : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void CreateCookie(string cookieValue)
        {
            HttpCookie userPrefCookies = new HttpCookie("UserPref");
            userPrefCookies["ColorTheme"] = cookieValue;
            userPrefCookies.Expires = DateTime.Now.AddDays(5);

            Response.Cookies.Add(userPrefCookies);
        }

        protected void whiteOptionId_Click(object sender, EventArgs e)
        {
            CreateCookie("white");
            Response.Redirect(Request.RawUrl);
        }

        protected void blackOptionId_Click(object sender, EventArgs e)
        {
            CreateCookie("black");
            Response.Redirect(Request.RawUrl);
        }
    }
}