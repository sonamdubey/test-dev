CREATE TABLE [dbo].[Absure_WarrantyStatuses] (
    [Id]          TINYINT       NOT NULL,
    [Status]      VARCHAR (20)  NOT NULL,
    [Description] VARCHAR (200) NULL,
    CONSTRAINT [PK_Absure_WarrantyStatuses] PRIMARY KEY CLUSTERED ([Id] ASC)
);

