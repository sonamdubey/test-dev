CREATE TABLE [dbo].[TC_TMMarketParameter] (
    [TC_TMMargetParameterId]           INT      IDENTITY (1, 1) NOT NULL,
    [Date]                             DATE     NULL,
    [TC_SpecialUsersId]                INT      NULL,
    [Percentage]                       INT      NULL,
    [TC_BrandZoneId]                   SMALLINT NULL,
    [CarModelId]                       INT      NULL,
    [StartMonth]                       INT      NULL,
    [EndMonth]                         INT      NULL,
    [AMId]                             INT      NULL,
    [DealerId]                         INT      NULL,
    [TC_TMDistributionPatternMasterId] INT      NULL,
    PRIMARY KEY CLUSTERED ([TC_TMMargetParameterId] ASC)
);

