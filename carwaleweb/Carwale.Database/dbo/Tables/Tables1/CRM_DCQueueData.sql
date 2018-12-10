CREATE TABLE [dbo].[CRM_DCQueueData] (
    [Id]               NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CBDId]            BIGINT       NOT NULL,
    [PQStatus]         SMALLINT     NULL,
    [PQSubDisposition] INT          NULL,
    [PQDate]           DATETIME     NULL,
    [TDStatus]         SMALLINT     NULL,
    [TDSubDisposition] INT          NULL,
    [TDDate]           DATETIME     NULL,
    [BLStatus]         SMALLINT     NULL,
    [BLSubDisposition] INT          NULL,
    [BLDate]           DATETIME     NULL,
    [RegisterWith]     VARCHAR (50) NULL,
    [CarColor]         VARCHAR (20) NULL,
    [Invoice]          INT          NULL,
    [IsProcessed]      BIT          NOT NULL,
    [UpdatedBy]        BIGINT       NULL,
    [UpdatedOn]        DATETIME     NULL,
    [CreatedOn]        DATETIME     CONSTRAINT [DF_CRM_DCQueueData_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]        BIGINT       NOT NULL,
    CONSTRAINT [PK_CRM_DCQueueData] PRIMARY KEY CLUSTERED ([Id] ASC)
);

