CREATE TABLE [dbo].[CRM_SkodaComments] (
    [Id]           NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [LeadId]       NUMERIC (18) NOT NULL,
    [TokenId]      VARCHAR (50) NOT NULL,
    [status]       SMALLINT     NOT NULL,
    [StatusDesc]   VARCHAR (50) NOT NULL,
    [ResultString] NCHAR (1000) NULL,
    [SentOn]       DATETIME     CONSTRAINT [DF_Table_1_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [SentBy]       NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_CRM_SkodaComments] PRIMARY KEY CLUSTERED ([Id] ASC)
);

