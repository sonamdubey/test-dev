CREATE TABLE [dbo].[TC_DealerAdminMapping] (
    [Id]            INT      IDENTITY (1, 1) NOT NULL,
    [DealerAdminId] INT      NOT NULL,
    [DealerId]      INT      NOT NULL,
    [CreatedOn]     DATETIME CONSTRAINT [DF_TC_DealerAdminMapping_CreatedOn] DEFAULT (getdate()) NULL,
    [IsGroup]       BIT      NULL,
    CONSTRAINT [PK_TC_DealerAdminMapping] PRIMARY KEY CLUSTERED ([Id] ASC)
);

