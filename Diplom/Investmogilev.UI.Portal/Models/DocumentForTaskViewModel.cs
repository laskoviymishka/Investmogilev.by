using Investmogilev.Infrastructure.Common.Model.Common;

namespace Investmogilev.UI.Portal.Models
{
    public class DocumentForTaskViewModel : DocumentAdditionalInfo
    {
        public DocumentForTaskViewModel()
        {
        }

        public DocumentForTaskViewModel(DocumentAdditionalInfo info)
        {
            Id = info.Id;
            FilePath = info.FilePath;
            InfoName = info.InfoName;
        }

        public string ProjectId { get; set; }
        public string TaskId { get; set; }
    }
}