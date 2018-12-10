CREATE TABLE [dbo].[TC_Insurance_PaymentLog] (
    [TC_Insurance_PaymentLogId] INT           IDENTITY (1, 1) NOT NULL,
    [TC_Insurance_InquiryId]    INT           NULL,
    [PaymentMode]               TINYINT       NULL,
    [PaymentConfirmationId]     VARCHAR (100) NULL,
    [ChequeNumber]              VARCHAR (20)  NULL,
    [CoverNoteNo]               VARCHAR (100) NULL,
    [PaymentDate]               DATETIME      NULL,
    [CollectionDateTime]        DATETIME      NULL,
    [ChequePickUpAddress]       VARCHAR (200) NULL,
    [PaymentMethod]             TINYINT       NULL,
    [ModifiedBy]                INT           NULL,
    [ModifiedOn]                DATETIME      NULL,
    CONSTRAINT [PK_TC_Insurance_PaymentLog] PRIMARY KEY CLUSTERED ([TC_Insurance_PaymentLogId] ASC)
);

