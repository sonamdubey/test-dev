CREATE TABLE [dbo].[NCS_PQRefNo] (
    [Id]            NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [PQRefNo]       VARCHAR (50) NULL,
    [UserId]        NUMERIC (18) NOT NULL,
    [VersionId]     INT          NULL,
    [CityId]        INT          NULL,
    [Status]        BIT          CONSTRAINT [DF_NCS_PQRefNo_Status] DEFAULT ((0)) NOT NULL,
    [EntryDateTime] DATETIME     NOT NULL,
    [SourceId]      SMALLINT     NOT NULL,
    [BuyTime]       VARCHAR (50) NULL,
    [IsPrinted]     BIT          NULL,
    CONSTRAINT [PK_NCS_PQRefNo] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 from opr and 2 from icici', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NCS_PQRefNo', @level2type = N'COLUMN', @level2name = N'SourceId';

