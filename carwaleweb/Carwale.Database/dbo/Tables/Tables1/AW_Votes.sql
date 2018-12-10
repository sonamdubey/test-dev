CREATE TABLE [dbo].[AW_Votes] (
    [ID]              NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId]      NUMERIC (18)  NULL,
    [ModelId]         NUMERIC (18)  NULL,
    [CategoryId]      NUMERIC (18)  NULL,
    [VotingDate]      DATETIME      NULL,
    [FuelEconomy]     SMALLINT      NULL,
    [Maintenance]     SMALLINT      NULL,
    [Style]           SMALLINT      NULL,
    [Performance]     SMALLINT      NULL,
    [CarSpace]        SMALLINT      NULL,
    [Comfort]         SMALLINT      NULL,
    [Safety]          SMALLINT      NULL,
    [Features]        SMALLINT      NULL,
    [ASServices]      SMALLINT      NULL,
    [ORCapabilities]  SMALLINT      NULL,
    [Maneuverability] SMALLINT      NULL,
    [BrandValue]      SMALLINT      NULL,
    [Description]     VARCHAR (500) NULL,
    [CustomerName]    VARCHAR (200) NULL,
    [CityId]          NUMERIC (18)  NULL,
    [MobileNo]        VARCHAR (50)  NULL,
    [LandLineNo]      VARCHAR (50)  NULL,
    [IPAddress]       VARCHAR (50)  NULL,
    [HWDriving]       SMALLINT      NULL,
    CONSTRAINT [PK_AW_Votes] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Value For Money', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AW_Votes', @level2type = N'COLUMN', @level2name = N'Features';

