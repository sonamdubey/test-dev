CREATE TABLE [dbo].[TC_CarTradeCertificationAction] (
    [TC_CarTradeCertificationActionId] INT      IDENTITY (1, 1) NOT NULL,
    [ListingId]                        INT      NOT NULL,
    [ActionType]                       TINYINT  NOT NULL,
    [IsProcessed]                      BIT      NULL,
    [ProcessDate]                      DATETIME NULL,
    [EntryDate]                        DATETIME NOT NULL,
    CONSTRAINT [PK_TC_CarTradeCertificationAction] PRIMARY KEY CLUSTERED ([TC_CarTradeCertificationActionId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ActionType : 1 . Update 2. Delete', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_CarTradeCertificationAction', @level2type = N'COLUMN', @level2name = N'ActionType';

