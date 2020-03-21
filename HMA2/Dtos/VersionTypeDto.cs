using HMA2.Models;

namespace HMA2.Dtos
{
    public partial class VersionTypeDto : BaseEntity
    {
        public int Id { get; set; }

        public string VersionTypeName { get; set; }
    }
}