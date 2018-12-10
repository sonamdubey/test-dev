CREATE TABLE [dbo].[CRM_SkodaDealers] (
    [DealerId]       NUMERIC (18) NOT NULL,
    [DealerCode]     VARCHAR (50) NOT NULL,
    [CityId]         NUMERIC (18) NULL,
    [DealerKVPSCode] VARCHAR (50) NULL,
    CONSTRAINT [PK_CRM_SkodaDealers] PRIMARY KEY CLUSTERED ([DealerId] ASC) WITH (FILLFACTOR = 90)
);

