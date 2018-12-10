CREATE TABLE [dbo].[BikeMakes] (
    [ID]              NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [Name]            VARCHAR (30)  NOT NULL,
    [IsDeleted]       BIT           CONSTRAINT [DF_BikeMakes_IsActive] DEFAULT ((0)) NOT NULL,
    [LogoUrl]         VARCHAR (100) CONSTRAINT [DF_BikeMakes_LogoUrl] DEFAULT ('nologo.gif') NULL,
    [Used]            BIT           CONSTRAINT [DF_BikeMakes_Used] DEFAULT ((1)) NOT NULL,
    [New]             BIT           CONSTRAINT [DF_BikeMakes_New] DEFAULT ((1)) NOT NULL,
    [Indian]          BIT           CONSTRAINT [DF_BikeMakes_Indian] DEFAULT ((1)) NOT NULL,
    [Imported]        BIT           CONSTRAINT [DF_BikeMakes_Imported] DEFAULT ((0)) NOT NULL,
    [Futuristic]      BIT           CONSTRAINT [DF_BikeMakes_Futuristic] DEFAULT ((0)) NOT NULL,
    [Classic]         BIT           CONSTRAINT [DF_BikeMakes_Classic] DEFAULT ((0)) NOT NULL,
    [Modified]        BIT           CONSTRAINT [DF_BikeMakes_Modified] DEFAULT ((0)) NOT NULL,
    [MaCreatedOn]     DATETIME      NULL,
    [MaUpdatedBy]     NUMERIC (18)  NULL,
    [MaUpdatedOn]     DATETIME      NULL,
    [HostURL]         VARCHAR (100) NULL,
    [IsReplicated]    BIT           CONSTRAINT [DF__BikeMakes__IsRepl__6A214711] DEFAULT ((0)) NULL,
    [IsOEM]           SMALLINT      NULL,
    [ModelCount]      SMALLINT      NULL,
    [MaskingName]     VARCHAR (50)  NULL,
    [PopularityIndex] TINYINT       NULL,
    CONSTRAINT [PK_BikeMakes] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [IX_BikeMakes] UNIQUE NONCLUSTERED ([Name] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_BikeMakes_MaskingName]
    ON [dbo].[BikeMakes]([MaskingName] ASC);

