CREATE TABLE [dbo].[TC_PaymentReceived] (
    [TC_PaymentReceived_Id] BIGINT        IDENTITY (1, 1) NOT NULL,
    [TC_CarBooking_Id]      INT           NOT NULL,
    [TC_PaymentOptions_Id]  TINYINT       NOT NULL,
    [UserId]                INT           NOT NULL,
    [AmountReceived]        DECIMAL (18)  NOT NULL,
    [PaymentType]           TINYINT       NOT NULL,
    [PayDate]               DATE          NOT NULL,
    [ChequeNo]              VARCHAR (20)  NULL,
    [ChequeDate]            DATE          NULL,
    [BankName]              VARCHAR (50)  NULL,
    [IsCleared]             BIT           CONSTRAINT [DF_TC_PaymentReceived_IsCleared] DEFAULT ((1)) NOT NULL,
    [Remarks]               VARCHAR (100) NULL,
    [IsActive]              BIT           CONSTRAINT [DF_TC_PaymentReceived_IsActive] DEFAULT ((1)) NOT NULL,
    [EntryDate]             DATETIME      DEFAULT (getdate()) NULL,
    [ModifiedBy]            INT           NULL,
    [ModifiedDate]          DATETIME      NULL,
    CONSTRAINT [PK_TC_PaymentReceived] PRIMARY KEY CLUSTERED ([TC_PaymentReceived_Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fk From TC_Payment table', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_PaymentReceived', @level2type = N'COLUMN', @level2name = N'TC_PaymentReceived_Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fk From TC_Payment option table', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_PaymentReceived', @level2type = N'COLUMN', @level2name = N'TC_CarBooking_Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'First payment would be for ''Token'' hence value is 1 & other would be ''Balance'' and value is 2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_PaymentReceived', @level2type = N'COLUMN', @level2name = N'PaymentType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fk from TC_Users', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_PaymentReceived', @level2type = N'COLUMN', @level2name = N'PayDate';

