namespace AplicacaoWeb.Models.Dtos
{
    public class PaginationDto<T> : IPaginationDto<T> where T : BaseDto
    {
        public T Filter { get; set; }
        public int ItemCount { get; set; }
        public int Page { get; set; }
        public string SortOrder { get; set; }
        public bool Descending { get; set; }
        public string RelParam { get; set; }
    }
}
