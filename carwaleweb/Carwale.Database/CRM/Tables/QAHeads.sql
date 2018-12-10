CREATE TABLE [CRM].[QAHeads] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [RoleId]      INT           NOT NULL,
    [HeadName]    VARCHAR (250) NOT NULL,
    [TotalWeight] FLOAT (53)    NOT NULL,
    [IsActive]    BIT           CONSTRAINT [DF_QAHeads_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedBy]   NUMERIC (18)  NOT NULL,
    [CreatedOn]   DATETIME      CONSTRAINT [DF_QAHeads_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]   NUMERIC (18)  NULL,
    [UpdatedOn]   DATETIME      NULL,
    CONSTRAINT [PK_QAHeads] PRIMARY KEY CLUSTERED ([Id] ASC)
);

