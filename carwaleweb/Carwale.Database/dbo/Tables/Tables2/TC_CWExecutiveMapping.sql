CREATE TABLE [dbo].[TC_CWExecutiveMapping] (
    [TC_CWExecutiveMappingId] INT IDENTITY (1, 1) NOT NULL,
    [OprUserId]               INT NULL,
    [BranchId]                INT NULL,
    CONSTRAINT [PK_TC_CWExecutiveMapping] PRIMARY KEY CLUSTERED ([TC_CWExecutiveMappingId] ASC)
);

