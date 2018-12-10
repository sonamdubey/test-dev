CREATE TABLE [dbo].[CRM_Dispositions] (
    [Id]          NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [Disposition] VARCHAR (100) NOT NULL,
    [IsActive]    BIT           CONSTRAINT [DF_CRM_Dispositions_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CRM_Dispositions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

