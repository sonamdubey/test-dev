CREATE TABLE [dbo].[CH_FollowEVal] (
    [Id]          NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [RecordDate]  DATETIME     CONSTRAINT [DF_CH_FollowEVal_RecordDate] DEFAULT (getdate()) NOT NULL,
    [OB]          NUMERIC (18) NULL,
    [Paid]        NUMERIC (18) NULL,
    [NewAddition] NUMERIC (18) NULL,
    [Discarded]   NUMERIC (18) NULL,
    [CB]          NUMERIC (18) NULL,
    CONSTRAINT [PK_CH_FollowEVal] PRIMARY KEY CLUSTERED ([Id] ASC)
);

