CREATE TABLE [dbo].[TC_UserAreaMapping] (
    [TC_UserAreaMappingId] INT      IDENTITY (1, 1) NOT NULL,
    [TC_UserId]            INT      NULL,
    [AreaId]               INT      NULL,
    [IsActive]             BIT      NULL,
    [UpdatedBy]            INT      NULL,
    [UpdatedOn]            DATETIME NULL,
    [EntryDate]            DATETIME NULL,
    [IsAssigned]           BIT      NULL,
    CONSTRAINT [PK_TC_UserAreaMapping] PRIMARY KEY CLUSTERED ([TC_UserAreaMappingId] ASC)
);

