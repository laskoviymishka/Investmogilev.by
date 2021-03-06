// // -----------------------------------------------------------------------
// // <copyright file="ProjectWorkflow.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.Common.State
{
	#region Using

	using Investmogilev.Infrastructure.Common.Model.Common;
	using Investmogilev.Infrastructure.StateMachine;

	#endregion

	public class ProjectWorkflow
	{
		public delegate void EntryExitDelegate();

		public delegate bool GuardClauseDelegate();

		public delegate void UnhandledTriggerDelegate(State state, Trigger trigger);

		public enum State
		{
			[EnumDescription("������ �������������")] Open,
			[EnumDescription("������ ��������� �� �����")] OnMap,
			[EnumDescription("������� ������")] InvestorApprove,
			[EnumDescription("������� ���������")] DocumentSending,
			[EnumDescription("������� ���������� ���������� ���")] WaitInvolved,
			[EnumDescription("������� ������� �� ���������� ���")] InvolvedOrganizations,
			[EnumDescription("������� ������ ��������")] WaitComission,
			[EnumDescription("�� �������")] OnComission,
			[EnumDescription("������� ����������� ��������� ��������� �� ��������")] WaitComissionFixes,
			[EnumDescription("������� ��������")] WaitIspolcom,
			[EnumDescription("�� ���������")] OnIspolcom,
			[EnumDescription("������� ����������� ��������� ���������� �� ���������")] WaitIspolcomFixes,
			[EnumDescription("��������� � ������������ ���������")] InMinEconomy,
			[EnumDescription("��������������� ���� ����������")] PlanCreating,
			[EnumDescription("����������")] Realization,
			[EnumDescription("������ ��������")] Done,
		}

		public enum Trigger
		{
			[EnumDescription("���������� �� �����")] FillInformation,
			[EnumDescription("��������� ����������")] UpdateInformation,
			[EnumDescription("������ � �����")] ReOpen,
			[EnumDescription("������������ ���������")] InvestorResponsed,
			[EnumDescription("������� ���������")] InvestorSelected,
			[EnumDescription("�������� ��������� ����������")] DocumentUpdate,
			[EnumDescription("��������� ���������� ���")] FillInvolvedOrganization,
			[EnumDescription("�������� ��������� ������� ���������� ���")] InvolvedOrganizationUpdate,
			[EnumDescription("��������� �� ��������")] ToComission,
			[EnumDescription("�� ��������")] Comission,
			[EnumDescription("��������� � ���������� ���������� �� ��������")] ComissionFix,
			[EnumDescription("�������� ��������� ��������� ���������� �� ��������")] ComissionFixUpdate,
			[EnumDescription("��������� �� ��������")] ToIspolcom,
			[EnumDescription("�� ��������")] Ispolcom,
			[EnumDescription("��������� � ���������� ���������� �� ���������")] ToIspolcomFix,
			[EnumDescription("�������� ��������� ��������� ���������� �� ���������")] IspolcomFixUpdate,
			[EnumDescription("��������� � ������������ ���������")] ToMinEconomy,
			[EnumDescription("��������� � ��������� ������������ ���������")] MinEconomyResponsed,
			[EnumDescription("�������� ���� ���������� �������")] UpdatePlan,
			[EnumDescription("�������� ���� ���������� �������")] ApprovePlan,
			[EnumDescription("�������� ��������� ���������� �������")] UpdateRealization,
			[EnumDescription("��������� ���������� �������")] RejectDocument,
		}

		private readonly StateMachine<State, Trigger> stateMachine;

		public GuardClauseDelegate GuardClauseFromDocumentSendingToDocumentSendingUsingTriggerDocumentUpdate = null;
		public GuardClauseDelegate GuardClauseFromDocumentSendingToWaitInvolvedUsingTriggerDocumentUpdate = null;
		public GuardClauseDelegate GuardClauseFromInMinEconomyToPlanCreatingUsingTriggerMinEconomyResponsed = null;
		public GuardClauseDelegate GuardClauseFromInvestorApproveToDocumentSendingUsingTriggerInvestorSelected = null;
		public GuardClauseDelegate GuardClauseFromInvestorApproveToInvestorApproveUsingTriggerInvestorResponsed = null;

		public GuardClauseDelegate
			GuardClauseFromInvolvedOrganizationsToInvolvedOrganizationsUsingTriggerInvolvedOrganizationUpdate = null;

		public GuardClauseDelegate GuardClauseFromInvolvedOrganizationsToWaitComissionUsingTriggerToComission = null;
		public GuardClauseDelegate GuardClauseFromOnComissionToOnMapUsingTriggerRejectDocument = null;
		public GuardClauseDelegate GuardClauseFromOnComissionToWaitComissionFixesUsingTriggerComissionFix = null;
		public GuardClauseDelegate GuardClauseFromOnComissionToWaitIspolcomUsingTriggerToIspolcom = null;
		public GuardClauseDelegate GuardClauseFromOnIspolcomToInMinEconomyUsingTriggerToMinEconomy = null;
		public GuardClauseDelegate GuardClauseFromOnIspolcomToOnMapUsingTriggerRejectDocument = null;
		public GuardClauseDelegate GuardClauseFromOnIspolcomToWaitIspolcomFixesUsingTriggerToIspolcomFix = null;
		public GuardClauseDelegate GuardClauseFromOnMapToInvestorApproveUsingTriggerInvestorResponsed = null;
		public GuardClauseDelegate GuardClauseFromOnMapToOnMapUsingTriggerUpdateInformation = null;
		public GuardClauseDelegate GuardClauseFromOnMapToOpenUsingTriggerReOpen = null;
		public GuardClauseDelegate GuardClauseFromOpenToOnMapUsingTriggerFillInformation = null;
		public GuardClauseDelegate GuardClauseFromPlanCreatingToPlanCreatingUsingTriggerUpdatePlan = null;
		public GuardClauseDelegate GuardClauseFromPlanCreatingToRealizationUsingTriggerApprovePlan = null;
		public GuardClauseDelegate GuardClauseFromRealizationToDoneUsingTriggerUpdateRealization = null;
		public GuardClauseDelegate GuardClauseFromRealizationToRealizationUsingTriggerUpdateRealization = null;
		public GuardClauseDelegate GuardClauseFromWaitComissionFixesToOnMapUsingTriggerRejectDocument = null;
		public GuardClauseDelegate GuardClauseFromWaitComissionFixesToWaitComissionFixesUsingTriggerComissionFixUpdate = null;
		public GuardClauseDelegate GuardClauseFromWaitComissionFixesToWaitIspolcomUsingTriggerComissionFixUpdate = null;
		public GuardClauseDelegate GuardClauseFromWaitComissionToOnComissionUsingTriggerComission = null;

		public GuardClauseDelegate GuardClauseFromWaitInvolvedToInvolvedOrganizationsUsingTriggerFillInvolvedOrganization =
			null;

		public GuardClauseDelegate GuardClauseFromWaitIspolcomFixesToOnMapUsingTriggerRejectDocument = null;
		public GuardClauseDelegate GuardClauseFromWaitIspolcomFixesToWaitIspolcomFixesUsingTriggerIspolcomFixUpdate = null;
		public GuardClauseDelegate GuardClauseFromWaitIspolcomFixesToWaitIspolcomUsingTriggerIspolcomFixUpdate = null;
		public GuardClauseDelegate GuardClauseFromWaitIspolcomToOnIspolcomUsingTriggerIspolcom = null;
		public EntryExitDelegate OnDocumentSendingEntry = null;
		public EntryExitDelegate OnDocumentSendingExit = null;
		public EntryExitDelegate OnDoneEntry = null;
		public EntryExitDelegate OnDoneExit = null;
		public EntryExitDelegate OnInMinEconomyEntry = null;
		public EntryExitDelegate OnInMinEconomyExit = null;
		public EntryExitDelegate OnInvestorApproveEntry = null;
		public EntryExitDelegate OnInvestorApproveExit = null;
		public EntryExitDelegate OnInvolvedOrganizationsEntry = null;
		public EntryExitDelegate OnInvolvedOrganizationsExit = null;
		public EntryExitDelegate OnOnComissionEntry = null;
		public EntryExitDelegate OnOnComissionExit = null;
		public EntryExitDelegate OnOnIspolcomEntry = null;
		public EntryExitDelegate OnOnIspolcomExit = null;
		public EntryExitDelegate OnOnMapEntry = null;
		public EntryExitDelegate OnOnMapExit = null;
		public EntryExitDelegate OnOpenEntry = null;
		public EntryExitDelegate OnOpenExit = null;
		public EntryExitDelegate OnPlanCreatingEntry = null;
		public EntryExitDelegate OnPlanCreatingExit = null;
		public EntryExitDelegate OnRealizationEntry = null;
		public EntryExitDelegate OnRealizationExit = null;
		public UnhandledTriggerDelegate OnUnhandledTrigger = null;
		public EntryExitDelegate OnWaitComissionEntry = null;
		public EntryExitDelegate OnWaitComissionExit = null;
		public EntryExitDelegate OnWaitComissionFixesEntry = null;
		public EntryExitDelegate OnWaitComissionFixesExit = null;
		public EntryExitDelegate OnWaitInvolvedEntry = null;
		public EntryExitDelegate OnWaitInvolvedExit = null;
		public EntryExitDelegate OnWaitIspolcomEntry = null;
		public EntryExitDelegate OnWaitIspolcomExit = null;
		public EntryExitDelegate OnWaitIspolcomFixesEntry = null;
		public EntryExitDelegate OnWaitIspolcomFixesExit = null;

		public ProjectWorkflow(StateMachine<State, Trigger> outerStateMachine)
		{
			stateMachine = outerStateMachine;
		}

		public ProjectWorkflow(State currentState)
		{
			stateMachine = new StateMachine<State, Trigger>(currentState);
			stateMachine.Configure(State.Open)
				.OnEntry(() => { if (OnOpenEntry != null) OnOpenEntry(); })
				.OnExit(() => { if (OnOpenExit != null) OnOpenExit(); })
				.PermitIf(Trigger.FillInformation, State.OnMap, () =>
				{
					if (GuardClauseFromOpenToOnMapUsingTriggerFillInformation != null)
						return GuardClauseFromOpenToOnMapUsingTriggerFillInformation();
					return true;
				})
				;
			stateMachine.Configure(State.OnMap)
				.OnEntry(() => { if (OnOnMapEntry != null) OnOnMapEntry(); })
				.OnExit(() => { if (OnOnMapExit != null) OnOnMapExit(); })
				.PermitReentryIf(Trigger.UpdateInformation, () =>
				{
					if (GuardClauseFromOnMapToOnMapUsingTriggerUpdateInformation != null)
						return GuardClauseFromOnMapToOnMapUsingTriggerUpdateInformation();
					return true;
				})
				.PermitIf(Trigger.InvestorResponsed, State.InvestorApprove, () =>
				{
					if (GuardClauseFromOnMapToInvestorApproveUsingTriggerInvestorResponsed != null)
						return GuardClauseFromOnMapToInvestorApproveUsingTriggerInvestorResponsed();
					return true;
				})
				.PermitIf(Trigger.ReOpen, State.Open, () =>
				{
					if (GuardClauseFromOnMapToOpenUsingTriggerReOpen != null) return GuardClauseFromOnMapToOpenUsingTriggerReOpen();
					return true;
				})
				;
			stateMachine.Configure(State.InvestorApprove)
				.OnEntry(() => { if (OnInvestorApproveEntry != null) OnInvestorApproveEntry(); })
				.OnExit(() => { if (OnInvestorApproveExit != null) OnInvestorApproveExit(); })
				.PermitReentryIf(Trigger.InvestorResponsed, () =>
				{
					if (GuardClauseFromInvestorApproveToInvestorApproveUsingTriggerInvestorResponsed != null)
						return GuardClauseFromInvestorApproveToInvestorApproveUsingTriggerInvestorResponsed();
					return true;
				})
				.PermitIf(Trigger.InvestorSelected, State.DocumentSending, () =>
				{
					if (GuardClauseFromInvestorApproveToDocumentSendingUsingTriggerInvestorSelected != null)
						return GuardClauseFromInvestorApproveToDocumentSendingUsingTriggerInvestorSelected();
					return true;
				})
				;

			stateMachine.Configure(State.DocumentSending)
				.OnEntry(() => { if (OnDocumentSendingEntry != null) OnDocumentSendingEntry(); })
				.OnExit(() => { if (OnDocumentSendingExit != null) OnDocumentSendingExit(); })
				.PermitIf(Trigger.DocumentUpdate, State.WaitInvolved, () =>
				{
					if (GuardClauseFromDocumentSendingToWaitInvolvedUsingTriggerDocumentUpdate != null)
						return GuardClauseFromDocumentSendingToWaitInvolvedUsingTriggerDocumentUpdate();
					return true;
				})
				.PermitReentryIf(Trigger.DocumentUpdate, () =>
				{
					if (GuardClauseFromDocumentSendingToDocumentSendingUsingTriggerDocumentUpdate != null)
						return GuardClauseFromDocumentSendingToDocumentSendingUsingTriggerDocumentUpdate();
					return true;
				})
				;
			stateMachine.Configure(State.WaitInvolved)
				.OnEntry(() => { if (OnWaitInvolvedEntry != null) OnWaitInvolvedEntry(); })
				.OnExit(() => { if (OnWaitInvolvedExit != null) OnWaitInvolvedExit(); })
				.PermitIf(Trigger.FillInvolvedOrganization, State.InvolvedOrganizations, () =>
				{
					if (GuardClauseFromWaitInvolvedToInvolvedOrganizationsUsingTriggerFillInvolvedOrganization != null)
						return GuardClauseFromWaitInvolvedToInvolvedOrganizationsUsingTriggerFillInvolvedOrganization();
					return true;
				})
				;
			stateMachine.Configure(State.InvolvedOrganizations)
				.OnEntry(() => { if (OnInvolvedOrganizationsEntry != null) OnInvolvedOrganizationsEntry(); })
				.OnExit(() => { if (OnInvolvedOrganizationsExit != null) OnInvolvedOrganizationsExit(); })
				.PermitReentryIf(Trigger.InvolvedOrganizationUpdate, () =>
				{
					if (GuardClauseFromInvolvedOrganizationsToInvolvedOrganizationsUsingTriggerInvolvedOrganizationUpdate != null)
						return GuardClauseFromInvolvedOrganizationsToInvolvedOrganizationsUsingTriggerInvolvedOrganizationUpdate();
					return true;
				})
				.PermitIf(Trigger.ToComission, State.WaitComission, () =>
				{
					if (GuardClauseFromInvolvedOrganizationsToWaitComissionUsingTriggerToComission != null)
						return GuardClauseFromInvolvedOrganizationsToWaitComissionUsingTriggerToComission();
					return true;
				})
				;
			stateMachine.Configure(State.WaitComission)
				.OnEntry(() => { if (OnWaitComissionEntry != null) OnWaitComissionEntry(); })
				.OnExit(() => { if (OnWaitComissionExit != null) OnWaitComissionExit(); })
				.PermitIf(Trigger.Comission, State.OnComission, () =>
				{
					if (GuardClauseFromWaitComissionToOnComissionUsingTriggerComission != null)
						return GuardClauseFromWaitComissionToOnComissionUsingTriggerComission();
					return true;
				})
				;
			stateMachine.Configure(State.OnComission)
				.OnEntry(() => { if (OnOnComissionEntry != null) OnOnComissionEntry(); })
				.OnExit(() => { if (OnOnComissionExit != null) OnOnComissionExit(); })
				.PermitIf(Trigger.RejectDocument, State.OnMap, () =>
				{
					if (GuardClauseFromOnComissionToOnMapUsingTriggerRejectDocument != null)
						return GuardClauseFromOnComissionToOnMapUsingTriggerRejectDocument();
					return true;
				})
				.PermitIf(Trigger.ComissionFix, State.WaitComissionFixes, () =>
				{
					if (GuardClauseFromOnComissionToWaitComissionFixesUsingTriggerComissionFix != null)
						return GuardClauseFromOnComissionToWaitComissionFixesUsingTriggerComissionFix();
					return true;
				})
				.PermitIf(Trigger.ToIspolcom, State.WaitIspolcom, () =>
				{
					if (GuardClauseFromOnComissionToWaitIspolcomUsingTriggerToIspolcom != null)
						return GuardClauseFromOnComissionToWaitIspolcomUsingTriggerToIspolcom();
					return true;
				})
				;
			stateMachine.Configure(State.WaitComissionFixes)
				.OnEntry(() => { if (OnWaitComissionFixesEntry != null) OnWaitComissionFixesEntry(); })
				.OnExit(() => { if (OnWaitComissionFixesExit != null) OnWaitComissionFixesExit(); })
				.PermitIf(Trigger.RejectDocument, State.OnMap, () =>
				{
					if (GuardClauseFromWaitComissionFixesToOnMapUsingTriggerRejectDocument != null)
						return GuardClauseFromWaitComissionFixesToOnMapUsingTriggerRejectDocument();
					return true;
				})
				.PermitReentryIf(Trigger.ComissionFixUpdate, () =>
				{
					if (GuardClauseFromWaitComissionFixesToWaitComissionFixesUsingTriggerComissionFixUpdate != null)
						return GuardClauseFromWaitComissionFixesToWaitComissionFixesUsingTriggerComissionFixUpdate();
					return true;
				})
				.PermitIf(Trigger.ComissionFixUpdate, State.WaitIspolcom, () =>
				{
					if (GuardClauseFromWaitComissionFixesToWaitIspolcomUsingTriggerComissionFixUpdate != null)
						return GuardClauseFromWaitComissionFixesToWaitIspolcomUsingTriggerComissionFixUpdate();
					return true;
				})
				;
			stateMachine.Configure(State.WaitIspolcom)
				.OnEntry(() => { if (OnWaitIspolcomEntry != null) OnWaitIspolcomEntry(); })
				.OnExit(() => { if (OnWaitIspolcomExit != null) OnWaitIspolcomExit(); })
				.PermitIf(Trigger.Ispolcom, State.OnIspolcom, () =>
				{
					if (GuardClauseFromWaitIspolcomToOnIspolcomUsingTriggerIspolcom != null)
						return GuardClauseFromWaitIspolcomToOnIspolcomUsingTriggerIspolcom();
					return true;
				})
				;
			stateMachine.Configure(State.OnIspolcom)
				.OnEntry(() => { if (OnOnIspolcomEntry != null) OnOnIspolcomEntry(); })
				.OnExit(() => { if (OnOnIspolcomExit != null) OnOnIspolcomExit(); })
				.PermitIf(Trigger.RejectDocument, State.OnMap, () =>
				{
					if (GuardClauseFromOnIspolcomToOnMapUsingTriggerRejectDocument != null)
						return GuardClauseFromOnIspolcomToOnMapUsingTriggerRejectDocument();
					return true;
				})
				.PermitIf(Trigger.ToIspolcomFix, State.WaitIspolcomFixes, () =>
				{
					if (GuardClauseFromOnIspolcomToWaitIspolcomFixesUsingTriggerToIspolcomFix != null)
						return GuardClauseFromOnIspolcomToWaitIspolcomFixesUsingTriggerToIspolcomFix();
					return true;
				})
				.PermitIf(Trigger.ToMinEconomy, State.InMinEconomy, () =>
				{
					if (GuardClauseFromOnIspolcomToInMinEconomyUsingTriggerToMinEconomy != null)
						return GuardClauseFromOnIspolcomToInMinEconomyUsingTriggerToMinEconomy();
					return true;
				})
				;
			stateMachine.Configure(State.WaitIspolcomFixes)
				.OnEntry(() => { if (OnWaitIspolcomFixesEntry != null) OnWaitIspolcomFixesEntry(); })
				.OnExit(() => { if (OnWaitIspolcomFixesExit != null) OnWaitIspolcomFixesExit(); })
				.PermitIf(Trigger.RejectDocument, State.OnMap, () =>
				{
					if (GuardClauseFromWaitIspolcomFixesToOnMapUsingTriggerRejectDocument != null)
						return GuardClauseFromWaitIspolcomFixesToOnMapUsingTriggerRejectDocument();
					return true;
				})
				.PermitReentryIf(Trigger.IspolcomFixUpdate, () =>
				{
					if (GuardClauseFromWaitIspolcomFixesToWaitIspolcomFixesUsingTriggerIspolcomFixUpdate != null)
						return GuardClauseFromWaitIspolcomFixesToWaitIspolcomFixesUsingTriggerIspolcomFixUpdate();
					return true;
				})
				.PermitIf(Trigger.IspolcomFixUpdate, State.WaitIspolcom, () =>
				{
					if (GuardClauseFromWaitIspolcomFixesToWaitIspolcomUsingTriggerIspolcomFixUpdate != null)
						return GuardClauseFromWaitIspolcomFixesToWaitIspolcomUsingTriggerIspolcomFixUpdate();
					return true;
				})
				;
			stateMachine.Configure(State.InMinEconomy)
				.OnEntry(() => { if (OnInMinEconomyEntry != null) OnInMinEconomyEntry(); })
				.OnExit(() => { if (OnInMinEconomyExit != null) OnInMinEconomyExit(); })
				.PermitIf(Trigger.MinEconomyResponsed, State.PlanCreating, () =>
				{
					if (GuardClauseFromInMinEconomyToPlanCreatingUsingTriggerMinEconomyResponsed != null)
						return GuardClauseFromInMinEconomyToPlanCreatingUsingTriggerMinEconomyResponsed();
					return true;
				})
				;
			stateMachine.Configure(State.PlanCreating)
				.OnEntry(() => { if (OnPlanCreatingEntry != null) OnPlanCreatingEntry(); })
				.OnExit(() => { if (OnPlanCreatingExit != null) OnPlanCreatingExit(); })
				.PermitReentryIf(Trigger.UpdatePlan, () =>
				{
					if (GuardClauseFromPlanCreatingToPlanCreatingUsingTriggerUpdatePlan != null)
						return GuardClauseFromPlanCreatingToPlanCreatingUsingTriggerUpdatePlan();
					return true;
				})
				.PermitIf(Trigger.ApprovePlan, State.Realization, () =>
				{
					if (GuardClauseFromPlanCreatingToRealizationUsingTriggerApprovePlan != null)
						return GuardClauseFromPlanCreatingToRealizationUsingTriggerApprovePlan();
					return true;
				})
				;
			stateMachine.Configure(State.Realization)
				.OnEntry(() => { if (OnRealizationEntry != null) OnRealizationEntry(); })
				.OnExit(() => { if (OnRealizationExit != null) OnRealizationExit(); })
				.PermitReentryIf(Trigger.UpdateRealization, () =>
				{
					if (GuardClauseFromRealizationToRealizationUsingTriggerUpdateRealization != null)
						return GuardClauseFromRealizationToRealizationUsingTriggerUpdateRealization();
					return true;
				})
				.PermitIf(Trigger.UpdateRealization, State.Done, () =>
				{
					if (GuardClauseFromRealizationToDoneUsingTriggerUpdateRealization != null)
						return GuardClauseFromRealizationToDoneUsingTriggerUpdateRealization();
					return true;
				})
				;
			stateMachine.Configure(State.Done)
				.OnEntry(() => { if (OnDoneEntry != null) OnDoneEntry(); })
				.OnExit(() => { if (OnDoneExit != null) OnDoneExit(); })
				;
			stateMachine.OnUnhandledTrigger(
				(state, trigger) => { if (OnUnhandledTrigger != null) OnUnhandledTrigger(state, trigger); });
		}

		public State GetState
		{
			get { return stateMachine.State; }
		}

		public bool TryFireTrigger(Trigger trigger)
		{
			if (!stateMachine.CanFire(trigger))
			{
				return false;
			}
			stateMachine.Fire(trigger);
			return true;
		}

		public bool CanFire(Trigger trigger)
		{
			return stateMachine.CanFire(trigger);
		}
	}
}