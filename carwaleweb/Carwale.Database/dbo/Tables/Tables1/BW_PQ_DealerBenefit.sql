CREATE TABLE [dbo].[BW_PQ_DealerBenefit] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [DealerId]      INT           NULL,
    [CityId]        INT           NULL,
    [CatId]         SMALLINT      NULL,
    [BenefitText]   VARCHAR (200) NULL,
    [EntryDate]     DATETIME      DEFAULT (getdate()) NULL,
    [LastUpdated]   DATETIME      NULL,
    [LastUpdatedBy] BIGINT        NULL,
    [IsActive]      BIT           DEFAULT ((1)) NULL
);

