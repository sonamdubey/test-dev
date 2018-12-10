CREATE TABLE [dbo].[CW_BusinessUnits] (
    [BU_Id]     TINYINT      IDENTITY (1, 1) NOT NULL,
    [Name]      VARCHAR (50) NOT NULL,
    [CreatedOn] DATETIME     CONSTRAINT [DF_CW_BusinessUnits_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [IsActive]  BIT          CONSTRAINT [DF_CW_BusinessUnits_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CW_BusinessUnits] PRIMARY KEY CLUSTERED ([BU_Id] ASC)
);

