CREATE TABLE [dbo].[Support_TicketStatus] (
    [Id]     SMALLINT      IDENTITY (1, 1) NOT NULL,
    [Status] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Support_TicketStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

