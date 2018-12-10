CREATE TABLE [dbo].[FB_SkodaDealers] (
    [Id]         NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [DealerName] VARCHAR (100) NOT NULL,
    [DealerCode] VARCHAR (50)  NULL,
    CONSTRAINT [PK_FB_SkodaDealers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

