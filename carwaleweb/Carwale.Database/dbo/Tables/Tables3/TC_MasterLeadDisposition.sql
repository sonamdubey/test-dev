CREATE TABLE [dbo].[TC_MasterLeadDisposition] (
    [TC_MasterLeadDispositionId] INT          IDENTITY (1, 1) NOT NULL,
    [Name]                       VARCHAR (50) NOT NULL,
    [TC_LeadInquiryTypeId]       TINYINT      NOT NULL,
    [IsActive]                   BIT          NOT NULL,
    [IsVisible]                  BIT          NULL,
    [PriorityOrder]              INT          NULL,
    CONSTRAINT [PK_TC_MasterLeadDisposition] PRIMARY KEY CLUSTERED ([TC_MasterLeadDispositionId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The column is added to manage visible funnels on MyTask page', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_MasterLeadDisposition', @level2type = N'COLUMN', @level2name = N'IsVisible';

