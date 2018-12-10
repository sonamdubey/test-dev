CREATE TABLE [dbo].[LDForwardedBackup] (
    [ID]             NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [LeadType]       SMALLINT       NOT NULL,
    [ForwardedTo]    INT            NOT NULL,
    [CustomerName]   VARCHAR (200)  NOT NULL,
    [CustomerEmail]  VARCHAR (100)  NOT NULL,
    [LandLineNo]     VARCHAR (50)   NULL,
    [MobileNo]       VARCHAR (50)   NULL,
    [CityId]         NUMERIC (18)   NOT NULL,
    [RecordId]       NUMERIC (18)   NOT NULL,
    [ForwardCount]   INT            NOT NULL,
    [InternalStatus] SMALLINT       NOT NULL,
    [ADFStatus]      SMALLINT       NOT NULL,
    [ForwardedDate]  DATETIME       NOT NULL,
    [StatusMessage]  VARCHAR (2000) NULL,
    [SourceId]       SMALLINT       NOT NULL
);

