CREATE TABLE [dbo].[InquiryPointCategory] (
    [Id]          INT           NOT NULL,
    [Name]        VARCHAR (50)  NOT NULL,
    [isActive]    BIT           CONSTRAINT [DF_InquiryPointCategory_isActive] DEFAULT (1) NOT NULL,
    [Description] VARCHAR (500) NULL,
    [IsPremium]   BIT           DEFAULT ((0)) NULL,
    [IsDealer]    BIT           NULL,
    [GroupType]   INT           NULL,
    CONSTRAINT [PK_InquiryPointCategory] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

