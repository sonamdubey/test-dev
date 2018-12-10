IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InsertMessageBoard]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InsertMessageBoard]
GO

	/*
Created By:Vishal Srivastava-AE1830
Date:October 23,2013
Purpose: To insert messages by Special Users
*/

CREATE PROCEDURE [dbo].[TC_InsertMessageBoard]
@TC_SpecialUsersId INT,
@Subject VARCHAR(150),
@Message VARCHAR(MAX),
@TC_UserTypesForMessageAlertId INT,
@MessageStartDate DATE,
@MessageEndDate DATE
AS
BEGIN
		INSERT INTO  TC_MessageBoardDetail (TC_SpecialUsersId,[Subject],[Message], TC_UserTypesForMessageAlertId,MessageStartDate,MessageEndDate)
		VALUES (@TC_SpecialUsersId,@Subject,@Message,@TC_UserTypesForMessageAlertId,@MessageStartDate,@MessageEndDate)
END
