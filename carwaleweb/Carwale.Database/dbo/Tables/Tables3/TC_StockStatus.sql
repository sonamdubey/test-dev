CREATE TABLE [dbo].[TC_StockStatus] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Status]   VARCHAR (100) NOT NULL,
    [IsActive] BIT           CONSTRAINT [DF_TC_Status_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_TC_Status] PRIMARY KEY CLUSTERED ([Id] ASC)
);

