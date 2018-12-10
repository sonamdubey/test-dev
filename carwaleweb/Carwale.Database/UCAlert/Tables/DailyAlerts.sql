CREATE TABLE [UCAlert].[DailyAlerts] (
    [DailyAlert_Id]              BIGINT        IDENTITY (1, 1) NOT NULL,
    [UsedCarAlert_Id]            INT           NOT NULL,
    [CustomerAlert_Email]        VARCHAR (100) NOT NULL,
    [Car_CityId]                 SMALLINT      NULL,
    [Car_City]                   VARCHAR (50)  NULL,
    [ProfileId]                  VARCHAR (50)  NOT NULL,
    [Car_SellerId]               SMALLINT      NULL,
    [Car_Price]                  NUMERIC (18)  NULL,
    [Car_MakeId]                 NUMERIC (18)  NULL,
    [Car_ModelId]                NUMERIC (18)  NULL,
    [Car_Make]                   VARCHAR (50)  NULL,
    [Car_Model]                  VARCHAR (50)  NULL,
    [Car_Seller]                 VARCHAR (20)  NULL,
    [Car_Kms]                    VARCHAR (20)  NULL,
    [Car_Version]                VARCHAR (50)  NULL,
    [Car_Year]                   SMALLINT      NULL,
    [Car_Color]                  VARCHAR (100) NULL,
    [Car_HasPhoto]               BIT           NULL,
    [Car_LastUpdated]            DATE          NULL,
    [Is_Mailed]                  INT           CONSTRAINT [DF_DailyAlerts_Is_Mailed] DEFAULT ((0)) NOT NULL,
    [CustomerAlert_City]         VARCHAR (50)  NULL,
    [CustomerAlert_Budget]       VARCHAR (100) NULL,
    [CustomerAlert_MakeYear]     VARCHAR (100) NULL,
    [CustomerAlert_Kms]          VARCHAR (100) NULL,
    [CustomerAlert_Make]         VARCHAR (100) NULL,
    [CustomerAlert_Model]        VARCHAR (100) NULL,
    [CustomerAlert_FuelType]     VARCHAR (100) NULL,
    [CustomerAlert_BodyStyle]    VARCHAR (100) NULL,
    [CustomerAlert_Transmission] VARCHAR (50)  NULL,
    [CustomerAlert_Seller]       VARCHAR (MAX) NULL,
    [alertUrl]                   VARCHAR (MAX) NULL,
    [CreatedOn]                  DATETIME      CONSTRAINT [DF_DailyAlerts_CreatedOn] DEFAULT (getdate()) NULL,
    [IsAlertFromNewDesign]       BIT           NULL
);


GO
CREATE CLUSTERED INDEX [IX_DailyAlerts]
    ON [UCAlert].[DailyAlerts]([UsedCarAlert_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DailyAlerts_IsMailed]
    ON [UCAlert].[DailyAlerts]([Is_Mailed] ASC);

