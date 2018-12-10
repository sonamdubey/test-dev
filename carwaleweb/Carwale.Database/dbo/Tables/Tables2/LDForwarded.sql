CREATE TABLE [dbo].[LDForwarded] (
    [ID]             NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [LeadType]       SMALLINT       NOT NULL,
    [ForwardedTo]    INT            NOT NULL,
    [CustomerName]   VARCHAR (200)  NOT NULL,
    [CustomerEmail]  VARCHAR (100)  NOT NULL,
    [LandLineNo]     VARCHAR (50)   NULL,
    [MobileNo]       VARCHAR (50)   NULL,
    [CityId]         NUMERIC (18)   NOT NULL,
    [RecordId]       NUMERIC (18)   NOT NULL,
    [ForwardCount]   INT            CONSTRAINT [DF_LDForwarded_ForwardCount] DEFAULT (1) NOT NULL,
    [InternalStatus] SMALLINT       CONSTRAINT [DF_LDForwarded_InternalStatus] DEFAULT ((-1)) NOT NULL,
    [ADFStatus]      SMALLINT       CONSTRAINT [DF_LDForwarded_ADFStatus] DEFAULT ((-1)) NOT NULL,
    [ForwardedDate]  DATETIME       NOT NULL,
    [StatusMessage]  VARCHAR (2000) NULL,
    [SourceId]       SMALLINT       CONSTRAINT [DF_LDForwarded_SourceId] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_LDForwarded] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_LDForwarded_LeadType]
    ON [dbo].[LDForwarded]([LeadType] ASC, [ForwardedTo] ASC)
    INCLUDE([CustomerName], [CustomerEmail], [LandLineNo], [MobileNo], [CityId], [InternalStatus], [ADFStatus], [ForwardedDate]);


GO
CREATE NONCLUSTERED INDEX [IX_LDForwarded_MobileNo]
    ON [dbo].[LDForwarded]([MobileNo] ASC);

