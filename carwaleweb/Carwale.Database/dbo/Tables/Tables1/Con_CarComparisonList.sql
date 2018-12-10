CREATE TABLE [dbo].[Con_CarComparisonList] (
    [ID]              NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [VersionId1]      NUMERIC (18)  NOT NULL,
    [VersionId2]      NUMERIC (18)  NOT NULL,
    [EntryDate]       DATETIME      NOT NULL,
    [IsActive]        BIT           CONSTRAINT [DF_Con_CarComparisonList_IsActive] DEFAULT ((0)) NOT NULL,
    [HostURL]         VARCHAR (250) CONSTRAINT [DF_Con_CarComparisonList_HostURL] DEFAULT ('img.carwale.com') NULL,
    [IsReplicated]    BIT           CONSTRAINT [DF_Con_CarComparisonList_IsReplicated] DEFAULT ((0)) NULL,
    [DisplayPriority] INT           NULL,
    [ImagePath]       VARCHAR (500) NULL,
    [ImageName]       VARCHAR (500) NULL,
    [IsArchived]      BIT           CONSTRAINT [Con_CarComparisonList_IsArchived] DEFAULT ((0)) NULL,
    [OriginalImgPath] VARCHAR (500) NULL,
    [IsSponsored]     BIT           NULL,
    CONSTRAINT [PK_Con_CarComparisonList] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

