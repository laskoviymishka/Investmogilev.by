namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
	public interface IDocumentSendingUoW
	{
		void OnDocumentSendingEntry();
		void OnDocumentSendingExit();
		bool CouldDocumentUpdate();
		bool CouldDocumentUpdateAndLeave();
	}
}