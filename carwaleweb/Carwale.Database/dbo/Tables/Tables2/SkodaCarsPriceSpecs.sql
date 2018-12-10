CREATE TABLE [dbo].[SkodaCarsPriceSpecs] (
    [Id]                 INT           IDENTITY (1, 1) NOT NULL,
    [VersionId]          INT           NOT NULL,
    [FuelType]           VARCHAR (50)  NULL,
    [Transmission]       VARCHAR (50)  NULL,
    [Transmission_large] VARCHAR (100) NULL,
    [Price]              VARCHAR (50)  NULL,
    [Engine]             VARCHAR (100) NULL,
    [Torque]             VARCHAR (50)  NULL,
    [Displacement]       INT           NULL,
    [Power]              VARCHAR (50)  NULL,
    [IsActive]           BIT           NULL,
    CONSTRAINT [PK_SkodaCarsPriceSpecs] PRIMARY KEY CLUSTERED ([Id] ASC)
);

