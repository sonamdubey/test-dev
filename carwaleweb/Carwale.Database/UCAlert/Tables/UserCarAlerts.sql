CREATE TABLE [UCAlert].[UserCarAlerts] (
    [UsedCarAlert_Id] INT            IDENTITY (1, 1) NOT NULL,
    [CustomerId]      BIGINT         NULL,
    [Email]           VARCHAR (100)  NOT NULL,
    [CityId]          SMALLINT       NULL,
    [City]            VARCHAR (50)   NULL,
    [CityDistance]    SMALLINT       NULL,
    [BudgetId]        VARCHAR (MAX)  NULL,
    [Budget]          VARCHAR (MAX)  NULL,
    [YearId]          VARCHAR (500)  NULL,
    [MakeYear]        VARCHAR (MAX)  NULL,
    [KmsId]           VARCHAR (500)  NULL,
    [Kms]             VARCHAR (1000) NULL,
    [MakeId]          VARCHAR (MAX)  NULL,
    [Make]            VARCHAR (MAX)  NULL,
    [modelId]         VARCHAR (500)  NULL,
    [Model]           VARCHAR (1000) NULL,
    [FuelTypeId]      VARCHAR (500)  NULL,
    [FuelType]        VARCHAR (1000) NULL,
    [BodyStyleId]     VARCHAR (500)  NULL,
    [BodyStyle]       VARCHAR (1000) NULL,
    [TransmissionId]  VARCHAR (50)   NULL,
    [Transmission]    VARCHAR (50)   NULL,
    [SellerId]        VARCHAR (50)   NULL,
    [Seller]          VARCHAR (50)   NULL,
    [EntryDateTime]   DATE           NULL,
    [LastUpdated]     DATE           CONSTRAINT [DF_Customers_LastUpdated] DEFAULT (getdate()) NULL,
    [EntryDateTicks]  CHAR (18)      NULL,
    [IsActive]        BIT            CONSTRAINT [DF_Customers_IsActive] DEFAULT ((1)) NULL,
    [AlertFrequency]  TINYINT        CONSTRAINT [DF_UserCarAlerts_AlertFrequency] DEFAULT ((1)) NOT NULL,
    [alertUrl]        VARCHAR (MAX)  NULL,
    [IsAutomated]     BIT            DEFAULT ((0)) NULL,
    CONSTRAINT [PK_UsedCarAlertId] PRIMARY KEY CLUSTERED ([UsedCarAlert_Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_UserCarAlerts_CityId]
    ON [UCAlert].[UserCarAlerts]([CityId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserCarAlerts_IsActive]
    ON [UCAlert].[UserCarAlerts]([IsActive] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserCarAlerts_AlertFrequency]
    ON [UCAlert].[UserCarAlerts]([AlertFrequency] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserCarAlerts_EntryDateTime]
    ON [UCAlert].[UserCarAlerts]([EntryDateTime] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserCarAlerts_CustomerId]
    ON [UCAlert].[UserCarAlerts]([CustomerId] ASC);

