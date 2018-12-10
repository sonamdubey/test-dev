CREATE TABLE [dbo].[BikeVersions] (
    [ID]                NUMERIC (18)    NOT NULL,
    [Name]              VARCHAR (50)    NULL,
    [BikeModelId]       NUMERIC (18)    NOT NULL,
    [SegmentId]         NUMERIC (18)    NULL,
    [BodyStyleId]       NUMERIC (18)    NULL,
    [smallPic]          VARCHAR (50)    NULL,
    [largePic]          VARCHAR (50)    NULL,
    [IsDeleted]         BIT             CONSTRAINT [DF_BikeVersions_IsDeleted] DEFAULT ((0)) NOT NULL,
    [Used]              BIT             CONSTRAINT [DF_BikeVersions_Used] DEFAULT ((1)) NOT NULL,
    [New]               BIT             CONSTRAINT [DF_BikeVersions_New] DEFAULT ((1)) NOT NULL,
    [Indian]            BIT             CONSTRAINT [DF_BikeVersions_Indian] DEFAULT ((1)) NOT NULL,
    [Imported]          BIT             CONSTRAINT [DF_BikeVersions_Imported] DEFAULT ((0)) NOT NULL,
    [Futuristic]        BIT             CONSTRAINT [DF_BikeVersions_Futuristic] DEFAULT ((0)) NOT NULL,
    [Classic]           BIT             CONSTRAINT [DF_BikeVersions_Classic] DEFAULT ((0)) NOT NULL,
    [Modified]          BIT             CONSTRAINT [DF_BikeVersions_Modified] DEFAULT ((0)) NOT NULL,
    [ReviewRate]        DECIMAL (18, 2) CONSTRAINT [DF_BikeVersions_ReviewRate] DEFAULT ((0)) NULL,
    [ReviewCount]       NUMERIC (18)    CONSTRAINT [DF_BikeVersions_ReviewCount] DEFAULT ((0)) NULL,
    [Looks]             DECIMAL (18, 2) NULL,
    [Performance]       DECIMAL (18, 2) NULL,
    [Comfort]           DECIMAL (18, 2) NULL,
    [ValueForMoney]     DECIMAL (18, 2) NULL,
    [FuelEconomy]       DECIMAL (18, 2) NULL,
    [SubSegmentId]      NUMERIC (18)    NULL,
    [BikeFuelType]      TINYINT         NULL,
    [BikeTransmission]  TINYINT         NULL,
    [VCreatedOn]        DATETIME        NULL,
    [VUpdatedBy]        NUMERIC (15)    NULL,
    [VUpdatedOn]        DATETIME        NULL,
    [HostURL]           VARCHAR (100)   NULL,
    [IsReplicated]      BIT             CONSTRAINT [DF__BikeVersio__IsRep__39B319E0] DEFAULT ((0)) NULL,
    [VersionPrice]      INT             NULL,
    [OriginalImagePath] VARCHAR (150)   NULL,
    CONSTRAINT [PK_BikeVersions] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_bikeversions_ISDEleted]
    ON [dbo].[BikeVersions]([IsDeleted] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_BikeVersions_Name]
    ON [dbo].[BikeVersions]([Name] ASC, [BikeModelId] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1=Petrol; 2=Diesel; 3=CNG;  4=LPG; 5=Electric', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BikeVersions', @level2type = N'COLUMN', @level2name = N'BikeFuelType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1=Automatic; 2=Manual;', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BikeVersions', @level2type = N'COLUMN', @level2name = N'BikeTransmission';

