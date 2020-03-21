namespace HMA2.Models
{
    public partial class File : BaseEntity
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string FileExtension { get; set; }
    }
}