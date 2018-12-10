CREATE TABLE [dbo].[TC_LeadBasedCallDetails] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [LeadId]      BIGINT        NULL,
    [LeadName]    VARCHAR (50)  NULL,
    [CallType]    INT           NULL,
    [CallTime]    DATETIME      NULL,
    [Duration]    VARCHAR (255) NULL,
    [PhoneNumber] VARCHAR (50)  NULL,
    [BranchId]    INT           NULL,
    [UpdatedOn]   DATETIME      NULL,
    [UpdatedBy]   INT           NULL,
    CONSTRAINT [PK__TC_LeadB__3214EC272D025EE1] PRIMARY KEY CLUSTERED ([ID] ASC)
);

