CREATE TABLE [dbo].[DealerCarRequests] (
    [Id]          NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarId]       NUMERIC (18)  NOT NULL,
    [isDealer]    BIT           NOT NULL,
    [DealerId]    NUMERIC (18)  NOT NULL,
    [Message]     VARCHAR (500) NOT NULL,
    [MsgSentTime] DATETIME      NOT NULL,
    [isActive]    BIT           NOT NULL,
    CONSTRAINT [PK_DealerCarRequests] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

