using HMA2.Models;

namespace HMA2.Dtos
{
    public partial class FileDto : BaseEntity
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string FileExtension { get; set; }

        public string FileBase64String { get; set; }
    }
}