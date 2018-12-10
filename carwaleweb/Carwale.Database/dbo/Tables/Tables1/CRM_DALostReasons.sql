CREATE TABLE [dbo].[CRM_DALostReasons] (
    [Id]       NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [LostId]   NUMERIC (18) NOT NULL,
    [LostName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_CRM_DALostReasons] PRIMARY KEY CLUSTERED ([Id] ASC)
);

