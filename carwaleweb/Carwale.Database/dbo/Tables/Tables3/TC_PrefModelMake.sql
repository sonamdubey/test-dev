CREATE TABLE [dbo].[TC_PrefModelMake] (
    [TC_PrefModelMakeId]  INT          IDENTITY (1, 1) NOT NULL,
    [TC_BuyerInquiriesId] INT          NULL,
    [ModelId]             NUMERIC (18) NULL,
    [CreatedOn]           DATETIME     CONSTRAINT [DF_Tc_PrefModelMake_CreatedOn] DEFAULT (getdate()) NULL,
    [IsActive]            BIT          CONSTRAINT [DF_Tc_PrefModelMake_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_Tc_PrefModelMake_id] PRIMARY KEY NONCLUSTERED ([TC_PrefModelMakeId] ASC)
);

