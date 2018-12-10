CREATE TABLE [dbo].[ESM_Agency] (
    [id]        NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ClientId]  NUMERIC (18) NOT NULL,
    [AgencyId]  NUMERIC (18) NOT NULL,
    [BrandId]   NUMERIC (18) NOT NULL,
    [IsActive]  BIT          CONSTRAINT [DF_ESM_Agency_IsActive] DEFAULT ((1)) NOT NULL,
    [UpdatedOn] DATETIME     NOT NULL,
    [UpdatedBy] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_ESM_Agency] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 90)
);

