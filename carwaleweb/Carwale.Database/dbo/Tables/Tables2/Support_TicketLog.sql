CREATE TABLE [dbo].[Support_TicketLog] (
    [Id]        NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [TicketId]  NUMERIC (18)   NOT NULL,
    [CreatedOn] DATETIME       NOT NULL,
    [Status]    SMALLINT       NOT NULL,
    [Comment]   VARCHAR (1500) NULL,
    [CreatedBy] NUMERIC (18)   NOT NULL,
    CONSTRAINT [PK_Support_TicketLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'open=1,inprocess=2,closed=3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Support_TicketLog', @level2type = N'COLUMN', @level2name = N'Status';

