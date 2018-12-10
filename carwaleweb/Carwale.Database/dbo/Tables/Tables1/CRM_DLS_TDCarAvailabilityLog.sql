CREATE TABLE [dbo].[CRM_DLS_TDCarAvailabilityLog] (
    [Id]                NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CreatedByDealerId] BIGINT       NOT NULL,
    [CreatedOn]         DATETIME     CONSTRAINT [DF_CRM_DLS_TDCarAvailabilityLog_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [IsInsert]          BIT          NULL,
    [IsDelete]          BIT          NULL,
    [ModelId]           NUMERIC (18) NOT NULL,
    [FuelType]          SMALLINT     NOT NULL,
    [TransmissionType]  SMALLINT     NOT NULL,
    CONSTRAINT [PK_CRM_DLS_TDCarAvailabilityLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

