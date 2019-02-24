namespace KuaforEvde.Core.Model
{
    public class BasePagingRequest
    {
        public int StartPage { get; set; } = 0;
        public int Limit { get; set; } = int.MaxValue;

        public int Offset
        {
            get { return (StartPage - 1) * Limit; }
        }


    }
}
