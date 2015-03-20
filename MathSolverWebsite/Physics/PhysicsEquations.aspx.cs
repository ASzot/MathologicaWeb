using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Hosting;

using MathSolverWebsite.WebsiteHelpers;

namespace MathSolverWebsite.Physics
{
    public partial class PhysicsEquations : System.Web.UI.Page
    {
        public const string EXAMPLE_FILE_PATH = "~/Content/Equations.xml";

        private static EquationMgr _equationMgr = null;

        public static TopicPath GetEqData(string eqName)
        {
            if (_equationMgr == null && !LoadEquations(true))
                return null;

            return _equationMgr.GetEqData(eqName);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (LoadEquations())
            {
                equationsContentDiv.InnerHtml = HtmlHelper.TopicDataToHtmlTree(_equationMgr.PhysicsEqData.Cast<TopicPath>().ToList(), "All Equations", "EquationDisplay", Server);
            }
        }

        public static bool LoadEquations(bool useRad)
        {
            if (_equationMgr == null)
            {
                _equationMgr = new EquationMgr();
                return _equationMgr.LoadEquations(HostingEnvironment.MapPath(EXAMPLE_FILE_PATH), useRad);
            }

            return true;
        }

        public bool LoadEquations()
        {
            object useRadObj = Session[_Default.RAD_SES_KEY];
            bool useRad = useRadObj != null ? (bool)useRadObj : true;

            return LoadEquations(useRad);
        }
    }
}