CREATE TABLE [CRM].[QAFatalType] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [FatalType] VARCHAR (500) NOT NULL,
    [IsActive]  BIT           CONSTRAINT [DF_QAFatalType_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedBy] NUMERIC (18)  NOT NULL,
    [CreatedOn] DATETIME      CONSTRAINT [DF_QAFatalType_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy] NUMERIC (18)  NULL,
    [UpDatedOn] DATETIME      NULL,
    CONSTRAINT [PK_QAFatalType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

