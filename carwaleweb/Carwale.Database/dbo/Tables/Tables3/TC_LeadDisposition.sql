CREATE TABLE [dbo].[TC_LeadDisposition] (
    [TC_LeadDispositionId]   TINYINT      NOT NULL,
    [Name]                   VARCHAR (50) NULL,
    [TC_LeadInquiryTypeId]   TINYINT      NULL,
    [IsActive]               BIT          CONSTRAINT [DF_TC_LeadDisposition_IsActive] DEFAULT ((1)) NULL,
    [IsClosed]               BIT          CONSTRAINT [DF_TC_LeadDisposition_IsClosed] DEFAULT ((1)) NULL,
    [IsVisibleCW]            BIT          NULL,
    [IsVisibleBW]            BIT          NULL,
    [TC_MasterDispositionId] INT          NULL,
    CONSTRAINT [PK_TC_LeadStatusDespositionID] PRIMARY KEY NONCLUSTERED ([TC_LeadDispositionId] ASC)
);

