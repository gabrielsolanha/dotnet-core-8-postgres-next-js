using System;

namespace AplicacaoWeb.Models.Entities
{
    public class BaseEntity
    {
        public virtual int Id { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual string? UpdatedBy { get; set; }
        public virtual DateTime? UpdatedDate { get; set; }
        public virtual bool IsDeleted { get; set; }
    }
}
