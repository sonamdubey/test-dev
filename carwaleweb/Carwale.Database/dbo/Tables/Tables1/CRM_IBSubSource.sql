CREATE TABLE [dbo].[CRM_IBSubSource] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [SubSource] VARCHAR (100) NULL,
    [IsActive]  BIT           DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

