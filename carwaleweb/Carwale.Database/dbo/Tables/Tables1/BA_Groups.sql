CREATE TABLE [dbo].[BA_Groups] (
    [ID]         BIGINT        IDENTITY (1, 1) NOT NULL,
    [BrokerId]   BIGINT        NOT NULL,
    [GroupName]  VARCHAR (100) NULL,
    [IsActive]   BIT           CONSTRAINT [DF_BA_Groups_IsActive] DEFAULT ((0)) NOT NULL,
    [CreatedOn]  DATETIME      NULL,
    [ModifyDate] DATETIME      NULL,
    [DeletedOn]  DATETIME      NULL,
    CONSTRAINT [PK_BA_Groups] PRIMARY KEY CLUSTERED ([ID] ASC)
);

