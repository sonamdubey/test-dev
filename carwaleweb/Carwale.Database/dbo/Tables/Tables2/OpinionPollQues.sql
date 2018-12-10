﻿CREATE TABLE [dbo].[OpinionPollQues] (
    [ID]         NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Question]   VARCHAR (500) NOT NULL,
    [startDate]  DATETIME      NOT NULL,
    [CategoryId] NUMERIC (18)  NULL,
    CONSTRAINT [PK_Table1_1] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

