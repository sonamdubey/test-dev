CREATE TABLE [dbo].[TC_UserTypesForMessageAlert] (
    [TC_UserTypesForMessageAlertId] INT          IDENTITY (1, 1) NOT NULL,
    [UserType]                      VARCHAR (50) NULL,
    [IsActive]                      BIT          CONSTRAINT [DF_TC_UserTypesForMessageAlert_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_TC_UserTypesForMessageAlertId] PRIMARY KEY NONCLUSTERED ([TC_UserTypesForMessageAlertId] ASC)
);

