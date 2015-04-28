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

namespace MathSolverWebsite.Website_Logic
{
    public class StringPair : MathSolverLibrary.TypePair<string, string>
    {
        public StringPair(string str0, string str1)
            : base(str0, str1)
        {

        }
    }

    public static class SqlHelper
    {
        public static bool Run(string queryStr, params StringPair[] pairs)
        {
            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(queryStr, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        foreach (StringPair stringPair in pairs)
                        {
                            cmd.Parameters.AddWithValue(stringPair.Data1, stringPair.Data2);
                        }

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            // Success.
                            return true;
                        }
                        else
                        {
                            // Failure.
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}