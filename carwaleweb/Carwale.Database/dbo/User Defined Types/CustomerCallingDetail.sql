CREATE TYPE [dbo].[CustomerCallingDetail] AS TABLE (
    [CustomerId]  INT           NULL,
    [InquiryDate] VARCHAR (100) NULL,
    [EntryDate]   DATETIME      NULL,
    [EnteredBy]   INT           NULL,
    [CExecutive]  VARCHAR (100) NULL);

