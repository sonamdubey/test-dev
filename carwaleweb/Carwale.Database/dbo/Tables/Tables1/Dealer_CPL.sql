CREATE TABLE [dbo].[Dealer_CPL] (
    [Organization] VARCHAR (100)    NOT NULL,
    [DealerId]     NUMERIC (18)     NOT NULL,
    [CreatedOn]    DATETIME         NOT NULL,
    [entrydate]    DATETIME         NOT NULL,
    [ClosingDate]  DATETIME         NULL,
    [CPL]          NUMERIC (29, 11) NULL
);

