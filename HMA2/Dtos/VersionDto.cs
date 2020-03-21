using HMA2.Models;

namespace HMA2.Dtos
{
    public partial class VersionDto : BaseEntity
    {
        public int Id { get; set; }

        public int VersionTypeId { get; set; }

        public string VersionTypeName { get; set; }

        public string VersionName { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public int FileId { get; set; }

        public string Memo { get; set; }

        public virtual VersionFileMappingDto VersionFileMapping { get; set; }
    }
}