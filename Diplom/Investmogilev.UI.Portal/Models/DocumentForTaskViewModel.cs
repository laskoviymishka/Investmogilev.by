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
            _id = info._id;
            FilePath = info.FilePath;
            InfoName = info.InfoName;
            InfoValue = info.InfoValue;
        }

        public string ProjectId { get; set; }
        public string TaskId { get; set; }
    }
}