CREATE TABLE [dbo].[Media] (
    [ID]               NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [PublisherId]      NUMERIC (18)  NOT NULL,
    [Online]           BIT           NOT NULL,
    [Title]            VARCHAR (250) NOT NULL,
    [Url]              VARCHAR (200) NULL,
    [SmallDescription] VARCHAR (500) NOT NULL,
    [PublishDate]      DATETIME      NOT NULL,
    [CreatedBy]        VARCHAR (100) NOT NULL,
    [CreatedOn]        DATETIME      NOT NULL,
    [IsActive]         BIT           CONSTRAINT [DF_Media_IsActive] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Media] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

