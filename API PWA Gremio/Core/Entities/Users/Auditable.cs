using PWA_GREMIO_API.Core.Entities.Users;

namespace PWA_GREMIO_API.Core.Entities
{
    public abstract class Auditable<TI> : EntityBase<TI>
    {
        public DateOnly? DeletedDate { get; set; }
        public DateOnly CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? DeletedBy { get; set; }
        public int? ModifiedBy { get; set; }

    }
}
