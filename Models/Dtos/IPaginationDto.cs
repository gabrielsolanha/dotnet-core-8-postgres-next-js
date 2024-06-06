namespace AplicacaoWeb.Models.Dtos
{
    public interface IPaginationDto<T> where T : BaseDto
    {
        T Filter { get; set; }
        int ItemCount { get; set; }
        int Page { get; set; }
        string SortOrder { get; set; }
        bool Descending { get; set; }
        public string RelParam { get; set; }
    }

}
