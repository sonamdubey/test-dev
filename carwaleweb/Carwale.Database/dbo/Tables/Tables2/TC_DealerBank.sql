CREATE TABLE [dbo].[TC_DealerBank] (
    [TC_DealerBank_Id] INT          IDENTITY (1, 1) NOT NULL,
    [DealerId]         NUMERIC (18) NOT NULL,
    [BankName]         VARCHAR (50) NOT NULL,
    [IsActive]         BIT          CONSTRAINT [DF_TC_DealerBank_IsActive] DEFAULT ((1)) NOT NULL,
    [EntryDate]        DATETIME     CONSTRAINT [DF_TC_DealerBank_EntryDate] DEFAULT (getdate()) NULL,
    [ModifiedDate]     DATETIME     NULL,
    [ModifiedBy]       INT          NULL,
    CONSTRAINT [PK_TC_DealerBank] PRIMARY KEY CLUSTERED ([TC_DealerBank_Id] ASC)
);

