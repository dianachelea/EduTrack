using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class FileDetails
    {
        [Column("FileName")]
        public string FileName { get; set; }
        [Column("Path")]
        public string Path { get; set; }
    }
}
