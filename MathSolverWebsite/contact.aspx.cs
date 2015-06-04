using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MathSolverWebsite
{
    public partial class contact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void submitBtn_Click(object sender, EventArgs e)
        {

            string msgStr = messageBody.Value;
            if (msgStr.Trim().Replace("\t", "").Replace("\n", "") == "")
            {
                outputDisp.InnerHtml = "<p class='error-msg'>Enter a message.</p>";
            }
            else
            {
                outputDisp.InnerHtml = "<p class='success-msg'>Thank you for the feedback.</p><a href='/'>Go Back to the Homepage.</a>";
            }
        }
    }
}