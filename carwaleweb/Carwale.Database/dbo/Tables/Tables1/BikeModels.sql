CREATE TABLE [dbo].[BikeModels] (
    [ID]                  NUMERIC (18)    NOT NULL,
    [Name]                VARCHAR (30)    NOT NULL,
    [BikeMakeId]          NUMERIC (18)    NOT NULL,
    [IsDeleted]           BIT             CONSTRAINT [DF_BikeModels_IsDeleted] DEFAULT ((0)) NOT NULL,
    [Used]                BIT             CONSTRAINT [DF_BikeModels_Used] DEFAULT ((1)) NOT NULL,
    [New]                 BIT             CONSTRAINT [DF_BikeModels_New] DEFAULT ((1)) NOT NULL,
    [Indian]              BIT             CONSTRAINT [DF_BikeModels_Indian] DEFAULT ((1)) NOT NULL,
    [Imported]            BIT             CONSTRAINT [DF_BikeModels_Imported] DEFAULT ((0)) NOT NULL,
    [Futuristic]          BIT             CONSTRAINT [DF_BikeModels_Futuristic] DEFAULT ((0)) NOT NULL,
    [Classic]             BIT             CONSTRAINT [DF_BikeModels_Classic] DEFAULT ((0)) NOT NULL,
    [Modified]            BIT             CONSTRAINT [DF_BikeModels_Modified] DEFAULT ((0)) NOT NULL,
    [ReviewRate]          DECIMAL (18, 2) CONSTRAINT [DF_BikeModels_ReviewRate] DEFAULT ((0)) NULL,
    [ReviewCount]         NUMERIC (18)    CONSTRAINT [DF_BikeModels_ReviewCount] DEFAULT ((0)) NULL,
    [Looks]               DECIMAL (18, 2) NULL,
    [Performance]         DECIMAL (18, 2) NULL,
    [Comfort]             DECIMAL (18, 2) NULL,
    [ValueForMoney]       DECIMAL (18, 2) NULL,
    [FuelEconomy]         DECIMAL (18, 2) NULL,
    [SmallPic]            VARCHAR (50)    NULL,
    [LargePic]            VARCHAR (50)    NULL,
    [MoCreatedOn]         DATETIME        NULL,
    [MoUpdatedBy]         NUMERIC (15)    NULL,
    [MoUpdatedOn]         DATETIME        NULL,
    [HostURL]             VARCHAR (100)   NULL,
    [IsReplicated]        BIT             CONSTRAINT [DF__BikeModels__IsRep__6C098F83] DEFAULT ((0)) NULL,
    [VersionCount]        SMALLINT        NULL,
    [MaskingName]         VARCHAR (50)    NULL,
    [BikeSeriesId]        INT             NULL,
    [BikeClassSegmentsId] INT             NULL,
    [TopVersionId]        INT             NULL,
    [MinPrice]            INT             NULL,
    [MaxPrice]            INT             NULL,
    [OriginalImagePath]   VARCHAR (150)   NULL,
    CONSTRAINT [PK_BikeModels] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_bikemodels_ISDEleted]
    ON [dbo].[BikeModels]([IsDeleted] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_BikeModels_MaskingName]
    ON [dbo].[BikeModels]([MaskingName] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_BikeModels_Name]
    ON [dbo].[BikeModels]([Name] ASC, [BikeMakeId] ASC);

