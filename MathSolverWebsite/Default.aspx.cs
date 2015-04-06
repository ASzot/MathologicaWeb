using MathSolverWebsite.MathSolverLibrary;
using MathSolverWebsite.MathSolverLibrary.Equation;
using MathSolverWebsite.WebsiteHelpers;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;


using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace MathSolverWebsite
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            evaluateDropDownList.Items.Clear();
            evaluateDropDownList.Items.Add("Tester eval input");
        }
    }
}