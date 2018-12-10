CREATE TABLE [dbo].[AxisBank_UserActivitiesLog] (
    [ActivityId]       NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [UserId]           INT          NOT NULL,
    [ActivityTypeId]   SMALLINT     NOT NULL,
    [ActivityDateTime] DATETIME     NOT NULL,
    [ClientIP]         VARCHAR (15) NULL,
    CONSTRAINT [PK_AxisBank_UserActivitiesLog] PRIMARY KEY CLUSTERED ([ActivityId] ASC)
);

