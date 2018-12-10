CREATE TABLE [dbo].[DLS_CarData] (
    [Id]             NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [LeadId]         NUMERIC (18)  NOT NULL,
    [CarVersionId]   NUMERIC (18)  NOT NULL,
    [PQStatus]       SMALLINT      NULL,
    [TDStatus]       SMALLINT      NULL,
    [BookingStatus]  SMALLINT      NULL,
    [DeliveryStatus] SMALLINT      NULL,
    [LostStatus]     SMALLINT      NULL,
    [CreatedOn]      DATETIME      CONSTRAINT [DF_DLS_CarData_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [Comments]       VARCHAR (500) NULL,
    [UpdatedOn]      DATETIME      NULL,
    [LostReason]     VARCHAR (150) NULL,
    CONSTRAINT [PK_DLS_CarData] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

