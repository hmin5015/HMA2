namespace HMA2.Models
{
    public partial class Version : BaseEntity
    {
        public int Id { get; set; }

        public int VersionTypeId { get; set; }

        public string VersionName { get; set; }

        public string Memo { get; set; }

        public virtual VersionType VersionType { get; set; }

        public virtual VersionFileMapping VersionFileMapping { get; set; }
    }
}