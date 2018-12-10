CREATE TABLE [dbo].[CRM_IB_PurposeOfCall] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [PurposeOfCall] VARCHAR (150) NOT NULL,
    [IsActive]      BIT           CONSTRAINT [DF_CRM_IB_PurposeOfCall_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CRM_IB_PurposeOfCall] PRIMARY KEY CLUSTERED ([Id] ASC)
);

