CREATE TABLE [dbo].[Support_Tickets] (
    [Id]               NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [SubcategoryId]    NUMERIC (18)   NOT NULL,
    [InitiatedBy]      NUMERIC (18)   NOT NULL,
    [TicketOwner]      NUMERIC (18)   NOT NULL,
    [TicketDate]       DATETIME       NOT NULL,
    [Status]           SMALLINT       NOT NULL,
    [LastActionDate]   DATETIME       NOT NULL,
    [Comment]          VARCHAR (1500) NULL,
    [ClosingDate]      DATETIME       NULL,
    [Rating]           VARCHAR (50)   NULL,
    [LastComment]      VARCHAR (1500) NULL,
    [ExpecClosingDate] DATETIME       NULL,
    CONSTRAINT [PK_Support_Tickets] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'open=1,inprocess=2,closed=3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Support_Tickets', @level2type = N'COLUMN', @level2name = N'Status';

