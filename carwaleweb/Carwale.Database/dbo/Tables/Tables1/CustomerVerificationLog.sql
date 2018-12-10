CREATE TABLE [dbo].[CustomerVerificationLog] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustID]    NUMERIC (18) NOT NULL,
    [UpdatedBy] NUMERIC (18) NOT NULL,
    [type]      BIT          NOT NULL,
    [EntryDate] DATETIME     NOT NULL,
    CONSTRAINT [PK_CustomerVerificationLog] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0-Fake, 1-Verified', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustomerVerificationLog', @level2type = N'COLUMN', @level2name = N'type';

