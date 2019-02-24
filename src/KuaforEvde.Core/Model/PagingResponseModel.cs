namespace KuaforEvde.Core.Model
{
    public class PagingResponseModel<T> : ResponseModel<T>
    {
        public int StartPage { get; set; }

        public int Limit { get; set; }

        public bool HasNextPage => StartPage + 1 < TotalPageCount;

        public bool HasPrevPage => StartPage > 1;

        public int TotalCount { get; set; }

        public int TotalPageCount
        {
            get
            {
                if (Limit == 0)
                    Limit = int.MaxValue;

                if (TotalCount % Limit == 0)
                {
                    return TotalCount / Limit;
                }
                return (TotalCount / Limit) + 1;
            }
        }
    }
}
