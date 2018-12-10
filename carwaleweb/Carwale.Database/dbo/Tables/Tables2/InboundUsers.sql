CREATE TABLE [dbo].[InboundUsers] (
    [InboundUserId]   SMALLINT      IDENTITY (1, 1) NOT NULL,
    [OprUserId]       INT           NULL,
    [InboundUserName] VARCHAR (100) NULL,
    [IsActive]        BIT           DEFAULT ((1)) NULL,
    [CreatedOn]       DATETIME      NULL
);

