CREATE TABLE [dbo].[TC_BWLeadStatusLog] (
    [TC_BWLeadStatusLogId] INT      IDENTITY (1, 1) NOT NULL,
    [TC_BWLeadStatusId]    TINYINT  NOT NULL,
    [TC_InquiryLeadId]     INT      NOT NULL,
    [CreatedOn]            DATETIME NOT NULL,
    [CreatedBy]            INT      NOT NULL
);

