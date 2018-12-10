CREATE TABLE [dbo].[TC_Release] (
    [TC_Release_Id] INT            IDENTITY (1, 1) NOT NULL,
    [Content]       VARCHAR (1000) NULL,
    [EntryDate]     DATETIME       CONSTRAINT [DF_TC_Release_EntryDate] DEFAULT (getdate()) NULL,
    [IsActive]      BIT            CONSTRAINT [DF_TC_Release_IsActive] DEFAULT ((1)) NOT NULL,
    [ModifiedDate]  DATETIME       NULL,
    [ModifiedBy]    INT            NULL,
    [DisplayDate]   DATE           NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'primary key', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_Release', @level2type = N'COLUMN', @level2name = N'TC_Release_Id';

