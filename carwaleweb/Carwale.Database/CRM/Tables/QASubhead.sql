CREATE TABLE [CRM].[QASubhead] (
    [Id]          NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [HeadId]      INT           NOT NULL,
    [SubheadName] VARCHAR (150) NOT NULL,
    [Weight]      FLOAT (53)    NOT NULL,
    [CreatedOn]   DATETIME      CONSTRAINT [DF_QASubhead_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]   NUMERIC (18)  NOT NULL,
    [UpdatedOn]   DATETIME      NULL,
    [UpdateBy]    NUMERIC (18)  NULL,
    [IsActive]    BIT           CONSTRAINT [DF_QASubhead_IsActive] DEFAULT ((1)) NOT NULL,
    [Type]        VARCHAR (10)  NULL,
    CONSTRAINT [PK_QASubhead] PRIMARY KEY CLUSTERED ([Id] ASC)
);

