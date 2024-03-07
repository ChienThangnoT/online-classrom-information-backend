using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Data
{
    public class AccountFilterParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; } = string.Empty;
        public string? Search { get; set; } = string.Empty;

    }

    public class AccountListResult
    {
        public IEnumerable<AccountModelGetList> Accounts { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalAccounts { get; set; }
        public int TotalPages { get; set; }
    }
}
