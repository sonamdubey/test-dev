CREATE TABLE [dbo].[CW_RenaultLodgyCustomer] (
    [Id]            INT          IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (50) NULL,
    [Email]         VARCHAR (50) NULL,
    [Source]        VARCHAR (50) NULL,
    [EntryDate]     DATETIME     CONSTRAINT [DF_CW_RenaultLodgyCustomer_EntryDate] DEFAULT (getdate()) NULL,
    [CityId]        INT          NULL,
    [ContactNumber] VARCHAR (10) NULL,
    CONSTRAINT [PK_RenaultLodgyCustomer] PRIMARY KEY CLUSTERED ([Id] ASC)
);

