CREATE TABLE [dbo].[NCD_SmsAddress] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [MobileNo]   VARCHAR (12)  NOT NULL,
    [SmsAddress] VARCHAR (140) NOT NULL,
    CONSTRAINT [PK_NCD_SmsAddress] PRIMARY KEY CLUSTERED ([Id] ASC)
);

