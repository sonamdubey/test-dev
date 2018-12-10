CREATE TABLE [dbo].[LDBankCarLoanForward] (
    [LBF_Id]           NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [LoanId]           NUMERIC (18)   NULL,
    [BankId]           NUMERIC (18)   NULL,
    [ReferenceNo]      VARCHAR (100)  NULL,
    [Status]           SMALLINT       CONSTRAINT [DF_LDBankCarLoanForward_Status] DEFAULT ((-1)) NULL,
    [Reason]           VARCHAR (1000) NULL,
    [ForwardedDate]    DATETIME       NULL,
    [StatusChangeDate] DATETIME       NULL,
    CONSTRAINT [PK_LDBankCarLoanForward] PRIMARY KEY CLUSTERED ([LBF_Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Accepted, 2-Rejected, 3-Neutral', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LDBankCarLoanForward', @level2type = N'COLUMN', @level2name = N'Status';

