CREATE TABLE [dbo].[TC_DealerConfiguration] (
    [Id]              INT IDENTITY (1, 1) NOT NULL,
    [DealerId]        INT NOT NULL,
    [isWorksheetOnly] BIT NOT NULL,
    [freshLeadCount]  INT NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Only worksheet tab will display to that dealer(single user case) for that  isWorksheet will be 1 else 0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_DealerConfiguration', @level2type = N'COLUMN', @level2name = N'isWorksheetOnly';

