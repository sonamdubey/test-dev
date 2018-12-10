CREATE TABLE [dbo].[NewCarShowroomInquiries] (
    [Id]              NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId]      NUMERIC (18)   NOT NULL,
    [Message]         VARCHAR (1000) NOT NULL,
    [RequestDateTime] DATETIME       NOT NULL,
    [isApprove]       BIT            CONSTRAINT [DF_NewCarShowroomInquiries_isActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_NewCarShowroomInquiries] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

