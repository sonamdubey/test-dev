CREATE TABLE [dbo].[TC_BrandZone] (
    [TC_BrandZoneId] NUMERIC (18) NOT NULL,
    [ZoneName]       VARCHAR (50) NOT NULL,
    [MakeId]         INT          NOT NULL,
    [IsActive]       BIT          CONSTRAINT [DF_TC_BrandZone_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_TC_BrandZone] PRIMARY KEY CLUSTERED ([TC_BrandZoneId] ASC)
);

