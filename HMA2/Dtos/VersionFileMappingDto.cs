namespace HMA2.Dtos
{
    public partial class VersionFileMappingDto
    {
        public int Id { get; set; }

        public int VersionId { get; set; }

        public int FileId { get; set; }

        public virtual FileDto File { get; set; }
    }
}