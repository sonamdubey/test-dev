CREATE TABLE [dbo].[AbSure_RejectionReasons] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Reason]   VARCHAR (MAX) NULL,
    [IsActive] BIT           NULL,
    CONSTRAINT [PK_AbSure_RejectionReasons] PRIMARY KEY CLUSTERED ([Id] ASC)
);

