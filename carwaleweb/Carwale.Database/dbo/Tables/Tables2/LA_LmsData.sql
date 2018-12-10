CREATE TABLE [dbo].[LA_LmsData] (
    [LAId]       NUMERIC (18)  NOT NULL,
    [TokenNo]    VARCHAR (50)  NULL,
    [LeadStatus] VARCHAR (50)  NULL,
    [ResultCode] VARCHAR (50)  NULL,
    [PushStatus] VARCHAR (200) NULL,
    [DmsId]      VARCHAR (50)  NULL,
    [CreatedOn]  DATETIME      CONSTRAINT [DF_LA_LmsData_CreatedOn] DEFAULT (getdate()) NULL
);

