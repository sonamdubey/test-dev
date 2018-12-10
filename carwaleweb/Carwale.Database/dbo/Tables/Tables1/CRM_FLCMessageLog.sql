CREATE TABLE [dbo].[CRM_FLCMessageLog] (
    [Id]           NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [FLCMessageId] NUMERIC (18)   NOT NULL,
    [MakeId]       NUMERIC (18)   NULL,
    [ModelId]      NUMERIC (18)   NULL,
    [UpdatedBy]    NUMERIC (18)   NOT NULL,
    [UpdatedOn]    DATETIME       NOT NULL,
    [Message]      VARCHAR (1500) NULL,
    [StateId]      NUMERIC (18)   NULL,
    [CityId]       NUMERIC (18)   NULL,
    CONSTRAINT [PK_CRM_FLCMessageLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

