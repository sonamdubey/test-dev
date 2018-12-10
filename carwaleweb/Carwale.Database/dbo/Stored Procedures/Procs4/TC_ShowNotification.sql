IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ShowNotification]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ShowNotification]
GO

	/*
Created By:Vishal Srivastava-AE1830
Date:October 24,2013
Purpose: To Show messages of Special Users
*/

CREATE PROCEDURE [dbo].[TC_ShowNotification]
@type INT
AS
BEGIN
		SELECT 
		MBD.[Subject],
		MBD.[Message],
		SP.UserName AS [From],
		MBD.MessageStartDate AS [Date]
		FROM TC_MessageBoardDetail AS MBD WITH(NOLOCK)
		INNER JOIN TC_SpecialUsers AS SP WITH(NOLOCK)
		ON SP.TC_SpecialUsersId=MBD.TC_SpecialUsersId
		WHERE CONVERT (DATE,GETDATE()) BETWEEN MessageStartDate AND MessageEndDate
	    AND	( MBD.TC_UserTypesForMessageAlertId=3  OR  MBD.TC_UserTypesForMessageAlertId=@type)
END

