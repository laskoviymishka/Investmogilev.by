// // -----------------------------------------------------------------------
// // <copyright file="IDocumentSendingUoW.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

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