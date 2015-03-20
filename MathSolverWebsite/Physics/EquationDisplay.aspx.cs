using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MathSolverWebsite.WebsiteHelpers;

namespace MathSolverWebsite.Physics
{
    public partial class EquationDisplay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string eqStr = Request.QueryString["Name"];
            if (eqStr != null)
                eqStr = HtmlHelper.CleanQueryStr(eqStr);

            if (eqStr == null)
                Response.Redirect("Physics/PhysicsEquations");

            eqStr = Server.UrlDecode(eqStr);

            Title = eqStr;

            PhysicsEqData nlblTopicData = (PhysicsEqData)PhysicsEquations.GetEqData(eqStr);

            if (nlblTopicData == null)
                Response.Redirect("Physics/PhysicsEquations");

            contentDiv.InnerHtml = nlblTopicData.ToHtml();

            Title = nlblTopicData.DispName;
        }
    }
}