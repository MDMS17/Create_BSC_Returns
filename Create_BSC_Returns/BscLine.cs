using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Create_BSC_Return
{
    public class BscLine
    {
        public string LineSeq { get; set; }
        public string ClaimType { get; set; }
        public string ProcCode { get; set; }
        public string DateQualifier { get; set; }
        public string Service_From { get; set; }
        public string Service_To { get; set; }
        public string Date_Separator { get; set; }
        public string LineCharge { get; set; }
        public string LineAllowed { get; set; }
        public string LinePaid { get; set; }
        public string LineDeductible { get; set; }
        public string LineCoInsurance { get; set; }
        public string LineCoPay { get; set; }
        public string LineAdjustment { get; set; }
    }
}