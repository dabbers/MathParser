using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dab.Library.MathParser;

namespace CalculateApi
{
    public partial class calculate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MathParser mp = new MathParser("https://openexchangerates.org/api/latest.json?app_id=REPLACE YOUR ID", Server.MapPath("~"));
            try
            {
                var expression = Request.QueryString["expression"];
                if (String.IsNullOrEmpty(expression))
                {
                    Response.Write("Please enter an expression url/?expression=1+1");
                }
                else
                {
                    var res = mp.Evaluate(expression);
                    Response.Write(mp.GetInterpretation() + " = " + res.ToString());
                }
            }
            catch (MathParserException ex)
            {
                Response.Write("Math Parser Error: " + ex.Message);
            }
            catch(Exception ex)
            {
                Response.Write("Exception: " + ex.Message);
            }
        }
    }
}