CREATE TABLE [dbo].[CallbackRequests] (
    [Id]              NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarId]           NUMERIC (18)  NOT NULL,
    [CustomerName]    VARCHAR (50)  NOT NULL,
    [ContactNo]       VARCHAR (50)  NOT NULL,
    [EMailId]         VARCHAR (100) NOT NULL,
    [Address]         VARCHAR (100) NULL,
    [City]            VARCHAR (50)  NOT NULL,
    [Comments]        VARCHAR (500) NULL,
    [RequestDateTime] DATETIME      NOT NULL,
    [IsForwarded]     BIT           CONSTRAINT [DF_CallbackRequests_IsForwarded] DEFAULT (0) NOT NULL,
    [IsRejected]      BIT           CONSTRAINT [DF_CallbackRequests_IsRejected] DEFAULT (0) NOT NULL,
    [IsViewed]        BIT           CONSTRAINT [DF_CallbackRequests_IsViewed] DEFAULT (0) NOT NULL,
    [IsMailSend]      BIT           CONSTRAINT [DF_CallbackRequests_IsMailSend] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_CallbackRequests] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

