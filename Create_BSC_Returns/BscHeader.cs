using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Create_BSC_Return
{
    public class BscHeader
    {
        public string PatLastName { get; set; }
        public string PatFirstName { get; set; }
        public string EmpLastName { get; set; }
        public string EmpFirstName { get; set; }
        public string MemId { get; set; }
        public string PayeeName { get; set; }
        public string PayeeAddr1 { get; set; }
        public string PayeeCity { get; set; }
        public string PayeeState { get; set; }
        public string PayeeZip { get; set; }
        public string PayeeTaxID { get; set; }
        public string ClaimStatus { get; set; }
        public string PaymentDirection { get; set; }
        public string Adjustment { get; set; }
        public string CheckNumber { get; set; }
        public string CheckDate { get; set; }
        public string ChargeAmount { get; set; }
        public string AllowedAmount { get; set; }
        public string PaidAmount { get; set; }
        public string EldoClaimNo { get; set; }
        public string Recipient { get; set; }
        public string ClaimType { get; set; }
    }
}

