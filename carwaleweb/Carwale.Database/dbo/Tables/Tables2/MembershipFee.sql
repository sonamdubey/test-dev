CREATE TABLE [dbo].[MembershipFee] (
    [ID]       NUMERIC (18) NOT NULL,
    [DealerId] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_MembershipFee] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_MembershipFee_Dealers] FOREIGN KEY ([DealerId]) REFERENCES [dbo].[Dealers] ([ID]) ON UPDATE CASCADE
);

