namespace ProfitLossCalculatorWebApi.Dto;

    public class GetRayanDataResponseDto
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int CustomerId { get; set; }
        public string DbsAccountNumber { get; set; }
        public string CustomerFullName { get; set; }
        public object ParentCustomerId { get; set; }
        public object ParentCustomerFullName { get; set; }
        public string TransactionDate { get; set; }
        public int IsAPurchase { get; set; }
        public string InstrumentBourseAccount { get; set; }
        public string InsMaxLcode { get; set; }
        public long Amount { get; set; }
        public int Interest { get; set; }
        public int Development { get; set; }
        public int BourseCo { get; set; }
        public int BourseOrg { get; set; }
        public int DepositCo { get; set; }
        public int ItManagement { get; set; }
        public int Facility { get; set; }
        public int Tax { get; set; }
        public int IsRight { get; set; }
        public int Quantity { get; set; }
        public string DlNumber { get; set; }
        public int CbranchId { get; set; }
        public string CbranchName { get; set; }
    }

    public class Transactions
    {
        public List<GetRayanDataResponseDto> result { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int Offset { get; set; }
        public int Total { get; set; }
    }

    
