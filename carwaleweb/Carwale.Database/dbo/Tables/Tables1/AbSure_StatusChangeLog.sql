CREATE TABLE [dbo].[AbSure_StatusChangeLog] (
    [Id]                  NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [AbSure_CarDetailsId] NUMERIC (18) NOT NULL,
    [Status]              INT          NOT NULL,
    [PreviousStatus]      INT          NULL,
    [ModifiedBy]          INT          NOT NULL,
    [ModifiedDate]        DATETIME     CONSTRAINT [DF_AbSure_StatusChangeLog_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [IsModified]          BIT          CONSTRAINT [DF_AbSure_StatusChangeLog_IsModified] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_AbSure_StatusChangeLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

