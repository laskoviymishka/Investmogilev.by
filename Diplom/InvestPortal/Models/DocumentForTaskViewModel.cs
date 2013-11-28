using Invest.Common.Model.Common;
namespace InvestPortal.Models
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