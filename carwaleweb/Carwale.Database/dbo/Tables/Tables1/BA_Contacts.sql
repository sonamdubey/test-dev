CREATE TABLE [dbo].[BA_Contacts] (
    [ID]         INT      IDENTITY (1, 1) NOT NULL,
    [BrokerId]   INT      NOT NULL,
    [CreatedOn]  DATETIME NULL,
    [ModifyDate] DATETIME NULL,
    [DeletedOn]  DATETIME NULL,
    [IsActive]   BIT      CONSTRAINT [DF_BA_Contacts_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_BA_Contacts] PRIMARY KEY CLUSTERED ([ID] ASC)
);

