CREATE TABLE [dbo].[ChatManagement] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [PageName]  VARCHAR (500) NOT NULL,
    [IsChatOn]  BIT           NOT NULL,
    [IsActive]  BIT           CONSTRAINT [DF_ChatManagement_IsActive] DEFAULT ((1)) NOT NULL,
    [UpdatedOn] DATETIME      CONSTRAINT [DF_ChatManagement_UpdatedOn] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy] INT           NOT NULL,
    CONSTRAINT [PK_ChatManagement] PRIMARY KEY CLUSTERED ([Id] ASC)
);

