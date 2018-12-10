CREATE TABLE [dbo].[CRM_IB_Offers] (
    [id]        INT           IDENTITY (1, 1) NOT NULL,
    [Offer]     VARCHAR (200) NOT NULL,
    [StartDate] DATETIME      NULL,
    [EndDate]   DATETIME      NULL,
    [MakeId]    NUMERIC (18)  NULL,
    [IsActive]  BIT           CONSTRAINT [DF_CRM_IB_Offers_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedBy] NUMERIC (18)  NULL,
    [CreatedOn] DATETIME      CONSTRAINT [DF_CRM_IB_Offers_CreatedOn] DEFAULT (getdate()) NULL,
    [UpdatedBy] NUMERIC (18)  NULL,
    [UpdatedOn] DATETIME      NULL,
    CONSTRAINT [PK_CRM_IB_Offers] PRIMARY KEY CLUSTERED ([id] ASC)
);

