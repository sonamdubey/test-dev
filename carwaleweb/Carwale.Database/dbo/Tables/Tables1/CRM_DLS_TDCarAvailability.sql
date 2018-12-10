CREATE TABLE [dbo].[CRM_DLS_TDCarAvailability] (
    [Id]               NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [ModelId]          INT          NOT NULL,
    [DealerId]         BIGINT       NOT NULL,
    [FuelType]         SMALLINT     NOT NULL,
    [TransmissionType] SMALLINT     NULL,
    [CreatedOn]        DATETIME     CONSTRAINT [DF_CRM_DLS_TDCarAvailability_CreatedOn] DEFAULT (getdate()) NULL,
    [DesiredPlace]     VARCHAR (50) NULL,
    CONSTRAINT [PK_CRM_DLS_TDCarAvailability] PRIMARY KEY CLUSTERED ([Id] ASC)
);

