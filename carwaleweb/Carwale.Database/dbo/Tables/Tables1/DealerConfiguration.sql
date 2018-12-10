CREATE TABLE [dbo].[DealerConfiguration] (
    [DealerId]      NUMERIC (18) NOT NULL,
    [SMSAlert]      BIT          CONSTRAINT [DF_DealerConfiguration_SMSAlert] DEFAULT ((1)) NOT NULL,
    [EmailAlert]    BIT          CONSTRAINT [DF_DealerConfiguration_EmailAlert] DEFAULT ((1)) NOT NULL,
    [LastUpdatedOn] DATETIME     CONSTRAINT [DF_DealerConfiguration_LastUpdatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK__DealerCo__CA2F8EB223624F60] PRIMARY KEY CLUSTERED ([DealerId] ASC)
);

