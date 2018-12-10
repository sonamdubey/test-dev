CREATE TABLE [dbo].[CRM_SkodaErrorCodes] (
    [Id]        NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [ErrorCode] VARCHAR (20)  NULL,
    [Stage]     VARCHAR (50)  NULL,
    [Details]   VARCHAR (100) NULL,
    [Remarks]   VARCHAR (300) NULL,
    [Type]      TINYINT       NOT NULL,
    [IsSuccess] BIT           CONSTRAINT [DF_CRM_SkodaErrorCodes_IsSuccess] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CRM_SkodaErrorCodes] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-LMS, 2-DMS, 3-Status Check', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CRM_SkodaErrorCodes', @level2type = N'COLUMN', @level2name = N'Type';

