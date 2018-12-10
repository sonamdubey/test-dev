CREATE TABLE [dbo].[Microsite_ModelBodyStyles] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [BodyStyleName] VARCHAR (100) NOT NULL,
    [IsActive]      BIT           CONSTRAINT [DF_Microsite_ModelBodyStyles_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Microsite_ModelBodyStyles] PRIMARY KEY CLUSTERED ([Id] ASC)
);

