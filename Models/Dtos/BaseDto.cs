namespace AplicacaoWeb.Models.Dtos
{
    public class BaseDto
    {
        public int? Id { get; set; }
        public virtual string? CreatedBy { get; set; }
        public virtual DateTime? CreatedAt { get; set; }
        public virtual string? UpdatedBy { get; set; }
        public virtual DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
