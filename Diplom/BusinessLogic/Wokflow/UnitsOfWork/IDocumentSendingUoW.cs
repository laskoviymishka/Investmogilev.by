namespace BusinessLogic.Wokflow.UnitsOfWork
{
    public interface IDocumentSendingUoW
    {
        void OnDocumentSendingEntry();
        void OnDocumentSendingExit();
        bool CouldDocumentUpdate();
    }
}