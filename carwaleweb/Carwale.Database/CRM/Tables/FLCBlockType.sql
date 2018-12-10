CREATE TABLE [CRM].[FLCBlockType] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [ModelId]     NUMERIC (18) NOT NULL,
    [CityId]      NUMERIC (18) NOT NULL,
    [Type]        TINYINT      NOT NULL,
    [UpdatedDate] DATETIME     NULL,
    [UpdatedBy]   NUMERIC (18) NULL,
    CONSTRAINT [PK_FLCBlockType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

