CREATE TABLE [dbo].[NCD_DealerRules] (
    [Id]           NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [DealerId]     NUMERIC (18)   NOT NULL,
    [StateId]      NVARCHAR (200) NOT NULL,
    [CityId]       NVARCHAR (200) NOT NULL,
    [MakeId]       NUMERIC (18)   NOT NULL,
    [ModelId]      NVARCHAR (200) NULL,
    [IsModelBased] BIT            NOT NULL,
    [CreatedOn]    DATETIME       NOT NULL,
    [UpdatedOn]    DATETIME       NULL,
    [UpdatedBy]    NUMERIC (18)   NULL,
    [ZoneId]       VARCHAR (200)  NULL,
    CONSTRAINT [PK_NCD_DealerRules] PRIMARY KEY CLUSTERED ([Id] ASC)
);

