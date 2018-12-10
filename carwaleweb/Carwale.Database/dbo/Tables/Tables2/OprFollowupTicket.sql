CREATE TABLE [dbo].[OprFollowupTicket] (
    [ID]                 NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [TicketId]           NUMERIC (18)  NOT NULL,
    [Comments]           VARCHAR (500) NOT NULL,
    [SubmissionDate]     DATETIME      NOT NULL,
    [NextFollowupDate]   DATETIME      NOT NULL,
    [followedById]       NUMERIC (18)  NOT NULL,
    [StatusId]           NUMERIC (18)  NOT NULL,
    [SubmissionDatePart] DATETIME      CONSTRAINT [DF_OprFollowupTicket_SubmissionDatePart] DEFAULT (CONVERT([varchar],getdate(),(1))) NULL,
    CONSTRAINT [PK_OprFollowupTicket] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [Idx_OprFollowupTicket_FById_SubDPart]
    ON [dbo].[OprFollowupTicket]([followedById] ASC, [SubmissionDatePart] ASC)
    INCLUDE([ID], [Comments], [SubmissionDate], [NextFollowupDate]);


GO
CREATE NONCLUSTERED INDEX [ix_OprFollowupTicket__TicketId]
    ON [dbo].[OprFollowupTicket]([TicketId] ASC);

