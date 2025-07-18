namespace PWA_GREMIO_API.Core.Entities.Users
{
    public abstract class EntityBase<TI>
    {
        public TI Id { get; set; }
    }
}