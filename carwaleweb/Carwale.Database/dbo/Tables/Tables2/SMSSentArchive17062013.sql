CREATE TABLE [dbo].[SMSSentArchive17062013] (
    [ID]              NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Number]          VARCHAR (50)  NOT NULL,
    [Message]         VARCHAR (500) NOT NULL,
    [ServiceType]     INT           NOT NULL,
    [SMSSentDateTime] DATETIME      NOT NULL,
    [Successfull]     BIT           NOT NULL,
    [ReturnedMsg]     VARCHAR (500) NULL,
    [SMSPageUrl]      VARCHAR (500) NULL,
    [ServiceProvider] VARCHAR (250) NULL,
    [IsSMSSent]       BIT           NULL,
    CONSTRAINT [PK_SMSSent] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_SMSSent_Number]
    ON [dbo].[SMSSentArchive17062013]([Number] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SMSSent_SMSSentDateTime]
    ON [dbo].[SMSSentArchive17062013]([SMSSentDateTime] ASC);

