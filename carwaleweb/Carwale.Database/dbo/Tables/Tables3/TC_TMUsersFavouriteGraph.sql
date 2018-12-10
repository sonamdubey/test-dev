CREATE TABLE [dbo].[TC_TMUsersFavouriteGraph] (
    [TC_TMUsersFavouriteGraphId] INT           IDENTITY (1, 1) NOT NULL,
    [TC_SpecialUsersId]          INT           NULL,
    [FavoriteGraphURL]           VARCHAR (150) NULL,
    [IsActive]                   BIT           CONSTRAINT [DF_TC_TMUsersFavouriteGraph_IsActive] DEFAULT ((1)) NULL,
    [CreatedOn]                  DATETIME      CONSTRAINT [DF_TC_TMUsersFavouriteGraph_CreatedOn] DEFAULT (getdate()) NULL,
    [LastUpdatedOn]              DATETIME      CONSTRAINT [DF_TC_TMUsersFavouriteGraph_LastUpdatedOn] DEFAULT (getdate()) NULL,
    [GraphName]                  VARCHAR (150) NULL,
    CONSTRAINT [PK_TC_TMUsersFavouriteGraphID] PRIMARY KEY NONCLUSTERED ([TC_TMUsersFavouriteGraphId] ASC)
);

