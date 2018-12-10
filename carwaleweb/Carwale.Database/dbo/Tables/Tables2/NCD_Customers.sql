CREATE TABLE [dbo].[NCD_Customers] (
    [Id]             INT          IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (70) NULL,
    [Email]          VARCHAR (70) NOT NULL,
    [Mobile]         VARCHAR (10) NULL,
    [EntryDate]      DATETIME     NULL,
    [CustomerSource] TINYINT      NULL,
    [IsActive]       BIT          NULL,
    CONSTRAINT [PK_NCD_Customers] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Customers_Email] UNIQUE NONCLUSTERED ([Email] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1=NCD,2=CarWale', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NCD_Customers', @level2type = N'COLUMN', @level2name = N'CustomerSource';

