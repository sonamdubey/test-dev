CREATE TABLE [dbo].[TC_RTO] (
    [TC_RTO_Id]    INT          IDENTITY (1, 1) NOT NULL,
    [DealerId]     INT          NOT NULL,
    [RTOName]      VARCHAR (50) NOT NULL,
    [IsActive]     BIT          CONSTRAINT [DF_TC_RTO_IsActive] DEFAULT ((1)) NOT NULL,
    [EntryDate]    DATETIME     CONSTRAINT [DF_TC_RTO_EntryDate] DEFAULT (getdate()) NULL,
    [ModifiedDate] DATETIME     NULL,
    [ModifiedBy]   INT          NULL,
    CONSTRAINT [PK_TC_RTO] PRIMARY KEY CLUSTERED ([TC_RTO_Id] ASC)
);

