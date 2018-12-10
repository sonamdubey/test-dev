CREATE TABLE [dbo].[TC_Exceptions] (
    [TC_Exception_Id]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [Programme_Name]    VARCHAR (100) NULL,
    [TC_Exception]      VARCHAR (MAX) NULL,
    [TC_Exception_Date] DATETIME      NULL,
    [InputParameters]   VARCHAR (MAX) DEFAULT (NULL) NULL
);

