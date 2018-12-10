CREATE TABLE [dbo].[CRM_CarDeliveryData] (
    [Id]                   NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarBasicDataId]       NUMERIC (18)   NOT NULL,
    [DeliveryStatusId]     INT            NOT NULL,
    [ExpectedDeliveryDate] DATETIME       NULL,
    [ActualDeliveryDate]   DATETIME       NULL,
    [DealerId]             NUMERIC (18)   NULL,
    [ContactPerson]        VARCHAR (50)   NULL,
    [Contact]              VARCHAR (50)   NULL,
    [ChasisNumber]         VARCHAR (50)   NULL,
    [EngineNumber]         VARCHAR (50)   NULL,
    [Color]                VARCHAR (100)  NULL,
    [RegistrationNumber]   VARCHAR (50)   NULL,
    [DeliveryComments]     VARCHAR (1000) NULL,
    [CreatedOn]            DATETIME       NOT NULL,
    [UpdatedOn]            DATETIME       NOT NULL,
    [UpdatedBy]            NUMERIC (18)   NOT NULL,
    [CarInvoiceDate]       DATETIME       NULL,
    [RegPersonName]        VARCHAR (100)  NULL,
    CONSTRAINT [PK_CRM_CarDeliveryData] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_CRM_CarDeliveryData_CRM_CarBasicData] FOREIGN KEY ([CarBasicDataId]) REFERENCES [dbo].[CRM_CarBasicData] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CarDeliveryData_CarBasicDataId]
    ON [dbo].[CRM_CarDeliveryData]([CarBasicDataId] ASC);

