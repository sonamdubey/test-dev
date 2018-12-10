CREATE TABLE [dbo].[TC_Alerts] (
    [TC_Alerts_Id]     BIGINT       IDENTITY (1, 1) NOT NULL,
    [BranchId]         NUMERIC (18) NOT NULL,
    [AlertType_Id]     INT          NOT NULL,
    [RequestedDate]    DATE         CONSTRAINT [DF_TC_Alerts_RequestedDate] DEFAULT (getdate()) NOT NULL,
    [Status]           BIT          NULL,
    [ResponseDatetime] DATETIME     NULL
);

