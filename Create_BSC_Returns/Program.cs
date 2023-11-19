using Create_BSC_Return;
using System.Runtime.CompilerServices;
using System.Text;

List<string> Dcns = new List<string> {"230417819702",
"000511112601",
"000545263700",
"226643441501",
"234468974800",
"232831288400",
"232935224500",
"232756835800",
"233388687300",
"233544702400",
"233673058200",
"225433638101",
"234025895900",
"234129558900",
"234345459700",
"234513946400",
"234745113700",
"233196621701",
"234364742000",
"235059577300",
"234790075601",
"235126904900"
};

StringBuilder sb = new StringBuilder();
string str_now = DateTime.Now.ToString("yyyyMMddhhmmss");
int Count101 = 0;
int Count102 = 0;
int Count201 = 0;
int Count501 = 0;
int Count502 = 0;

//header
sb.AppendLine($"00000000000000 001 WP2024290           P{str_now}" + "".PadRight(471, ' '));
foreach (string Dcn in Dcns)
{
    BscHeader bscHeader = new BscHeader();
    List<BscLine> bscLines = new List<BscLine>();
    DataAccess.GetBsc835Data(Dcn, ref bscHeader, ref bscLines);

    //101
    sb.AppendLine($"{Dcn}   101 001 {bscHeader.PatLastName.PadRight(35, ' ')}{bscHeader.PatFirstName.PadRight(25, ' ')} {bscHeader.EmpLastName.PadRight(35, ' ')}{bscHeader.EmpFirstName.PadRight(25, ' ')} {bscHeader.MemId}{"".PadRight(371, ' ')}");
    Count101++;

    //102
    sb.AppendLine($"{Dcn}   102 001 {bscHeader.PayeeName.PadRight(60, ' ')}{bscHeader.PayeeAddr1.PadRight(55, ' ')}{bscHeader.PayeeCity.PadRight(30, ' ')}{bscHeader.PayeeState.PadRight(2, ' ')}{bscHeader.PayeeZip.PadRight(9, '0')}          {bscHeader.PayeeTaxID.PadRight(9, ' ')}{"".PadRight(327, ' ')}");
    Count102++;

    //201
    sb.AppendLine($"{Dcn}   201 001 {bscHeader.ClaimStatus}{bscHeader.PaymentDirection} N{bscHeader.Adjustment}{bscHeader.CheckNumber.PadRight(30, ' ')}{bscHeader.CheckDate}{bscHeader.ChargeAmount.Replace(".", "").PadLeft(18, '0')}{bscHeader.AllowedAmount.Replace(".", "").PadLeft(18, '0')}{bscHeader.PaidAmount.Replace(".", "").PadLeft(18, '0')}{bscHeader.EldoClaimNo}{"".PadRight(393, ' ')}");
    Count201++;

    //claim level balance
    decimal ClaimCharged = decimal.Parse(bscHeader.ChargeAmount);
    decimal ClaimPaid = decimal.Parse(bscHeader.PaidAmount);
    decimal ClaimAllowed = decimal.Parse(bscHeader.AllowedAmount);
    decimal TotalLineCharged = bscLines.Sum(x => decimal.Parse(x.LineCharge));
    decimal TotalLinePaid = bscLines.Sum(x => decimal.Parse(x.LinePaid));
    decimal TotalLineAllowed = bscLines.Sum(x => decimal.Parse(x.LineAllowed));
    if (ClaimCharged != TotalLineCharged)
    {
        //decimal rateCharged = Math.Round(ClaimCharged / TotalLineCharged, 4);
        //decimal sumCharged = 0;
        //for (int i = 0; i < bscLines.Count; i++)
        //{
        //    decimal lineCharged = Math.Round(decimal.Parse(bscLines[i].LineCharge) * rateCharged, 2);
        //    bscLines[i].LineCharge = lineCharged.ToString("#.00");
        //    if (i == bscLines.Count - 1)
        //    {
        //        bscLines[i].LineCharge = (ClaimCharged - sumCharged).ToString("#.00");
        //    }
        //    sumCharged += lineCharged;
        //}
        bscHeader.ChargeAmount = TotalLineCharged.ToString("#.00");
    }

    if (ClaimAllowed != TotalLineAllowed)
    {
        //decimal rateAllowed = Math.Round(ClaimAllowed / TotalLineAllowed, 4);
        //decimal sumAllowed = 0;
        //for (int i = 0; i < bscLines.Count; i++)
        //{
        //    decimal lineAllowed = Math.Round(decimal.Parse(bscLines[i].LineAllowed) * rateAllowed, 2);
        //    bscLines[i].LineAllowed = lineAllowed.ToString("#.00");
        //    if (i == bscLines.Count - 1)
        //    {
        //        bscLines[i].LineAllowed = (ClaimAllowed - sumAllowed).ToString("#.00");
        //    }
        //    sumAllowed += lineAllowed;
        //}
        bscHeader.AllowedAmount = TotalLineAllowed.ToString("#.00");
    }

    if (ClaimPaid != TotalLinePaid)
    {
        decimal ratePaid = Math.Round(ClaimPaid / TotalLinePaid, 4);
        decimal sumPaid = 0;
        for (int i = 0; i < bscLines.Count; i++)
        {
            decimal linePaid = Math.Round(decimal.Parse(bscLines[i].LinePaid) * ratePaid, 2);
            bscLines[i].LinePaid = linePaid.ToString("#.00");
            if (i == bscLines.Count - 1)
            {
                bscLines[i].LinePaid = (ClaimPaid - sumPaid).ToString("#.00");
            }
            sumPaid += linePaid;
        }
    }

    foreach (BscLine bscLine in bscLines)
    {
        //501
        sb.AppendLine($"{Dcn}   501 {bscLine.LineSeq.ToString().PadLeft(3, '0')} {bscLine.LineSeq.ToString().PadRight(4, ' ')}{bscLine.ClaimType}{bscLine.ProcCode.PadRight(5, ' ')}        {bscLine.DateQualifier}{bscLine.Service_From}{bscLine.Date_Separator}{bscLine.Service_To}{bscLine.LineCharge.Replace(".", "").PadLeft(18, '0')}{bscLine.LineAllowed.Replace(".", "").PadLeft(18, '0')}{bscLine.LinePaid.Replace(".", "").PadLeft(18, '0')}{"".PadRight(407, ' ')}");
        Count501++;
        //line level balance, linePR = lineallowed-linepaid; lineadjust = linecharged - lineallowed
        decimal lineCharge = decimal.Parse(bscLine.LineCharge);
        decimal linePaid = decimal.Parse(bscLine.LinePaid);
        decimal lineAdjust = decimal.Parse(bscLine.LineAdjustment);
        decimal lineDeductible = decimal.Parse(bscLine.LineDeductible);
        decimal lineCoInsurance = decimal.Parse(bscLine.LineCoInsurance);
        decimal lineCoPay = decimal.Parse(bscLine.LineCoPay);
        decimal lineAllowed = decimal.Parse(bscLine.LineAllowed);
        if (lineCharge != lineAllowed + lineAdjust)
        {
            lineAdjust = lineCharge - lineAllowed;
            bscLine.LineAdjustment = lineAdjust.ToString("#.00");
        }
        if (lineAllowed != linePaid + lineDeductible + lineCoInsurance + lineCoPay)
        {
            lineCoPay = lineAllowed - linePaid - lineDeductible - lineCoInsurance;
            bscLine.LineCoPay = lineCoPay.ToString("#.00");
        }
        if (lineAdjust + lineDeductible + lineCoInsurance + lineCoPay != 0)
        {
            //502
            string str502 = $"{Dcn}   502 {bscLine.LineSeq.ToString().PadLeft(3, '0')} {bscLine.LineSeq.ToString().PadRight(4, ' ')}";
            if (lineAdjust != 0)
            {
                str502 += $"CO45   {bscLine.LineAdjustment.Replace(".", "").PadLeft(18, '0')}";
            }
            if (lineDeductible != 0)
            {
                str502 += $"PR01   {bscLine.LineDeductible.Replace(".", "").PadLeft(18, '0')}";
            }
            if (lineCoInsurance != 0)
            {
                str502 += $"PR02   {bscLine.LineCoInsurance.Replace(".", "").PadLeft(18, '0')}";
            }
            if (lineCoPay != 0)
            {
                str502 += $"PR03   {bscLine.LineCoPay.Replace(".", "").PadLeft(18, '0')}";
            }
            sb.AppendLine(str502 + "".PadRight(525 - str502.Length, ' '));
            Count502++;
        }
    }
}



//footer

sb.Append("00000000000000 999 " + Count101.ToString().PadLeft(7, '0') + Count102.ToString().PadLeft(7, '0') + Count201.ToString().PadLeft(7, '0') + "0000000" + Count501.ToString().PadLeft(9, '0') + Count502.ToString().PadLeft(9, '0') + "".PadRight(460, ' '));

File.WriteAllText(@"C:\Project_MY\AgingClaims\BlueShield\New_Gens\WP2024290_RETURN_" + DateTime.Today.ToString("yyyyMMdd") + "_90.TXT", sb.ToString());