CREATE TABLE [dbo].[NCD_InquiryFollowup] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [InquiryId]         INT           NULL,
    [CustomerId]        INT           NULL,
    [LastCallTime]      DATETIME      NULL,
    [NextCallTime]      DATETIME      NULL,
    [LeadStatusId]      TINYINT       NULL,
    [Comment]           VARCHAR (500) NULL,
    [AssignedExecutive] INT           NULL,
    [TestDriveDate]     DATETIME      NULL,
    CONSTRAINT [PK_NCD_InquiryFollowup] PRIMARY KEY CLUSTERED ([Id] ASC)
);

