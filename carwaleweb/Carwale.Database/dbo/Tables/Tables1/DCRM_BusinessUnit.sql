CREATE TABLE [dbo].[DCRM_BusinessUnit] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (10)  NOT NULL,
    [Description] VARCHAR (100) NOT NULL,
    [IsActive]    BIT           NOT NULL,
    [AddedOn]     DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

