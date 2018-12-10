CREATE TABLE [dbo].[CRM_DeleteReason] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Reason]   VARCHAR (300) NOT NULL,
    [IsActive] BIT           CONSTRAINT [DF_CRM_DeleteReason_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CRM_DeleteReason] PRIMARY KEY CLUSTERED ([Id] ASC)
);

