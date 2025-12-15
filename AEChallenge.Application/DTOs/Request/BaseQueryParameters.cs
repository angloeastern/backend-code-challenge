namespace AEChallenge.Application.DTOs.Request
{
    public class BaseQueryParameters
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public string SortColumn { get; set; } = "Id";
        public string SortDirection { get; set; } = "ASC";
        public string? SearchTerm { get; set; } = string.Empty;

        private const int MaxSearchLength = 50;

        public virtual (bool IsValid, string ErrorMessage) Validate()
        {
            if (PageSize <= 0)
            {
                return (false, "Page size must be greater than zero.");
            }
            if (PageNumber <= 0)
            {
                return (false, "Page number must be greater than zero.");
            }
            if (SearchTerm != null && SearchTerm.Length > MaxSearchLength)
            {
                return (false, $"Search term cannot exceed {MaxSearchLength} characters.");
            }
            string normalizedDirection = SortDirection.Trim().ToUpperInvariant();
            if (normalizedDirection != "ASC" && normalizedDirection != "DESC")
            {
                return (false, "Sort direction must be 'ASC' or 'DESC'.");
            }

            SortDirection = normalizedDirection;

            return (true, string.Empty);
        }
    }
}