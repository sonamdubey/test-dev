CREATE TABLE [dbo].[TC_PrefBodyStyle] (
    [TC_PrefBodyStyleId]  INT          IDENTITY (1, 1) NOT NULL,
    [TC_BuyerInquiriesId] INT          NULL,
    [BodyType]            NUMERIC (18) NULL,
    [CreatedOn]           DATETIME     CONSTRAINT [DF_Tc_PrefBodyStyle_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [IsActive]            BIT          CONSTRAINT [DF_Tc_PrefBodyStyle_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Tc_PrefBodyStyle_id] PRIMARY KEY NONCLUSTERED ([TC_PrefBodyStyleId] ASC),
    CONSTRAINT [TC_PrefBodyStyle_CarBodyStyles] FOREIGN KEY ([BodyType]) REFERENCES [dbo].[CarBodyStyles] ([ID])
);

