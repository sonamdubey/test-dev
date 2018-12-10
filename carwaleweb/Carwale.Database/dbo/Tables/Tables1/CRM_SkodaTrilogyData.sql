CREATE TABLE [dbo].[CRM_SkodaTrilogyData] (
    [Id]           NUMERIC (18) NOT NULL,
    [LeadDate]     DATETIME     NOT NULL,
    [LeadId]       VARCHAR (50) NULL,
    [CustomerName] VARCHAR (50) NULL,
    [BookingName]  VARCHAR (50) NULL,
    [DealerName]   VARCHAR (50) NOT NULL,
    [DealerId]     NUMERIC (18) NOT NULL,
    [CityId]       NUMERIC (18) NOT NULL,
    [Mobile]       VARCHAR (20) NULL,
    [CarModel]     VARCHAR (20) NOT NULL,
    [ModelId]      NUMERIC (18) NOT NULL,
    [VersionId]    NUMERIC (18) NOT NULL,
    [TokenNo]      VARCHAR (20) NULL,
    CONSTRAINT [PK_CRM_SkodaTrilogyData] PRIMARY KEY CLUSTERED ([Id] ASC)
);

