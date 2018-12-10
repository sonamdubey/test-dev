CREATE TABLE [dbo].[TC_TimeDuration] (
    [TC_TimeDuration_Id] INT           IDENTITY (1, 1) NOT NULL,
    [Programme_Name]     VARCHAR (100) NULL,
    [Starttime]          DATETIME      NULL,
    [Endtime]            DATETIME      NULL,
    [ProgrammDescr]      VARCHAR (200) NULL,
    [ReqTime]            DATETIME      NULL
);

