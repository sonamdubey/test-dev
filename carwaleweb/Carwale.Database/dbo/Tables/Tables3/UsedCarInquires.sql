CREATE TABLE [dbo].[UsedCarInquires] (
    [ProfileId]         NUMERIC (18) NOT NULL,
    [CustomerId]        NUMERIC (18) NOT NULL,
    [Mobile]            VARCHAR (20) NULL,
    [LoginId]           VARCHAR (50) NOT NULL,
    [UserName]          VARCHAR (50) NOT NULL,
    [CarListedDate]     DATETIME     NOT NULL,
    [ScheduledDateTime] DATETIME     NOT NULL,
    [CalledDateTime]    DATETIME     NOT NULL,
    [calltype]          SMALLINT     NOT NULL
);

