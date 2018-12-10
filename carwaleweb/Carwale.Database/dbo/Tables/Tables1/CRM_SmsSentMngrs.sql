CREATE TABLE [dbo].[CRM_SmsSentMngrs] (
    [Id]                  INT           IDENTITY (1, 1) NOT NULL,
    [Manager Id]          INT           NOT NULL,
    [Name]                NVARCHAR (50) NOT NULL,
    [Mobile No]           NUMERIC (12)  NOT NULL,
    [Assign_Leads]        INT           NOT NULL,
    [PQ_Req]              INT           NOT NULL,
    [TD_Req]              INT           NOT NULL,
    [BQ_Comp]             INT           NOT NULL,
    [SMS Send On]         DATETIME      CONSTRAINT [DF_CRM_SmsSentMngrs_SMS Send On] DEFAULT (getdate()) NOT NULL,
    [Assign_leadsMonthly] INT           NULL,
    [PQ_ReqMonthly]       INT           NULL,
    [TD_ReqMonthly]       INT           NULL,
    [BQ_CompMonthly]      INT           NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is Id of The NCS_RMangers Table. ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CRM_SmsSentMngrs', @level2type = N'COLUMN', @level2name = N'Manager Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Mangers Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CRM_SmsSentMngrs', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Mobile No.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CRM_SmsSentMngrs', @level2type = N'COLUMN', @level2name = N'Mobile No';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Leads assign to that manager on day ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CRM_SmsSentMngrs', @level2type = N'COLUMN', @level2name = N'Assign_Leads';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Number of Price Quote requested for that Manager on day ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CRM_SmsSentMngrs', @level2type = N'COLUMN', @level2name = N'PQ_Req';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Test Drive requested for that dealer on day', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CRM_SmsSentMngrs', @level2type = N'COLUMN', @level2name = N'TD_Req';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Booking Completed for that dealer on day', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CRM_SmsSentMngrs', @level2type = N'COLUMN', @level2name = N'BQ_Comp';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date on which sms send to manager', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CRM_SmsSentMngrs', @level2type = N'COLUMN', @level2name = N'SMS Send On';

