
namespace MES.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; private set; }
        public string CreatedBy { get; private set; } = string.Empty;
        public string? UpdatedBy { get; private set; }
        public bool IsDeleted { get; private set; }

        public void SetCreatedBy(string user) => CreatedBy = user;

        public void SetUpdatedBy(string user)
        {
            UpdatedBy = user;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SoftDelete() => IsDeleted = true;
    }
}
