CREATE TABLE [dbo].[Opr_QueryLog] (
    [QueryLog_Id] INT           IDENTITY (1, 1) NOT NULL,
    [Query]       VARCHAR (MAX) NULL,
    [QryFiredBy]  INT           NULL,
    [QryFiredOn]  DATETIME      CONSTRAINT [DF_Opr_QueryLog_QryFiredOn] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_Opr_QueryLog] PRIMARY KEY CLUSTERED ([QueryLog_Id] ASC)
);

