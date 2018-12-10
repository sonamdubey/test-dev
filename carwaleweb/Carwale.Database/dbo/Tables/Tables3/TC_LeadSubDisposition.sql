CREATE TABLE [dbo].[TC_LeadSubDisposition] (
    [TC_LeadSubDispositionId] SMALLINT      IDENTITY (1, 1) NOT NULL,
    [SubDispositionName]      VARCHAR (100) NULL,
    [TC_LeadDispositionId]    TINYINT       NULL,
    [IsActive]                BIT           CONSTRAINT [DF_TC_LeadSubDisposition_IsActive] DEFAULT ((1)) NULL,
    [IsVisibleCW]             BIT           NULL,
    [IsVisibleBW]             BIT           NULL,
    CONSTRAINT [PK_TC_LeadSubDispositionID] PRIMARY KEY NONCLUSTERED ([TC_LeadSubDispositionId] ASC)
);

