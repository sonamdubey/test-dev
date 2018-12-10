CREATE TABLE [dbo].[NCS_PoolHistory] (
    [NCS_PoolDate]      DATE          NOT NULL,
    [NCS_PoolTime]      CHAR (12)     NOT NULL,
    [FLCGroupName]      VARCHAR (250) NULL,
    [NCS_PoolCount]     INT           NULL,
    [NCS_PoolResources] SMALLINT      NULL
);

