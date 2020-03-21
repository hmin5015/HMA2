namespace HMA2.Models
{
    public partial class VersionFileMapping
    {
        public int Id { get; set; }

        public int VersionId { get; set; }

        public int FileId { get; set; }

        public virtual File File { get; set; }
    }
}