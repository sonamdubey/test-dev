CREATE TABLE [dbo].[TC_DealerAdminMappingLog] (
    [Id]                      INT      IDENTITY (1, 1) NOT NULL,
    [TC_DealerAdminMappingId] INT      NULL,
    [DealerAdminId]           INT      NULL,
    [DealerId]                INT      NULL,
    [IsGroup]                 INT      NULL,
    [CreatedOn]               DATETIME NULL,
    [DeletedOn]               DATETIME DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

