CREATE TABLE [dbo].[Absure_RejectedCarReasons] (
    [Id]                  INT      IDENTITY (1, 1) NOT NULL,
    [Absure_CarDetailsId] INT      NOT NULL,
    [RejectedType]        INT      NOT NULL,
    [RejectedReason]      INT      NULL,
    [ReasonDate]          DATETIME CONSTRAINT [DF_Absure_RejectedCarReasons_ReasonDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_Absure_RejectedCarReasons] PRIMARY KEY CLUSTERED ([Id] ASC)
);

