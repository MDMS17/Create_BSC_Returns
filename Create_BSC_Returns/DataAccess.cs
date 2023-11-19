using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualBasic;
namespace Create_BSC_Return
{
    public static class DataAccess
    {
        public static void GetBsc835Data(string DCN, ref BscHeader bscHeader, ref List<BscLine> bscLines)
        {
            string sYear = DateTime.Today.Year.ToString();
            using (SqlConnection conn = new SqlConnection(@""))
            {
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(@$"", conn);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {


                }
                if (ds.Tables[3].Rows.Count > 0)
                {


                }
                if (ds.Tables[1].Rows.Count > 0)
                {

                }
                List<BscLine> bscLines2 = new List<BscLine>();
                if (ds.Tables[4].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                    {
                        BscLine bscLine = new BscLine();
                        bscLines2.Add(bscLine);
                    }
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        BscLine bscLine = new BscLine();
                        bscLines.Add(bscLine);

                        //ClaimType: NU==>institutional; HC==>professional
                        //DateQualifier: normally 472, but if cross year, 150
                        //Date_Separator: empty if from=to; 151 from<>to
                    }
                }
                if (ds.Tables[5].Rows.Count != ds.Tables[2].Rows.Count)
                {
                    List<BscLine> bscLines3 = new List<BscLine>();
                    for (int i = 0; i < ds.Tables[5].Rows.Count; i++)
                    {
                        BscLine bscLine = new BscLine();
                        bscLines3.Add(bscLine);
                        bscLines3.Add(bscLine);
                    }
                    bscLines = bscLines3;
                }
            }
        }

    }
}