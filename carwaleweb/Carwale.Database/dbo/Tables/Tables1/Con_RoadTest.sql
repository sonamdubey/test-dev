CREATE TABLE [dbo].[Con_RoadTest] (
    [ID]           NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarModelID]   NUMERIC (18)   NOT NULL,
    [CarVersionID] NUMERIC (18)   NULL,
    [Title]        VARCHAR (250)  NOT NULL,
    [DisplayDate]  DATETIME       NOT NULL,
    [AuthorName]   VARCHAR (100)  NOT NULL,
    [MainImgPath]  VARCHAR (100)  NULL,
    [Pros]         VARCHAR (500)  NULL,
    [Cons]         VARCHAR (500)  NULL,
    [EntryDate]    DATETIME       NOT NULL,
    [IsActive]     BIT            CONSTRAINT [DF_Con_RoadTest_IsActive] DEFAULT ((1)) NULL,
    [IsPublished]  BIT            CONSTRAINT [DF_Con_RoadTest_IsPublished] DEFAULT ((0)) NULL,
    [IsType]       SMALLINT       CONSTRAINT [DF_Con_RoadTest_IsType] DEFAULT ((0)) NULL,
    [Description]  VARCHAR (2000) NULL,
    [IsReplicated] BIT            CONSTRAINT [DF__Con_RoadT__IsRep__4060176F] DEFAULT ((0)) NULL,
    [HostURL]      VARCHAR (100)  DEFAULT ('img.carwale.com') NULL,
    CONSTRAINT [PK_Con_RoadTest] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0 - Roaad test, 1 - First drive', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Con_RoadTest', @level2type = N'COLUMN', @level2name = N'IsType';

