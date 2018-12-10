CREATE TABLE [dbo].[HDFCDealerRepresentatives] (
    [DealerId]       NUMERIC (18)  NOT NULL,
    [Representative] VARCHAR (50)  NOT NULL,
    [Mobile]         VARCHAR (10)  NULL,
    [Email]          VARCHAR (100) NULL,
    [EntryDate]      DATETIME      CONSTRAINT [DF_HDFCDealerRepresentatives_EntryDate] DEFAULT (getdate()) NOT NULL,
    [IsActive]       BIT           CONSTRAINT [DF_HDFCDealerRepresentatives_IsActive] DEFAULT ((1)) NOT NULL,
    [ASM]            VARCHAR (50)  NULL,
    [ASMEmailId]     VARCHAR (100) NULL,
    [ASMContactNo]   VARCHAR (10)  NULL,
    CONSTRAINT [PK_HDFCDealerRepresentatives] PRIMARY KEY CLUSTERED ([DealerId] ASC)
);

