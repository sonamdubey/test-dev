CREATE TABLE [dbo].[CarCostDepreciation] (
    [Id]           NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [MakeId]       INT            NOT NULL,
    [BodyStyleId]  INT            NOT NULL,
    [SubSegmentId] INT            NOT NULL,
    [FuelType]     VARCHAR (50)   NOT NULL,
    [DepYear1]     DECIMAL (5, 2) NOT NULL,
    [DepYear2]     DECIMAL (5, 2) NOT NULL,
    [DepYear3]     DECIMAL (5, 2) NOT NULL,
    [DepYear4]     DECIMAL (5, 2) NOT NULL,
    [DepYear5]     DECIMAL (5, 2) NOT NULL,
    [IsActive]     BIT            CONSTRAINT [DF_CarCostDepreciation_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CarCostDepreciation] PRIMARY KEY CLUSTERED ([Id] ASC, [BodyStyleId] ASC, [SubSegmentId] ASC, [MakeId] ASC) WITH (FILLFACTOR = 90)
);

