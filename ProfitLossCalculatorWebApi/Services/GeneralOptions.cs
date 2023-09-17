namespace ProfitLossCalculatorWebApi.Services;

public class GeneralOptions
{

    public class GeneralOption1
    {
        public static string DsCode { get; set; }
        public static string FromDate { get; set; }
        public static string ToDate { get; set; }
        public static string NationalCode { get; set; }
        public static string Size { get; set; }


        public static void Init(string dsCode, string fromDate, string toDate, string nationalCode, string size)
        {
            DsCode = dsCode;
            FromDate = fromDate;
            ToDate = toDate;
            NationalCode = nationalCode;
            Size = size;
        }
    }


    public class GeneralOption2
    {
        public static string DsCode2 { get; set; }
        public static string FromDate2 { get; set; }
        public static string ToDate2 { get; set; }
        public static string NationalCode2 { get; set; }
        public static string Size2 { get; set; }


        
        public static void Init(string dsCode2, string fromDate2, string toDate2, string nationalCode2, string size2 )
        {
            DsCode2 = dsCode2;
            FromDate2 = fromDate2;
            ToDate2 = toDate2;
            NationalCode2 = nationalCode2;
            Size2 = size2;
        }
    }
}