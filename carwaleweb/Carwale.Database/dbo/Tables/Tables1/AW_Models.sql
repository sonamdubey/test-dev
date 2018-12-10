CREATE TABLE [dbo].[AW_Models] (
    [Id]           NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CategoryId]   NUMERIC (18) NULL,
    [ModelId]      NUMERIC (18) NULL,
    [CarCC]        VARCHAR (50) NULL,
    [MaxPower]     VARCHAR (50) NULL,
    [SeatCapacity] VARCHAR (50) NULL,
    [Segment]      VARCHAR (50) NULL,
    [BodyStyle]    VARCHAR (50) NULL,
    [AliasName]    VARCHAR (50) NULL,
    CONSTRAINT [PK_AW_Models_1] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

