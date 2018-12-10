CREATE TABLE [dbo].[TC_CarTradeTyreCondition] (
    [TC_CarTradeTyreConditionID]     INT          IDENTITY (1, 1) NOT NULL,
    [TC_CarTradeCertificationDataID] INT          NULL,
    [Tyretype]                       VARCHAR (20) NULL,
    [value]                          SMALLINT     NULL
);

