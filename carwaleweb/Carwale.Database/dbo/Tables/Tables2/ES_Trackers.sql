CREATE TABLE [dbo].[ES_Trackers] (
    [ES_Trackers_Id] INT          IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (50) NULL,
    CONSTRAINT [PK_ES_Trackers] PRIMARY KEY CLUSTERED ([ES_Trackers_Id] ASC)
);

