CREATE TABLE [dbo].[DCRM_MeetingReason] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [MeetingId]   NUMERIC (18)  NOT NULL,
    [ReasonId]    TINYINT       NOT NULL,
    [OtherReason] VARCHAR (100) NULL
);

