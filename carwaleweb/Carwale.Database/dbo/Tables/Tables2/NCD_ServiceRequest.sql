CREATE TABLE [dbo].[NCD_ServiceRequest] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [DealerId]      INT           NOT NULL,
    [CustomerId]    INT           NOT NULL,
    [VersionId]     INT           NOT NULL,
    [RegNo]         VARCHAR (50)  NULL,
    [PreferredDate] DATETIME      NULL,
    [RequestDate]   DATETIME      NOT NULL,
    [TypeOfService] SMALLINT      NULL,
    [Comments]      VARCHAR (250) NULL,
    CONSTRAINT [PK_NCD_ServiceRequest] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1=Regular Car Maintenance Service,2=Specific Repair/Maintenance Service', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NCD_ServiceRequest', @level2type = N'COLUMN', @level2name = N'TypeOfService';

