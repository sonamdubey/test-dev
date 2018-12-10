CREATE TABLE [dbo].[ZipDial_Transactions] (
    [ID]                       INT          IDENTITY (1, 1) NOT NULL,
    [ClientTransactionId]      VARCHAR (50) NULL,
    [ZipDialNumber]            VARCHAR (15) NULL,
    [TransactionToken]         VARCHAR (50) NULL,
    [MobileNumber]             VARCHAR (15) NOT NULL,
    [IsVerified]               BIT          CONSTRAINT [DF_ZipDial_Transactions_IsVerified] DEFAULT ((0)) NOT NULL,
    [InquiryId]                VARCHAR (10) NULL,
    [SellerType]               TINYINT      NULL,
    [CreatedTime]              DATETIME     NOT NULL,
    [TokenResponseTime]        DATETIME     NULL,
    [VerificationResponseTime] DATETIME     NULL,
    [Email]                    VARCHAR (50) NULL,
    [Source]                   VARCHAR (25) NULL,
    [PlatformId]               SMALLINT     NULL,
    CONSTRAINT [PK_ZipDial_Transactions] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_ZipDial_Transactions_MobileNumber]
    ON [dbo].[ZipDial_Transactions]([MobileNumber] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_ZipDial_TransactionToken]
    ON [dbo].[ZipDial_Transactions]([TransactionToken] ASC);

